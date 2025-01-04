using System;
using UnityEngine;
using Cinemachine;
using UnityEditor;
using UnityEngine.InputSystem;

namespace DefaultNamespace.Cinemachine
{ 
    public enum VehicleState
    {
        InVehicle,
        NotInVehicle
    }
    
    public class CinemachinePovExtension : CinemachineExtension
    {
        [SerializeField]
        private VehicleState currentState ;
        [SerializeField]
        private float clampAngleY = 80f;  [SerializeField]
        private float clampAngleX = 90f;
        [SerializeField]
        private bool xClamp;
        

        
        

        [SerializeField] private float horizontalSpeed = 10f;
        [SerializeField] private float verticalSpeed = 10f;
        [SerializeField] private PlayerInputManager _playerInputManager;
        private Vector3 startingRotation;
        [SerializeField] private Transform rotationReference;
        private bool hasInitializedInVehicle = false;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Y))
            {
                startingRotation.x += 10;
            }
            if (Input.GetKeyDown(KeyCode.H))
            {
                startingRotation.x -= 10;
            }
            
        }

        private void AlignCamera()
        {
            startingRotation.x = rotationReference.eulerAngles.y;
        }

        protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
        {
            if (vcam.Follow)
            {
                if (stage == CinemachineCore.Stage.Aim)
                {
                    switch (currentState)
                    {
                        case VehicleState.InVehicle:
                           
                            if (!hasInitializedInVehicle)
                            {
                                AlignCamera();
                                hasInitializedInVehicle = true;
                            }

                            Vector2 deltaInputt = _playerInputManager.GetMouseDelta();
                            
                            startingRotation.x += deltaInputt.x *   verticalSpeed * Time.deltaTime;
                            startingRotation.y += deltaInputt.y * horizontalSpeed * Time.deltaTime;
                            startingRotation.y = Mathf.Clamp(startingRotation.y, -clampAngleY, clampAngleY);
                            if(xClamp) startingRotation.x = Mathf.Clamp(startingRotation.x, -clampAngleX + rotationReference.eulerAngles.y, clampAngleX + rotationReference.eulerAngles.y);
                            state.RawOrientation = Quaternion.Euler(-startingRotation.y, startingRotation.x, 0f);
                            break;
                        case VehicleState.NotInVehicle:
                            if (startingRotation == null) startingRotation = transform.localRotation.eulerAngles;
                            Vector2 deltaInput = _playerInputManager.GetMouseDelta();
                            startingRotation.x += deltaInput.x *   verticalSpeed * Time.deltaTime;
                            startingRotation.y += deltaInput.y * horizontalSpeed * Time.deltaTime;
                            startingRotation.y = Mathf.Clamp(startingRotation.y, -clampAngleY, clampAngleY);
                            if(xClamp) startingRotation.x = Mathf.Clamp(startingRotation.x, -clampAngleX, clampAngleX);
                            state.RawOrientation = Quaternion.Euler(-startingRotation.y, startingRotation.x, 0f);
                            break;
                    }
                   

                    


                }
            }
        }
    }
}