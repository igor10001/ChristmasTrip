
using UnityEngine;

namespace DefaultNamespace.ScriptableObjects
{
    [CreateAssetMenu()]
    public class AudioClipRefsSO : ScriptableObject
    {
        public AudioClip reverseBeep;
        public AudioClip engineStart;
        public AudioClip[] interaction;
        public AudioClip handBrake;
        public AudioClip horn;
        public AudioClip[] radio;


    }
}