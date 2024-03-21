using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using UnityEngine;
using Drawing;

[RequireComponent(typeof(SphereCollider), typeof(Rigidbody))]
public class PlayerStateMotor : SerializedMonoBehaviour
{
    [OdinSerialize] private IInputProvider inputProvider;
    private InputState inputState;
    public event Action<bool> OnGroundedChanged;
    private bool previouslyGrounded;
    [Header("Player Spring Properties")]
    [SerializeField] private float height=2f;
    public SphereCollider Head { get; private set; }
    [SerializeField] private float headSpringForce=500f, headSpringDamper=50f, groundSnappingDistance=0.5f, springAdjustSpeed=2f;
    private float springRadius, targetHeadHeight, currentHeadHeight;

    [Header("Move State Properties")]
    [ShowInInspector] private MoveState activeState;
    public WalkMoveState WalkState = new WalkMoveState();
    public SlideMoveState SlideState = new SlideMoveState();
    public WallRunMoveState WallRunState = new WallRunMoveState();

    public bool TryingToStartSlide => SlideReset && TryingToSlide;

    [Header("Uniform Movement Properties")]
    [SerializeField] private float gravity = 9.81f;
    [SerializeField] private float terminalVelocity=15f;
    private bool shouldJump;
    public float BaseSpeed=6;
    [SerializeField, Range(0, 90)] private float maxSlope=45f;
    [SerializeField, Range(0, 1)] private float slopeJumpBias=0.5f;
    private float slopeDotProduct;
    [SerializeField, Range(0, 10)] private float jumpHeight=2f;
    [SerializeField] private int maxAirJumps;
    [SerializeField] LayerMask groundMask;
    
    private Rigidbody rb;
    [field: SerializeField] public Vector3 ContactNormal { get; private set; }
    public bool IsGrounded => ContactNormal != Vector3.zero;
    public Vector3 TargetDirection => inputState.moveDirection;
    public Vector3 Velocity => rb.velocity;
    public Vector3 LookDirection => inputState.lookDirection;

    [Header("Read Only (DEBUG)")]
    [ShowInInspector] private bool disableGroundSnapping;
    public bool TryingToSlide, SlideReset = true;

    private void Awake()
    {
        Head = GetComponent<SphereCollider>();
        rb = GetComponent<Rigidbody>();
        OnValidate();
        InitializeStates();
        ChangeState(WalkState);
        currentHeadHeight = targetHeadHeight;
    }

    private void OnValidate()
    {
        if (Head != null) {
            springRadius = Head.radius - 0.01f;
            UpdateHeight(height);
        }
    }

    private void OnEnable()
    {
        inputProvider.Jump.started += TryJump;
        inputProvider.Slide.started += StartTrySlide;
        inputProvider.Slide.ended += EndTrySlide;
    }

    private void OnDisable()
    {
        inputProvider.Jump.started -= TryJump;
        inputProvider.Slide.started -= StartTrySlide;
        inputProvider.Slide.ended -= EndTrySlide;
    }

    private void FixedUpdate()
    {
        inputState = inputProvider.GetState();
        HandleGrounded();
        if (activeState.ShouldApplyGravity) ApplyGravity();
        activeState.Update();
        activeState.MovePlayer();

        if (shouldJump) {
            if (activeState.OverrideJump) activeState.Jump();
            else Jump();
            shouldJump = false;
        }
    }

    private void LateUpdate()
    {
        using (Draw.WithColor(Color.green))
        {
            Vector3 groundPoint = transform.position - Vector3.up * currentHeadHeight;
            Draw.SphereOutline(groundPoint, springRadius);
            Draw.Arrow(groundPoint, groundPoint + ContactNormal);
        }
        using (Draw.WithColor(Color.blue))
        {
            Draw.SphereOutline(transform.position - Vector3.up * targetHeadHeight, springRadius - 0.1f);
            Draw.ArrowheadArc(transform.position, TargetDirection, 0.55f);
        }
        activeState.DrawGizmos();
    }

    private void InitializeStates() {
        WalkState.Register(this);
        SlideState.Register(this);
        WallRunState.Register(this);
    }

    public void ChangeState(MoveState state) {
        activeState?.Exit();
        activeState = state;
        activeState.Enter();
        UpdateHeight(activeState.Height);
    }

    private void HandleGrounded() {
        Vector3 springDir = transform.up;
        float checkDistance = (previouslyGrounded && !disableGroundSnapping) ? currentHeadHeight + groundSnappingDistance : currentHeadHeight;
        if (Physics.SphereCast(transform.position, springRadius, -springDir, out RaycastHit hit, checkDistance, groundMask)) {
            float offset = targetHeadHeight - hit.distance;
            float springVelocity = Vector3.Dot(springDir, Velocity);
            float force = (offset * headSpringForce) - (springVelocity * headSpringDamper);
            ContactNormal = hit.normal;
            currentHeadHeight = hit.distance;
            disableGroundSnapping = false;
            //TODO: remove conditional wrapper
            //jumping during the window of the spring bouncing upwards causes forces to stack, sending the player super high
            //only way of truly fixing it is with GetAccumulatedForces() but that is in a later version of Unity
            //upgrading Unity versions would make it incompatible on hopkins computers 
            if (!shouldJump) rb.AddForce(force * springDir, ForceMode.Acceleration);
        } else {
            ContactNormal = Vector3.zero;
            currentHeadHeight = Mathf.MoveTowards(currentHeadHeight, targetHeadHeight, Time.fixedDeltaTime * springAdjustSpeed);
        }
        if (previouslyGrounded = IsGrounded) OnGroundedChanged?.Invoke(IsGrounded);
        previouslyGrounded = IsGrounded;
    }

    private void ApplyGravity()
    {
        if (Velocity.y < -terminalVelocity) return;
        Vector3 gravityForce = Vector3.ClampMagnitude((-terminalVelocity - Velocity.y) * Vector3.up, gravity * Time.fixedDeltaTime);
        rb.AddForce(Vector3.ProjectOnPlane(gravityForce, ContactNormal), ForceMode.VelocityChange);
    }

    private void Jump() => JumpDirectional(IsGrounded ? Vector3.Lerp(Vector3.up, ContactNormal, slopeJumpBias) : Vector3.up);

    public void JumpDirectional(Vector3 direction)
    {
        direction = direction.normalized;
        float jumpSpeed = Mathf.Sqrt(2f * gravity * jumpHeight);
        rb.AddForce(-Vector3.Project(Velocity, direction), ForceMode.VelocityChange);
        rb.AddForce(direction * jumpSpeed, ForceMode.VelocityChange);
        disableGroundSnapping = true;
    }

    private void TryJump() => shouldJump = activeState.CheckTryJump();

    public void StartTrySlide() {
        TryingToSlide = true;
    }

    public void EndTrySlide() {
        TryingToSlide = false;
        SlideReset = true;
    }

    private void CheckTryDash()
    {
        if (activeState.CheckTryDash())
        {
            disableGroundSnapping = true;
        }
    }

    private void UpdateHeight(float height) {
        this.height = height;
        targetHeadHeight = height - springRadius;
    }
}
