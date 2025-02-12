using UnityEngine;
using System;
using DefaultNamespace;


[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public event EventHandler OnPickedSomething;
    public event EventHandler<OnSelectedBaseObjChangedEventArgs> OnSelectedBaseObjChanged;
    
    public class OnSelectedBaseObjChangedEventArgs : EventArgs
    {
        public BaseObject selectedObject;
    }

    [SerializeField] private LayerMask objectsLayerMask;
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    [SerializeField]float interactDistance = 5f;
    [SerializeField] private float playerSpeed = 2.0f;

    private float gravityValue = -9.81f;
    private PlayerInputManager _playerInputManager;

    private Transform cameraTransform;
    private Vector3 lastInteractDir;

    private BaseObject selectedObject;
    public bool stopMovement;

    private void Start()
    {
        PlayerStateManager.SetState(PlayerState.NotInVehicle);
        controller = GetComponent<CharacterController>();
        _playerInputManager = GetComponent<PlayerInputManager>();
        cameraTransform = Camera.main.transform;
        _playerInputManager.OnInteractionAction += PlayerInputManagerOnInteractionAction;

    }


    private void OnEnable()
    {

    }
    private void OnDisable()
    {
        _playerInputManager.OnInteractionAction -= PlayerInputManagerOnInteractionAction;

    }
    private void OnDestroy()
    {
        _playerInputManager.OnInteractionAction -= PlayerInputManagerOnInteractionAction;

    }

    private void PlayerInputManagerOnInteractionAction(object sender, EventArgs e)
    {
        if (selectedObject != null)
        {
            selectedObject.Interact(this);
        }
    }


    void Update()
    {
        if (!stopMovement)
        {
            HandleMovement();
        }

        HandleInteractions();
    }

    private void HandleInteractions()
    {
     


        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit interactionHit,
                interactDistance))
        {
            Debug.Log("Interaction Raycast hit: " + interactionHit.transform.name);
            if (IsObjectOnLayer(objectsLayerMask, interactionHit.collider.gameObject))
            {
                if (interactionHit.collider.TryGetComponent(out BaseObject baseObject))
                {
                    if (baseObject != selectedObject)
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
                Debug.Log("Hit object is not on the correct layer, deselecting.");
                DeselectObject(); // If object is not on the layer, deselect it
            }
        }
        else
        {
            Debug.Log("Hit object is not on the correct layer, deselecting.");
            DeselectObject(); // If object is not on the layer, deselect it
        }
        
    }

    private void DeselectObject()
    {
        if (selectedObject != null)
        {
            SetSelectedCounter(null);
        }
    }

    bool IsObjectOnLayer(LayerMask layerMask, GameObject obj)
    {
        return (layerMask.value & (1 << obj.layer)) != 0;
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

        OnSelectedBaseObjChanged?.Invoke(this, new OnSelectedBaseObjChangedEventArgs
        {
            selectedObject = selectedObject
        });
    }
}