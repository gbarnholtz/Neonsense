using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

public class CameraController : SerializedMonoBehaviour, IInputModifier
{
    [Header("Input")]
    [OdinSerialize] private IPlayerInputProvider inputProvider;
    private PlayerInputState playerInputState;

    [SerializeField] private Transform target;
    [Header("Aim Parameters")]
    [SerializeField] private Vector2 cameraAngleLimit;
    [SerializeField] private LayerMask aimMask;
    private Camera activeCamera;

    [SerializeField] private Vector2 viewportAimPivot = new Vector2(0.5f,0.5f);
    private Vector3 aimPoint;


    private void Awake()
    {
        activeCamera = GetComponentInChildren<Camera>();    
    }

    void LateUpdate()
    {
        playerInputState = inputProvider.GetState();
        UpdatePosition();
        UpdateRotation();
        aimPoint = GetAimPoint();
    }

    private void UpdatePosition() {
        transform.position = target.position;
    }

    private void UpdateRotation() {
        Vector3 clampedLookEulers = transform.eulerAngles + new Vector3(-playerInputState.lookInput.y, playerInputState.lookInput.x, 0);
        clampedLookEulers.x = Mathf.Clamp(Mathf.DeltaAngle(0, clampedLookEulers.x), cameraAngleLimit.x,  cameraAngleLimit.y);

        transform.eulerAngles = clampedLookEulers;
    }

    private Vector3 GetAimPoint() {
        Ray cameraRay = activeCamera.ViewportPointToRay(viewportAimPivot);
        if (Physics.Raycast(cameraRay.origin, cameraRay.direction, out RaycastHit hit, float.MaxValue, aimMask)) return hit.point;
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
