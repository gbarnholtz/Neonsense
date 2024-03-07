using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

public class CameraController : SerializedMonoBehaviour, IInputModifier
{
    [Header("Input")]
    [OdinSerialize] private IPlayerInputProvider inputProvider;
    private PlayerInputState playerInputState;

    [SerializeField] private Rigidbody target;

    [Header("Velocity Offset Parameters")]
    [SerializeField] private Vector3 maxVelocityOffset;
    private Vector3 currentVelocityOffset, velocityOffset;
    [SerializeField] private float velocityRoof = 1, velocityOffsetLerp;
    [Header("Aim Parameters")]
    [SerializeField] private Vector2 cameraAngleLimit;
    [SerializeField] private float minimumAimDistance, additionalOffset;
    [SerializeField] private LayerMask aimMask;
    private Camera activeCamera;

    [SerializeField] private Vector2 viewportAimPivot = new Vector2(0.5f,0.5f);
    private Vector3 aimPoint;

    [Header("Camera Shoulder Parameters")]
    [SerializeField] private Transform shoulder;
    [SerializeField] private float armReturnSpeed = 1f;
    [SerializeField] private LayerMask wallMask;
    private Vector3 armDistance;

    private void Awake()
    {
        armDistance = shoulder.localPosition;
        activeCamera = Camera.main;
    }

    void LateUpdate()
    {
        playerInputState = inputProvider.GetState();
        UpdatePosition();
        UpdateRotation();

        Vector3 localShoulderPosition = shoulder.localPosition;
        
        if (Physics.Raycast(transform.position + transform.right * localShoulderPosition.x + transform.up * armDistance.y, -shoulder.forward, out RaycastHit hit, Mathf.Abs(armDistance.z) + additionalOffset, wallMask))
        {
            localShoulderPosition.z = -hit.distance + additionalOffset;
        }
        else {
            localShoulderPosition.z = Mathf.Lerp(localShoulderPosition.z, armDistance.z, armReturnSpeed*Time.deltaTime);
        }
        shoulder.localPosition = localShoulderPosition;
        aimPoint = GetAimPoint();
    }

    private void UpdatePosition() {
        Vector3 scaledVelocity = target.velocity / velocityRoof;
        Vector3 localOffset = new Vector3(Vector3.Dot(scaledVelocity, transform.right), Vector3.Dot(scaledVelocity, transform.up), Vector3.Dot(scaledVelocity, transform.forward));
        localOffset = Vector3.Scale(localOffset, maxVelocityOffset);
        velocityOffset = localOffset.x * transform.right + localOffset.y * transform.up + localOffset.z * transform.forward;
        currentVelocityOffset = Vector3.Lerp(currentVelocityOffset, velocityOffset, 1 - Mathf.Exp(-velocityOffsetLerp * Time.deltaTime));
        transform.position = target.position + currentVelocityOffset;
    }

    private void UpdateRotation() {
        Vector3 clampedLookEulers = transform.eulerAngles + new Vector3(-playerInputState.lookInput.y, playerInputState.lookInput.x, 0);
        clampedLookEulers.x = Mathf.Clamp(Mathf.DeltaAngle(0, clampedLookEulers.x), cameraAngleLimit.x,  cameraAngleLimit.y);

        transform.eulerAngles = clampedLookEulers;
    }

    private Vector3 GetAimPoint() {
        Ray cameraRay = activeCamera.ViewportPointToRay(viewportAimPivot);
        if (Physics.Raycast(cameraRay.origin, cameraRay.direction, out RaycastHit hit, float.MaxValue, aimMask)) {
            return (hit.distance < minimumAimDistance) ? cameraRay.origin + cameraRay.direction * minimumAimDistance : hit.point; 
        }
        return cameraRay.origin + cameraRay.direction * 100;
    }

    public InputState ModifyInput(InputState input)
    {
        input.aimPoint = aimPoint;
        input.lookEulers = input.lookEulers + transform.rotation.eulerAngles;
        input.moveDirection = Quaternion.Euler(0,transform.rotation.eulerAngles.y,0) * input.moveDirection;
        return input;
    }
}
