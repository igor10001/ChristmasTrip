using UnityEngine;
using UnityEngine.Windows.WebCam;

namespace DefaultNamespace
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController: MonoBehaviour
    {
        
        private CharacterController controller;
        private Vector3 playerVelocity;
        private bool groundedPlayer;
        [SerializeField]
        private float playerSpeed = 2.0f;
        
        private float gravityValue = -9.81f;
        private PlayerInputManager _playerInputManager;

        private Transform cameraTransform;

        private void Start()
        {
            controller = GetComponent<CharacterController>();
            _playerInputManager = GetComponent<PlayerInputManager>();
            cameraTransform = Camera.main.transform;    

            

        }

        void Update()
        {
            groundedPlayer = controller.isGrounded;
            if (groundedPlayer && playerVelocity.y < 0)
            {
                playerVelocity.y = 0f;
            }


            Vector2 movement = _playerInputManager.GetPlayerMovement();
            Vector3 move = new Vector3(movement.x, 0, movement.y);
            move = cameraTransform.forward * move.z + cameraTransform.right * move.x;

            Vector3 movementDelta = move * playerSpeed;
            movementDelta *= Time.deltaTime;
            move.y = 0f;
            controller.Move(movementDelta);

            if (move != Vector3.zero)
            {
                gameObject.transform.forward = move;
            }



            playerVelocity.y += gravityValue * Time.deltaTime;
            controller.Move(playerVelocity * Time.deltaTime);
        }
    }
}