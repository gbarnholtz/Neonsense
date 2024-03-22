using Drawing;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using Unity.VisualScripting;
using UnityEngine;

public enum PlayerMoveState {
    Default,
    Dash,
    Boost
}

[RequireComponent(typeof(Rigidbody))]
public class CharacterMotor : SerializedMonoBehaviour, IInputModifier
{
    public event Action<bool> OnGroundedChanged;
    public event Action OnHardLanding;

    [OdinSerialize] private ICharacterInputProvider inputProvider;

    public int priority => 5;
    private InputState inputState;
    [Header("Physical Properties")]
    [SerializeField] private float gravity;
    [SerializeField] private float terminalVelocity;
    [SerializeField, Range(0, 1)] private float hardLandingThreshold = 0.9f;
    [SerializeField] private float friction, airFriction;
    [Header("Movement Properties")]
    [SerializeField] private float maxSpeed;
    [SerializeField] private float sprintSpeedScalar;
    [SerializeField] private float groundAccelerationScalar, airAccelerationScalar;
    [SerializeField, Range(0, 1)] private float rotationLerp;
    [SerializeField, Range(0, 90)] private float maxSlope;
    [SerializeField, Range(0, 1)] private float slopeJumpBias;
    private float slopeDotProduct;
    [SerializeField, Range(0, 10)] private float jumpHeight;
    [SerializeField] private int maxAirJumps;
    [SerializeField] LayerMask groundMask;
    
    
    public Vector3 getNormal { get; private set; }
    public Vector3 getSteepNormal { get; private set; }

    [Header("Read Only")]
    [SerializeField] private Vector3 contactNormal; 
    [SerializeField] private Vector3 steepContactNormal;
    [SerializeField] private int airJumps, groundContactCount;
    private bool shouldJump;
    
    public bool IsGrounded { get; private set; } 
    private bool grounded => groundContactCount > 0;
    public Vector3 velocity => rb.velocity;
    private Vector3 projectedVelocity => ProjectOnContactPlane(velocity);
    private Vector3 targetVelocity;
    private Vector3 projectedTargetVelocity => ProjectOnContactPlane(targetVelocity);
    public Vector3 position => transform.position;

    [SerializeField] private float targetRotationAngle;
    private bool previouslyGrounded;
    private Rigidbody rb;
    private ContactPoint[] contactBuffer = new ContactPoint[10];

    void OnValidate() {
        slopeDotProduct = Mathf.Cos(maxSlope * Mathf.Deg2Rad);
    }

    void Awake() {
        rb = GetComponent<Rigidbody>();
        rb.maxAngularVelocity = 50;
        rb.sleepThreshold = 0.0f;
        OnValidate();
    }

    private void OnEnable() {
        inputProvider.Jump.started += TryJump;
    }

    void Update() {
        inputState = inputProvider.GetState();
    }
    /*
    void LateUpdate() {
        using (Draw.WithColor(Color.blue))
        {
            Draw.ArrowheadArc(transform.position, inputState.moveDirection, 0.5f, 30f);
            Draw.Cross(inputState.aimPoint);
        }

        using (Draw.WithColor(Color.green))
        {
            Draw.ArrowheadArc(transform.position, transform.forward, 0.5f);
            Draw.Arrow(transform.position - Vector3.up, transform.position - Vector3.up+getNormal);
        }
    }
    */
    void FixedUpdate() {
        contactNormal = getNormal = grounded ? contactNormal.normalized : Vector3.up;
        getSteepNormal = steepContactNormal.normalized;

        IsGrounded = grounded;
        if (grounded != previouslyGrounded) {
            OnGroundedChanged?.Invoke(grounded);
            if (grounded) airJumps = maxAirJumps;
        }
        
        UpdateTargetsFromInput();
        ApplyGravity();
        RotatePlayer();
        MovePlayer();
        ApplyFriction();

        if (shouldJump)
        {
            shouldJump = false;
            Jump();
        }

        previouslyGrounded = grounded;
        groundContactCount = 0;
        contactNormal = Vector3.zero;
        steepContactNormal = Vector3.zero;
    }

