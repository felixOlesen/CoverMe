using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Steps to implement:
    // 1. Subscribe to InputBroadcaster events in OnEnable and unsubscribe in OnDisable
    // 2. Implement event handlers for each input event (e.g., HandleMove, HandleLook, etc.)
    // 3. In each handler, update the player's state or trigger animations based on the input received
    // 4. Ensure that the player's movement and actions are responsive and smooth by using Time.deltaTime for movement calculations
    // 5. Test the implementation in the Unity Editor to ensure that the player responds correctly to all input events and that there are no

    // Implement Shooting Mechanism
    // Implement Movement Mechanics
    // Add a collider
    // Implement Health System
    [SerializeField] private Camera mainCamera;
    private bool lookingForMouse;
    private Vector2 mousePosition;

    private Vector2 moveInput;
    [SerializeField]
    private float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Rigidbody2D container_rb;

    [SerializeField]
    private GameObject playerContainer;

    [SerializeField]
    private float health = 100f;
    [SerializeField]
    private float maxHealth = 100f;
    [SerializeField]
    private GameObject healthBar; 


    void Awake() {
        

        if (mainCamera == null) mainCamera = Camera.main;
        lookingForMouse = true;
        rb = GetComponent<Rigidbody2D>();
        container_rb = playerContainer.GetComponent<Rigidbody2D>();

    }

    void Start() {
        LevelManager.Instance.playerObject = this.gameObject;
        LevelManager.Instance.playerController = this;
    }

    void Update() {
        RotateTowardsMouse();
    }

    void FixedUpdate() {
        // Handle physics-based movement here if needed
        Vector2 targetVelocity = moveInput * moveSpeed;
        container_rb.linearVelocity = targetVelocity;

    }

    void OnEnable() {
        // Subscribe to input events
        InputBroadcaster.OnMoveEvent += HandleMove;
        InputBroadcaster.OnLookEvent += HandleLook;
        InputBroadcaster.OnAttackEvent += HandleAttack;
        InputBroadcaster.OnInteractEvent += HandleInteract;
    }

    void OnDisable() {
        // Unsubscribe from input events
        InputBroadcaster.OnMoveEvent -= HandleMove;
        InputBroadcaster.OnLookEvent -= HandleLook;
        InputBroadcaster.OnAttackEvent -= HandleAttack;
        InputBroadcaster.OnInteractEvent -= HandleInteract;
    }


    void HandleMove(InputValue inputValue) {
        moveInput = inputValue.Get<Vector2>();
    }

    void HandleLook(InputValue inputValue) {
        mousePosition = inputValue.Get<Vector2>();
    }

    void HandleAttack(InputValue inputValue) {
        // Implement attack logic here
        Debug.Log($"Handling attack input");
    }

    void HandleInteract(InputValue inputValue) {
        // Implement interact logic here
        Debug.Log($"Handling interact input");
    }


    void RotateTowardsMouse() {
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, mainCamera.transform.position.y - transform.position.y));
        mouseWorldPosition.z = transform.position.z;
        Vector3 direction = mouseWorldPosition - transform.position;
        transform.up = direction;

    }

}
