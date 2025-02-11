using UnityEngine;
using Cinemachine;

using DefaultNamespace;

public class CinemachinePovExtension : CinemachineExtension
{

    [SerializeField] private Transform rotationReference; 
    [SerializeField] private float clampAngleY = 80f;
    [SerializeField] private float clampAngleX = 90f;
    [SerializeField] private bool xClamp;
    [SerializeField] private float horizontalSpeed = 10f;
    [SerializeField] private float verticalSpeed = 10f;
    [SerializeField] private PlayerInputManager _playerInputManager;

    private Vector2 rotationInput;
    private Quaternion relativeRotation; 
    private bool hasInitializedInVehicle = false;

    private void Start()
    {
        PlayerStateManager.OnEnterVehicle += PlayerStateManagerOnEnterVehicle;
        PlayerStateManager.OnExitVehicle += PlayerStateManagerOnExitVehicle;
        if (rotationReference != null)
            relativeRotation = Quaternion.Inverse(rotationReference.rotation) * transform.rotation;
        
    }

    private void PlayerStateManagerOnExitVehicle()
    {
        
    }

    private void PlayerStateManagerOnEnterVehicle()
    {
        
    }

    private void AlignCameraOnVehicleEnter()
    {
        relativeRotation = Quaternion.Inverse(rotationReference.rotation) * transform.rotation;
    }

    private void AlignCameraVehicleRotation()
    {
        if (rotationReference == null) return;
        Quaternion carRotation = rotationReference.rotation;

        Quaternion targetRotation = carRotation * relativeRotation;
        transform.rotation = targetRotation;
    }

    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (vcam.Follow && stage == CinemachineCore.Stage.Aim)
        {
            PlayerState currentState = PlayerStateManager.CurrentState;
            switch (currentState)
            {
                case PlayerState.InVehicle:
                    Debug.Log("trarara");
                    if (!hasInitializedInVehicle)
                    {
                        AlignCameraOnVehicleEnter();
                        hasInitializedInVehicle = true;
                    }

                    AlignCameraVehicleRotation(); 

                    Vector2 deltaInput = _playerInputManager.GetMouseDelta();
                    rotationInput.x += deltaInput.x * verticalSpeed * deltaTime;
                    rotationInput.y += deltaInput.y * horizontalSpeed * deltaTime;

                    rotationInput.y = Mathf.Clamp(rotationInput.y, -clampAngleY, clampAngleY);
                    if (xClamp)
                        rotationInput.x = Mathf.Clamp(rotationInput.x, -clampAngleX, clampAngleX);

                    Quaternion manualRotation = Quaternion.Euler(-rotationInput.y, rotationInput.x, 0);
                    state.RawOrientation = transform.rotation * manualRotation;
                    break;

                case PlayerState.NotInVehicle:
                    Debug.Log("fafa");
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
