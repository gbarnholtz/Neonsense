using Sirenix.Serialization;
using System;
using UnityEngine;

[RequireComponent(typeof(SphereCollider), typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour
{
    [OdinSerialize] private IInputProvider inputProvider;
    private InputState inputState;
    public event Action<bool> OnGroundedChanged;

    [SerializeField] public float Height { 
        get { return height; } 
        set { 
            height = value;
            headHeight = value - head.radius;
        } 
    }

    private float height, headHeight;
    private SphereCollider head;
    [SerializeField] private float headSpringForce, headSpringDamper;
    private float headSpringVel, springRadius;
    [SerializeField] private float gravity, terminalVelocity;
    [SerializeField, Range(0, 1)] private float hardLandingThreshold = 0.9f;
    [SerializeField] private float friction, airFriction;

    private void Awake()
    {
        head = GetComponent<SphereCollider>();
        OnValidate();
    }

    private void OnValidate()
    {
        if (head != null) {
            springRadius = head.radius - 0.01f;
            headHeight = height - head.radius;
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
        if (Physics.SphereCast(transform.position, springRadius, -transform.up, out RaycastHit hit, headHeight)) {
        }
    }

}
