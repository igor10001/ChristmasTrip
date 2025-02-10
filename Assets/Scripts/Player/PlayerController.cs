using UnityEngine;
using System;


    [RequireComponent(typeof(CharacterController))]
    public class PlayerController: MonoBehaviour
    {

        public event EventHandler OnPickedSomething;
        public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
        public class OnSelectedCounterChangedEventArgs : EventArgs
        {
            public BaseObject selectedObject;
        }
        [SerializeField] private LayerMask objectsLayerMask;
        private CharacterController controller;
        private Vector3 playerVelocity;
        private bool groundedPlayer;
        [SerializeField]
        private float playerSpeed = 2.0f;
        
        private float gravityValue = -9.81f;
        private PlayerInputManager _playerInputManager;

        private Transform cameraTransform;
        private Vector3 lastInteractDir;
        
        private BaseObject selectedObject;

        private void Start()
        {
            controller = GetComponent<CharacterController>();
            _playerInputManager = GetComponent<PlayerInputManager>();
            cameraTransform = Camera.main.transform;    

            

        }
        
        
        

        void Update()
        {
            HandleMovement();
            HandleInteractions();
        }

        private void HandleInteractions()
        { 
           
            float interactDistance = 5f;
            
            if(Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit raycastHit, interactDistance, objectsLayerMask))
            {
                if(raycastHit.transform.TryGetComponent(out BaseObject baseObject))
                {
                    if(baseObject != selectedObject)
                    {
                        SetSelectedCounter(baseObject);
                        Debug.Log("Selected");
                    }
                }
                else
                {
                    Debug.Log("Deselected");
                    DeselectObject();
                }
            }
            else
            {
                Debug.Log("Deselected");

                DeselectObject();

            }

        }
        private void DeselectObject()
        {
            if (selectedObject != null)
            {
                SetSelectedCounter(null);
            }
        }
        private void HandleMovement()
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
        
        private void SetSelectedCounter(BaseObject selectedObject)
        {
            this.selectedObject = selectedObject;

            OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs{
                selectedObject = selectedObject
            });
        }
    }
