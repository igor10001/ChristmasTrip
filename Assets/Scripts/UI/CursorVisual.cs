using UnityEngine;

namespace UI
{
    public class CursorVisual : MonoBehaviour
    {
        [SerializeField] private PlayerController player;
        [SerializeField] private GameObject cursorSelectionImage;

        private void Start()
        {
            if (player != null) 
            {
                player.OnSelectedBaseObjChanged += PlayerOnSelectedBaseObjChanged;
            }
        }

        private void OnDestroy()
        {
            if (player != null)
            {
                player.OnSelectedBaseObjChanged -= PlayerOnSelectedBaseObjChanged;
            }
        }

        private void PlayerOnSelectedBaseObjChanged(object sender, PlayerController.OnSelectedBaseObjChangedEventArgs e)
        {
            cursorSelectionImage.SetActive(e.selectedObject != null);
        }
    }
}