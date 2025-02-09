using UnityEngine;
using Cinemachine;

public class CinemachinePovExtension : CinemachineExtension
{
    public enum VehicleState { InVehicle, NotInVehicle }

    [SerializeField] private VehicleState currentState;
    [SerializeField] private Transform rotationReference; // Car Transform
    [SerializeField] private float clampAngleY = 80f;
    [SerializeField] private float clampAngleX = 90f;
    [SerializeField] private bool xClamp;
    [SerializeField] private float horizontalSpeed = 10f;
    [SerializeField] private float verticalSpeed = 10f;
    [SerializeField] private PlayerInputManager _playerInputManager;

    private Vector2 rotationInput;
    private Quaternion relativeRotation; // Stores relative camera rotation to the car
    private bool hasInitializedInVehicle = false;

    private void Start()
    {
        if (rotationReference != null)
            relativeRotation = Quaternion.Inverse(rotationReference.rotation) * transform.rotation;
    }

    private void AlignCameraOnVehicleEnter()
    {
        relativeRotation = Quaternion.Inverse(rotationReference.rotation) * transform.rotation;
    }

    private void AlignCameraVehicleRotation()
    {
        if (rotationReference == null) return;
        Quaternion carRotation = rotationReference.rotation;

        // Apply car rotation while maintaining manual input
        Quaternion targetRotation = carRotation * relativeRotation;
        transform.rotation = targetRotation;
    }

    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (vcam.Follow && stage == CinemachineCore.Stage.Aim)
        {
            switch (currentState)
            {
                case VehicleState.InVehicle:
                    if (!hasInitializedInVehicle)
                    {
                        AlignCameraOnVehicleEnter();
                        hasInitializedInVehicle = true;
                    }

                    AlignCameraVehicleRotation(); // Sync with vehicle rotation

                    // Get player input
                    Vector2 deltaInput = _playerInputManager.GetMouseDelta();
                    rotationInput.x += deltaInput.x * verticalSpeed * deltaTime;
                    rotationInput.y += deltaInput.y * horizontalSpeed * deltaTime;

                    // Clamp vertical rotation
                    rotationInput.y = Mathf.Clamp(rotationInput.y, -clampAngleY, clampAngleY);
                    if (xClamp)
                        rotationInput.x = Mathf.Clamp(rotationInput.x, -clampAngleX, clampAngleX);

                    // Apply manual rotation on top of car rotation
                    Quaternion manualRotation = Quaternion.Euler(-rotationInput.y, rotationInput.x, 0);
                    state.RawOrientation = transform.rotation * manualRotation;
                    break;

                case VehicleState.NotInVehicle:
                    Vector2 freeLookInput = _playerInputManager.GetMouseDelta();
                    rotationInput.x += freeLookInput.x * verticalSpeed * deltaTime;
                    rotationInput.y += freeLookInput.y * horizontalSpeed * deltaTime;

                    rotationInput.y = Mathf.Clamp(rotationInput.y, -clampAngleY, clampAngleY);
                    if (xClamp)
                        rotationInput.x = Mathf.Clamp(rotationInput.x, -clampAngleX, clampAngleX);

                    state.RawOrientation = Quaternion.Euler(-rotationInput.y, rotationInput.x, 0f);
                    break;
            }
        }
    }
}
