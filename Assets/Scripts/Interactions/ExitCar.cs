using System;
using System.Collections;
using DefaultNamespace;
using UnityEngine;
using Ezereal;
using UnityEngine.InputSystem;


namespace Interactions
{
    
    
    [System.Serializable]
    public struct ExitCarTransitionRefs
    {
        public Transform exitPoint;
        public PlayerController playerController;
        public GameObject playerCamera;
        public GameObject carCamera;
        public CharacterController CharacterController;
        public EzerealCarController ezerealCarController;

    }
    
    
    public class ExitCar : MonoBehaviour
    {
        [SerializeField] private PlayerInputManager _playerInputManager;
        [SerializeField] private Rigidbody rb;
        [SerializeField] private GameObject hint;
        public float thresholdSpeed;
        private Coroutine hintCoroutine;
        private bool hasShownHint = false; 
        
        //data exit car
        //exit transform from car
        [SerializeField] private PlayerController _playerController;//reset position, rotation
        //  playerController.stopMovement = true;
        //car camera false
        // playuer camera true
        // character controler.player true
        // change car engine sound to idle

        [SerializeField] private ExitCarTransitionRefs _exitCarTransitionRefs;
        

        private void OnEnable()
        {
            _playerInputManager.OnVechicleExitAction += OnVehicleExitAction;
        }

        private void OnDisable()
        {
            _playerInputManager.OnVechicleExitAction -= OnVehicleExitAction;
        }

        private void OnDestroy()
        {
            _playerInputManager.OnVechicleExitAction -= OnVehicleExitAction;
        }

        private void Update()
        {
            if (PlayerStateManager.CurrentState == PlayerState.InVehicle)
            {
                float speed = rb.velocity.magnitude * 3.6f;

                if (speed <= thresholdSpeed)
                {
                    if (!hasShownHint) // ✅ Show hint only once
                    {
                        hasShownHint = true;
                        hintCoroutine = StartCoroutine(ShowHintForSeconds(4f));
                    }
                }
                else
                {
                    if (hintCoroutine != null)
                    {
                        StopCoroutine(hintCoroutine);
                        hint.SetActive(false);
                        hintCoroutine = null;
                    }
                    
                    hasShownHint = false; 
                }
            }
        }

        private void OnVehicleExitAction(object sender, EventArgs e)
        {
            float speed = rb.velocity.magnitude * 3.6f;

            if (speed <= thresholdSpeed)
            {
                ExitCarActions();
            }
            else
            {
                Debug.Log("Car is moving too fast to exit.");
            }
        }

        private IEnumerator ShowHintForSeconds(float duration)
        {
            hint.SetActive(true);
            yield return new WaitForSeconds(duration);
            hint.SetActive(false);
            hintCoroutine = null;
        }

        private void ExitCarActions()
        {
            PlayerStateManager.SetState(PlayerState.NotInVehicle);
            _exitCarTransitionRefs.playerController.stopMovement = false;
            _exitCarTransitionRefs.playerController.transform.position = _exitCarTransitionRefs.exitPoint.position;
            _exitCarTransitionRefs.playerController.transform.rotation = _exitCarTransitionRefs.exitPoint.rotation;
            // _vehicleInteractionRefs.camera[0].SetActive(false);
            _exitCarTransitionRefs.playerCamera.SetActive(true);
            _exitCarTransitionRefs.carCamera.SetActive(false);
            _exitCarTransitionRefs.CharacterController.enabled = true;
           //    _exitCarTransitionRefs.ezerealCarController.enabled = false;
        }
    }
}
