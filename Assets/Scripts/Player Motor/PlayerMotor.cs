using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UIElements;
using Drawing;

[RequireComponent(typeof(SphereCollider), typeof(Rigidbody))]
public class PlayerMotor : SerializedMonoBehaviour
{
    [OdinSerialize] private IInputProvider inputProvider;
    private InputState inputState;
    public event Action<bool> OnGroundedChanged;
    private bool previouslyGrounded;
    [Header("Player Spring Properties")]
    [SerializeField] private float height=2f;
    private float headHeight;
    private SphereCollider head;
    [SerializeField] private float headSpringForce=500f, headSpringDamper=50f, groundSnappingDistance=0.5f;

    private float springRadius;

    [Header("Physical Properties")]
    [SerializeField] private float gravity = 9.81f, terminalVelocity=15f;
    [SerializeField, Range(0, 1)] private float hardLandingThreshold = 0.9f;
    [SerializeField] private float friction=4f, airFriction=2f;

    [Header("Movement Properties")]
    [SerializeField] private float maxSpeed=6;
    [SerializeField] private float sprintSpeedScalar=1.5f;
    [SerializeField] private float groundAccelerationScalar=6f, airAccelerationScalar=2f;
    [SerializeField, Range(0, 90)] private float maxSlope=45f;
    [SerializeField, Range(0, 1)] private float slopeJumpBias=0.5f;
    private float slopeDotProduct;
    [SerializeField, Range(0, 10)] private float jumpHeight=2f;
    [SerializeField] private int maxAirJumps;
    [SerializeField] LayerMask groundMask;
    
    private Rigidbody rb;
    [field: SerializeField] public Vector3 ContactNormal { get; private set; }
    public bool IsGrounded => ContactNormal != Vector3.zero;
    private Vector3 targetVelocity;
    private Vector3 velocity => rb.velocity;
    float groundDist;
    private void Awake()
    {
        head = GetComponent<SphereCollider>();
        rb = GetComponent<Rigidbody>();
        OnValidate();
    }

    private void OnValidate()
    {
        if (head != null) {
            springRadius = head.radius - 0.01f;
            headHeight = height - head.radius - springRadius;
        }
    }

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
    }

    private void FixedUpdate()
    {
        UpdateTargetsFromInput();
        HandleGrounded();
        ApplyGravity();
        MovePlayer();
    }

    private void LateUpdate()
    {
        using (Draw.WithColor(Color.red))
        {
            Vector3 groundPoint = transform.position - Vector3.up * groundDist;
            Draw.SphereOutline(transform.position - Vector3.up * headHeight, springRadius);
            Draw.SphereOutline(groundPoint, springRadius);
            Draw.Arrow(groundPoint, groundPoint + ContactNormal);
        }
    }

    private void UpdateTargetsFromInput() {
        inputState = inputProvider.GetState();
        targetVelocity = inputState.moveDirection.magnitude == 0? Vector3.zero : inputState.moveDirection * maxSpeed;
    }

    private void HandleGrounded() {
        Vector3 springDir = transform.up;
        float checkDistance = previouslyGrounded ? headHeight + groundSnappingDistance : headHeight;
        if (Physics.SphereCast(transform.position, springRadius, -springDir, out RaycastHit hit, checkDistance, groundMask))
        {
            float offset = headHeight - hit.distance;
            float springVelocity = Vector3.Dot(springDir, velocity);
            float force = (offset * headSpringForce) - (springVelocity * headSpringDamper);
            rb.AddForce(force * springDir, ForceMode.Acceleration);
            ContactNormal = hit.normal;
            groundDist = hit.distance;
        }
        else
        {
            ContactNormal = Vector3.zero;
            groundDist = headHeight;
        }
        previouslyGrounded = IsGrounded;
    }

    private void ApplyGravity()
    {
        if (velocity.y < -terminalVelocity || IsGrounded) return;
        Debug.Log("Applying gravit");
        rb.AddForce(Vector3.ClampMagnitude((-terminalVelocity - velocity.y) * Vector3.up, gravity * Time.fixedDeltaTime), ForceMode.VelocityChange);
    }

    private void MovePlayer()
    {
        if (!IsGrounded && targetVelocity.magnitude < 0.01f) return;
        float maxAcceleration = maxSpeed * (IsGrounded ? groundAccelerationScalar : airAccelerationScalar) * Time.fixedDeltaTime;
        Vector3 acceleration = Vector3.ClampMagnitude(Vector3.ProjectOnPlane(targetVelocity - velocity, Vector3.up), maxAcceleration);
        rb.AddForce(Vector3.ProjectOnPlane(acceleration, ContactNormal), ForceMode.VelocityChange);
    }

}
