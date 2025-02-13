using System.Collections;
using DefaultNamespace.Sound;
using UnityEngine;

namespace Ezereal
{
    public class EzerealSoundController : MonoBehaviour // This system plays tire and engine sounds.
    {
        [Header("References")]
        [SerializeField] bool useSounds = false;
        [SerializeField] EzerealCarController ezerealCarController;
        [SerializeField] AudioSource tireAudio;
        [SerializeField] AudioSource engineAudio;
        [SerializeField] AudioSource reverseAudio;
        private Coroutine reverseSoundCoroutine = null;

        [Header("Settings")]
        public float maxVolume = 0.5f; // Maximum volume for high speeds

        [Header("Debug")]
        [SerializeField] bool alreadyPlaying;
        private bool reverseSoundPlayed = false;
        void Start()
        {
            if (useSounds)
            {
                alreadyPlaying = false;

                if (ezerealCarController == null || ezerealCarController.vehicleRB == null || tireAudio == null || engineAudio == null)
                {
                    Debug.LogWarning("ezerealSoundController is missing some references. Ignore or attach them if you want to have sound controls.");


                }

                if (tireAudio != null)
                {
                    tireAudio.volume = 0f; // Start with zero volume
                    tireAudio.Stop();
                }
            }
        }

        public void TurnOnEngineSound()
        {
            useSounds = true;
            if (useSounds)
            {
                if (engineAudio != null)
                {
                    SoundManager.Instance.PlayEngineStartSound(transform.position, () =>
                    {
                        engineAudio.Play();
                    } );
                   
                }
            }
        }



        public void ReverseSoundOn()
        {
            if (!reverseSoundPlayed) // Play sound only once
            {
                reverseAudio.Play();
                reverseSoundPlayed = true; // Set flag to true to prevent further plays
            }
        }

        public void ReverseSoundOff()
        {
            // Stop reverse sound only once when reversing stops
            if (reverseAudio.isPlaying)
            {
                reverseAudio.Stop();
                reverseSoundPlayed = false; // Reset flag when reversing stops
            }
        }

        private void ReverseSound()
        {
            if (ezerealCarController.currentGear == AutomaticGears.Reverse && ezerealCarController.currentBrakeValue == 1)
            {
                StartReverseSoundDelay(); // Start delay for reverse sound
            }
            else
            {
                // Stop the sound only if it's already playing
                if (reverseAudio.isPlaying)
                {
                    ReverseSoundOff();
                }
            }
        }

        private IEnumerator ReverseSoundDelay()
        {
            yield return new WaitForSeconds(0.05f); // Wait for a short time

            if (ezerealCarController.currentGear == AutomaticGears.Reverse && ezerealCarController.currentBrakeValue == 1)
            {
                if (!reverseAudio.isPlaying) // Check if it's not already playing
                {
                    ReverseSoundOn(); // Play reverse sound
                }
            }
        }
        public void TurnOffEngineSound()
        {
            useSounds = false;
            
                if (engineAudio != null)
                {
                    SoundManager.Instance.PlayHandBrakeSound(transform.position, () =>
                    {
                        engineAudio.Stop();

                    });
                }
            
        }
        private void StartReverseSoundDelay()
        {
            if (reverseSoundCoroutine != null) // If coroutine is already running, stop it
            {
                StopCoroutine(reverseSoundCoroutine);
            }
            reverseSoundCoroutine = StartCoroutine(ReverseSoundDelay());
        }

      


        void Update()
        {
            ReverseSound();
            
            if (useSounds)
            {
#if UNITY_6000_0_OR_NEWER
                if (ezerealCarController != null && ezerealCarController.vehicleRB != null && tireAudio != null && engineAudio != null)
                {
                    if (!ezerealCarController.stationary && !alreadyPlaying && !ezerealCarController.InAir())
                    {
                        tireAudio.Play();
                        alreadyPlaying = true;
                    }
                    else if (ezerealCarController.stationary || ezerealCarController.InAir())
                    {
                        tireAudio.Stop();
                        alreadyPlaying = false;
                    }

                    // Get the car's current speed
                    float speed = ezerealCarController.vehicleRB.linearVelocity.magnitude;

                    // Calculate the volume based on speed
                    float targetVolume = Mathf.Clamp01(speed / 15) * maxVolume;


                    tireAudio.volume = targetVolume;

                    //Tire Pitch

                    float tireSoundPitch = 0.8f + (Mathf.Abs(ezerealCarController.vehicleRB.linearVelocity.magnitude) / 50f);
                    tireAudio.pitch = tireSoundPitch;

                    //Engine Pitch

                    float engineSoundPitch = 0.8f + (Mathf.Abs(ezerealCarController.vehicleRB.linearVelocity.magnitude) / 25f);
                    engineAudio.pitch = engineSoundPitch;
#else
            if (ezerealCarController != null && ezerealCarController.vehicleRB != null && tireAudio != null && engineAudio != null)
            {
                if (!ezerealCarController.stationary && !alreadyPlaying && !ezerealCarController.InAir())
                {
                    tireAudio.Play();
                    alreadyPlaying = true;
                }
                else if (ezerealCarController.stationary || ezerealCarController.InAir())
                {
                    tireAudio.Stop();
                    alreadyPlaying = false;
                }

                // Get the car's current speed
                float speed = ezerealCarController.vehicleRB.velocity.magnitude;

                // Calculate the volume based on speed
                float targetVolume = Mathf.Clamp01(speed / 15) * maxVolume;


                tireAudio.volume = targetVolume;

                //Tire Pitch

                float tireSoundPitch = 0.8f + (Mathf.Abs(ezerealCarController.vehicleRB.velocity.magnitude) / 50f);
                tireAudio.pitch = tireSoundPitch;

                //Engine Pitch

                float engineSoundPitch = 0.1f + (Mathf.Abs(ezerealCarController.vehicleRB.velocity.magnitude) / 40f);
                engineAudio.pitch = engineSoundPitch;
#endif
                }
            }
        }
    }
}
