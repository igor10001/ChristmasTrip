using DefaultNamespace.Sound;
using UnityEngine;

namespace Interactions.Objects
{
    public class SteeringWheel : BaseObject
    {
        [SerializeField] private AudioSource hornAudio;
        
        public override void InteractPerformed( PlayerController player)
        {
            hornAudio.Play();
        }
    }
}