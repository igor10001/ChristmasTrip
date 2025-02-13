using System;
using System.Collections;
using DefaultNamespace.ScriptableObjects;
using UnityEngine;
using UnityEngine.Audio;

namespace DefaultNamespace.Sound
{
    public class SoundManager : MonoBehaviour
    {
        private const string PLAYER_PREFS_SOUND_EFFECTS_VOLUME = "SoundEffectsVolume";


        public static SoundManager Instance { get; private set; }
        [SerializeField] private AudioMixerGroup mixerGroupSfx;
        [SerializeField] private AudioMixerGroup mixerGroupMusic;


        [SerializeField] private AudioClipRefsSO audioClipRefsSO;


        public  float volume = 6f;


        private void Awake()
        {
            Instance = this;

            volume = PlayerPrefs.GetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, 1f);
        }


        private void Start()
        {
           // PlayerShooter.Instance.OnPlayerShoot += OnPLayerShoot;
           // PlayerController.Instance.OnPlayerJump += OnPlayerJump;
           // GameEvents.OnEnemyDie += OnEnemyDie;
            /*DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
            DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;
            CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;
            Player.Instance.OnPickedSomething += Player_OnPickedSomething;
            BaseCounter.OnAnyObjectPlacedHere += BaseCounter_OnAnyObjectPlacedHere;
            TrashCounter.OnAnyObjectTrashed += TrashCounter_OnAnyObjectTrashed;*/
        }

        private void OnDisable()
        {
           // PlayerController.Instance.OnPlayerJump -= OnPlayerJump;
          //  PlayerShooter.Instance.OnPlayerShoot -= OnPLayerShoot;
           // GameEvents.OnEnemyDie -= OnEnemyDie;
        }

        private void OnEnemyDie(Vector3 obj)
        {
          //  PlaySound(audioClipRefsSO.explosion, obj, mixerGroupExplosion);
        }

        private void OnPlayerJump(object sender, EventArgs e)
        {
           // PlaySound(audioClipRefsSO.playerJump, PlayerController.Instance.transform.position, mixerGroupJump);
        }


        private void OnPLayerShoot(object sender, EventArgs e)
        {
           // PlaySound(audioClipRefsSO.shoot, PlayerController.Instance.transform.position, mixerGroupShoot);
        }

        /*
        private void TrashCounter_OnAnyObjectTrashed(object sender, System.EventArgs e) {
            TrashCounter trashCounter = sender as TrashCounter;
            PlaySound(audioClipRefsSO.trash, trashCounter.transform.position);
        }

        private void BaseCounter_OnAnyObjectPlacedHere(object sender, System.EventArgs e) {
            BaseCounter baseCounter = sender as BaseCounter;
            PlaySound(audioClipRefsSO.objectDrop, baseCounter.transform.position);
        }

        private void Player_OnPickedSomething(object sender, System.EventArgs e) {
            PlaySound(audioClipRefsSO.objectPickup, Player.Instance.transform.position);
        }

        private void CuttingCounter_OnAnyCut(object sender, System.EventArgs e) {
            CuttingCounter cuttingCounter = sender as CuttingCounter;
            PlaySound(audioClipRefsSO.chop, cuttingCounter.transform.position);
        }

        private void DeliveryManager_OnRecipeFailed(object sender, System.EventArgs e) {
            DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
            PlaySound(audioClipRefsSO.deliveryFail, deliveryCounter.transform.position);
        }

        private void DeliveryManager_OnRecipeSuccess(object sender, System.EventArgs e) {
            DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
            PlaySound(audioClipRefsSO.deliverySuccess, deliveryCounter.transform.position);
        }
        */

        /*public void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volumeMultiplier = 1f)
        {
            if (audioClipArray.Length == 0) return;

            AudioClip clipToPlay = audioClipArray[Random.Range(0, audioClipArray.Length)];
            PlaySound(clipToPlay, position, volumeMultiplier);
        }*/

        public void PlayEngineStartSound(  Vector3 position, Action onComplete)
        {
            PlaySound(audioClipRefsSO.engineStart, position, mixerGroupSfx, volume, onComplete);
        }

        public void PlayHandBrakeSound(Vector3 position, Action onComplete)
        {
            PlaySound(audioClipRefsSO.handBrake, position, mixerGroupSfx, volume, onComplete);
        }
        public void PlaySound(AudioClip audioClip, Vector3 position, AudioMixerGroup mixerGroup, float volumeMultiplier = 1f, Action onComplete = null)
        {
            GameObject soundGameObject = new GameObject("Sound");
            AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
            audioSource.clip = audioClip;
            audioSource.outputAudioMixerGroup = mixerGroup;
            audioSource.volume = volumeMultiplier * volume;
            audioSource.Play();

            StartCoroutine(DestroyAfterPlay(audioSource, onComplete));
        }

        private IEnumerator DestroyAfterPlay(AudioSource audioSource, Action onComplete)
        {
            yield return new WaitForSeconds(audioSource.clip.length);
            onComplete?.Invoke(); // Trigger callback when sound finishes
            Destroy(audioSource.gameObject);
        }


        /*public void PlayCountdownSound() {
            PlaySound(audioClipRefsSO.warning, Vector3.zero);
        }

        public void PlayWarningSound(Vector3 position) {
            PlaySound(audioClipRefsSO.warning, position);
        }*/

        public void ChangeVolume()
        {
            volume += .1f;
            if (volume > 1f)
            {
                volume = 0f;
            }

            PlayerPrefs.SetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, volume);
            PlayerPrefs.Save();
        }

        public float GetVolume()
        {
            return volume;
        }
    }
}