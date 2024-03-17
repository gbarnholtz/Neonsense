using UnityEngine;

public class WalkMoveState : MoveState
{
    public override bool ShouldApplyGravity => !psm.IsGrounded;
    [SerializeField] private float groundAccelerationScalar = 8f, airAccelerationScalar=4f;

    public override void Update()
    {   
       if (psm.TryingToStartSlide) psm.ChangeState(psm.SlideState);
    }

    public override void MovePlayer()
    {
        float accScalar = psm.IsGrounded? groundAccelerationScalar : airAccelerationScalar;
        float maxAcceleration = psm.BaseSpeed * accScalar * Time.fixedDeltaTime;
        Vector3 acceleration = Vector3.ClampMagnitude(Vector3.ProjectOnPlane(psm.TargetVelocity - rb.velocity, Vector3.up), maxAcceleration);
        rb.AddForce(Vector3.ProjectOnPlane(acceleration, psm.ContactNormal), ForceMode.VelocityChange);
    }
}