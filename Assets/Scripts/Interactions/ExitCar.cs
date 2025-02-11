using System;
using System.Collections;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Interactions
{
    public class ExitCar : MonoBehaviour
    {
        [SerializeField] private PlayerInputManager _playerInputManager;
        [SerializeField] private Rigidbody rb;
        [SerializeField] private GameObject hint;
        
        public float thresholdSpeed;
        private Coroutine hintCoroutine;
        private bool hasShownHint = false; // ✅ Flag to ensure hint shows only once

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
                    
                    hasShownHint = false; // ✅ Reset flag when speed is too high again
                }
            }
        }

        private void OnVehicleExitAction(object sender, EventArgs e)
        {
            float speed = rb.velocity.magnitude * 3.6f;

            if (speed <= thresholdSpeed)
            {
                Debug.Log("Space to exit car pressed! Car speed is low enough.");
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
    }
}
