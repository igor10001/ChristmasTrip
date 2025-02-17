using DefaultNamespace.Sound;

namespace Interactions.Objects
{
    public class Radio : BaseObject
    {
        private bool _hasStarted = false;

        public override void InteractPerformed(PlayerController playerController)
        {
            if (!_hasStarted)
            {
                SoundManager.Instance.PlayRadio();
                _hasStarted = true;
            }
            else
            {
                if (SoundManager.Instance.isRadioPaused)
                {
                    SoundManager.Instance.ResumeRadio();
                }
                else
                {
                    SoundManager.Instance.PauseRadio();
                }
            }
        }
    }
}