    private void UpdateTargetsFromInput()
    {
        targetVelocity = Vector3.zero;
        if (inputState.moveDirection.magnitude == 0) return;
        targetVelocity = inputState.moveDirection * maxSpeed;
        targetRotationAngle = (Mathf.Rad2Deg * Mathf.Atan2(-inputState.moveDirection.z, inputState.moveDirection.x) + 360+90) % 360;
    }

    private void MovePlayer() {
        if (!grounded && targetVelocity.magnitude < 0.01f) return;
        float maxAcceleration = maxSpeed * (grounded ? groundAccelerationScalar : airAccelerationScalar);
        if (getSteepNormal.magnitude > 0 && Vector3.Dot(targetVelocity, getSteepNormal) < 0) targetVelocity = Vector3.ProjectOnPlane(targetVelocity, getSteepNormal);
        rb.AddForce(Vector3.ClampMagnitude(ProjectOnContactPlane(targetVelocity - velocity), maxAcceleration * Time.fixedDeltaTime), ForceMode.VelocityChange);
    }

    private void RotatePlayer() {
        if (inputState.shouldLookAtAim) targetRotationAngle = Vector3.SignedAngle(Vector3.forward, inputState.aimPoint - transform.position, Vector3.up);
        float velocityToNext = Mathf.DeltaAngle(transform.eulerAngles.y, targetRotationAngle) * Mathf.Deg2Rad/Time.fixedDeltaTime*rotationLerp;
        rb.AddTorque(Vector3.up * (velocityToNext  - rb.angularVelocity.y), ForceMode.VelocityChange);
    }

    private void ApplyFriction()
    {
        Vector3 frictionForce = ProjectOnContactPlane(-velocity);
        if (getSteepNormal.magnitude > 0 && Vector3.Dot(frictionForce, getSteepNormal) < 0)
        {
            frictionForce = Vector3.ProjectOnPlane(frictionForce, getSteepNormal);
        }
        rb.AddForce(Vector3.ClampMagnitude(frictionForce, (grounded ? friction : airFriction) * Time.fixedDeltaTime), ForceMode.VelocityChange);
    }

    private void ApplyGravity() {
        if (velocity.y < -terminalVelocity) return;
        rb.AddForce(Vector3.ClampMagnitude((-terminalVelocity - velocity.y ) * Vector3.up, gravity * Time.fixedDeltaTime), ForceMode.VelocityChange);
    }

    private void TryJump() {
        if (grounded)
        {
            shouldJump = true;
        }
        else if (airJumps > 0)
        {
            shouldJump = true;
            airJumps--;
        }
    }

    private void Jump() {
        float jumpSpeed = Mathf.Sqrt(2f * gravity * jumpHeight);
        if (velocity.y != 0) rb.AddForce(new Vector3(0, -velocity.y, 0), ForceMode.VelocityChange);
        rb.AddForce(Vector3.Lerp(Vector3.up, getNormal, slopeJumpBias) * jumpSpeed, ForceMode.VelocityChange);
    }

    private void OnCollisionEnter(Collision collision) {
        EvaluateGroundCollision(collision);
        if (collision.relativeVelocity.y > terminalVelocity*hardLandingThreshold) OnHardLanding?.Invoke();
    } 

    private void OnCollisionStay(Collision collision) => EvaluateGroundCollision(collision);

    private void EvaluateGroundCollision(Collision collision)
    {
        int layer = collision.gameObject.layer;
        if (Utilities.IsInLayer(groundMask, layer)) {
            
            for (int i = 0; i < collision.GetContacts(contactBuffer); i++) {
                Vector3 normal = collision.GetContact(i).normal;
                if (normal.y >= slopeDotProduct) {
                    groundContactCount++;
                    contactNormal += normal;
                } else {
                    steepContactNormal += normal;
                }
            }
        }
    }

    private Vector3 ProjectOnContactPlane(Vector3 vector) {
        return vector - contactNormal * Vector3.Dot(vector, contactNormal);
    }

    public InputState ModifyInput(InputState input) {
        //TODO: Maybe lock input state to forward while sprinting
        return input;
    }
}
