using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Entity
{

    [SerializeField] private Camera mainCamera;
    private bool lookingForMouse;
    private Vector2 mousePosition;

    private Vector2 moveInput;

    void Awake() {
        if (mainCamera == null) mainCamera = Camera.main;
        lookingForMouse = true;
        EntityRB = gameObject.GetComponent<Rigidbody2D>();
    }

    void Start() {
        LevelManager.Instance.playerObject = this.gameObject;
        LevelManager.Instance.playerController = this;
    }

    void Update() {
    }

    void FixedUpdate() {
        // Handle physics-based movement here if needed
        RotateTowardsMouse();
        Vector2 targetVelocity = moveInput * moveSpeed;
        EntityRB.linearVelocity = targetVelocity;

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
        // Use rigidbody physics to rotate towards the mouse position
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, mainCamera.transform.position.y - transform.position.y));
        Vector2 direction = (Vector2)mouseWorldPosition - EntityRB.position;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        targetAngle -= 90f; // Adjust if your sprite faces up instead of right
        EntityRB.MoveRotation(targetAngle);
    }

}
