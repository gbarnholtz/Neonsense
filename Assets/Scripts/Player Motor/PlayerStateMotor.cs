using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using UnityEngine;
using Drawing;
using Unity.VisualScripting;

[RequireComponent(typeof(SphereCollider), typeof(Rigidbody))]
public class PlayerStateMotor : SerializedMonoBehaviour
{
    [OdinSerialize] private IInputProvider inputProvider;
    private InputState inputState;
    public event Action<bool> OnGroundedChanged;
    private bool previouslyGrounded;
    [Header("Player Spring Properties")]
    [SerializeField] private float height=2f;
    private SphereCollider head;
    [SerializeField] private float headSpringForce=500f, headSpringDamper=50f, groundSnappingDistance=0.5f, springAdjustSpeed=2f;
    private float springRadius, targetHeadHeight, currentHeadHeight;

    [Header("Player Move State Properties")]
    [ShowInInspector] private MoveState activeState;
    public WalkMoveState WalkState;
    public MidairMoveState MidairState;
    public SlideMoveState SlideState;
    public bool TryingToStartSlide => SlideReset && TryingToSlide;

    [Header("Uniform Properties")]
    [SerializeField] private float gravity = 9.81f;
    [SerializeField] private float terminalVelocity=15f;
    [SerializeField, Range(0, 1)] private float hardLandingThreshold = 0.9f;
    private bool shouldJump;
    [Header("Movement Properties")]
    public float MaxSpeed=6;
    [SerializeField, Range(0, 90)] private float maxSlope=45f;
    [SerializeField, Range(0, 1)] private float slopeJumpBias=0.5f;
    private float slopeDotProduct;
    [SerializeField, Range(0, 10)] private float jumpHeight=2f;
    [SerializeField] private int maxAirJumps;
    [SerializeField] LayerMask groundMask;
    
    private Rigidbody rb;
    [field: SerializeField] public Vector3 ContactNormal { get; private set; }
    public bool IsGrounded => ContactNormal != Vector3.zero;
    [HideInInspector] public Vector3 TargetVelocity;
    private Vector3 velocity => rb.velocity;

    [Header("Read Only (DEBUG)")]
    [ShowInInspector] private bool disableGroundSnapping;
    public bool TryingToSlide, SlideReset = true;

    private void Awake()
    {
        head = GetComponent<SphereCollider>();
        rb = GetComponent<Rigidbody>();
        OnValidate();
        InitializeStates();
        SetState(WalkState);
        currentHeadHeight = targetHeadHeight;
    }
    private void OnValidate()
    {
        if (head != null) {
            springRadius = head.radius - 0.01f;
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
        UpdateTargetsFromInput();
        HandleGrounded();
        activeState.Update();
        activeState.MovePlayer();

        if(shouldJump) { Jump(); shouldJump = false; }
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
        }
    }

    private void InitializeStates() {
        WalkState.Register(this);
        SlideState.Register(this);
        MidairState.Register(this);
    }

    public void SetState(MoveState state) {
        activeState?.Exit();
        activeState = state;
        activeState.Enter();
        UpdateHeight(activeState.Height);
    }

    private void UpdateTargetsFromInput() {
        inputState = inputProvider.GetState();
        TargetVelocity = inputState.moveDirection.magnitude == 0? Vector3.zero : inputState.moveDirection * MaxSpeed;
    }

    private void HandleGrounded() {
        Vector3 springDir = transform.up;
        currentHeadHeight = Mathf.MoveTowards(currentHeadHeight, targetHeadHeight, Time.fixedDeltaTime * springAdjustSpeed);
        float checkDistance = (previouslyGrounded && !disableGroundSnapping) ? currentHeadHeight + groundSnappingDistance : currentHeadHeight;
        if (Physics.SphereCast(transform.position, springRadius, -springDir, out RaycastHit hit, checkDistance, groundMask)) {
            float offset = targetHeadHeight - hit.distance;
            float springVelocity = Vector3.Dot(springDir, velocity);
            float force = (offset * headSpringForce) - (springVelocity * headSpringDamper);
            rb.AddForce(force * springDir, ForceMode.Acceleration);
            ContactNormal = hit.normal;
            currentHeadHeight = hit.distance;
            disableGroundSnapping = false;
        } else {
            ContactNormal = Vector3.zero;
        }
        previouslyGrounded = IsGrounded;
    }
    private void Jump()
    {
        float jumpSpeed = Mathf.Sqrt(2f * gravity * jumpHeight);
        rb.AddForce(new Vector3(0, -velocity.y, 0), ForceMode.VelocityChange);
        Vector3 jumpDirection = IsGrounded ? Vector3.Lerp(Vector3.up, ContactNormal, slopeJumpBias) : Vector3.up;
        rb.AddForce(jumpDirection * jumpSpeed, ForceMode.VelocityChange);
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
