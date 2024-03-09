using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(SphereCollider), typeof(Rigidbody))]
public class PlayerMotor : SerializedMonoBehaviour
{
    [OdinSerialize] private IInputProvider inputProvider;
    private InputState inputState;
    public event Action<bool> OnGroundedChanged;
    [Header("Player Spring Properties")]
    [SerializeField] private float height;
    private float headHeight;
    private SphereCollider head;
    [SerializeField] private float headSpringForce, headSpringDamper;
    private float headSpringVel, springRadius;

    [Header("Physical Properties")]
    [SerializeField] private float gravity = 15f, terminalVelocity;
    [SerializeField, Range(0, 1)] private float hardLandingThreshold = 0.9f;
    [SerializeField] private float friction, airFriction;
    private Collider[] collisionBuffer = new Collider[10];
    [SerializeField] private LayerMask groundMask;
    private Rigidbody rb;
    public Vector3 ContactNormal { get; private set; }
    public bool IsGrounded => ContactNormal == Vector3.zero;

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

    void Update()
    {
        inputState = inputProvider.GetState();
    }


    private void FixedUpdate()
    {
        HandleGrounded();
    }
    private void HandleGrounded() {
        Vector3 springDir = transform.up;
        if (Physics.SphereCast(transform.position, springRadius, -springDir, out RaycastHit hit, headHeight, groundMask))
        {
            float offset = headHeight - hit.distance;
            float springVelocity = Vector3.Dot(springDir, rb.velocity);
            float force = (offset * headSpringForce) - (springVelocity * headSpringDamper);
            rb.AddForce(force * springDir, ForceMode.Acceleration);
            ContactNormal = hit.normal;
        }
        else
        {
            ContactNormal = Vector3.zero;
        }
    }

}
