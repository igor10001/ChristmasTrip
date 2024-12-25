using System;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

namespace DefaultNamespace.Cinemachine
{
    public class CinemachinePovExtension : CinemachineExtension
    {

        [SerializeField]
        private float clampAngle = 80f;
        [SerializeField]
        private bool xClamp;

        [SerializeField] private float horizontalSpeed = 10f;
        [SerializeField] private float verticalSpeed = 10f;
        [SerializeField] private PlayerInputManager _playerInputManager;
        private Vector3 startingRotation;


      
        protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
        {
            if (vcam.Follow)
            {
                if (stage == CinemachineCore.Stage.Aim)
                {
                    if (startingRotation == null) startingRotation = transform.localRotation.eulerAngles;
                    Vector2 deltaInput = _playerInputManager.GetMouseDelta();
                    startingRotation.x += deltaInput.x *   verticalSpeed * Time.deltaTime;
                    startingRotation.y += deltaInput.y * horizontalSpeed * Time.deltaTime;
                    startingRotation.y = Mathf.Clamp(startingRotation.y, -clampAngle, clampAngle);
                    if(xClamp) startingRotation.x = Mathf.Clamp(startingRotation.x, -clampAngle, clampAngle);
                    state.RawOrientation = Quaternion.Euler(-startingRotation.y, startingRotation.x, 0f);
                    


                }
            }
        }
    }
}