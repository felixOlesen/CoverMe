using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    private Vector2 mousePosition;

    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed=10f;

    private Queue<GameObject> projectilePool = new Queue<GameObject>();
    private int poolSize = 15;
    
    void Awake() {
        if (mainCamera == null) mainCamera = Camera.main;
    }

    void Start() {
        LevelManager.Instance.weaponController = this;
        // Pre-instantiate projectiles for pooling
        for(int i = 0; i < poolSize; i++) {
            GameObject projectile = Instantiate(projectilePrefab);
            projectile.SetActive(false);
            projectilePool.Enqueue(projectile);
        }
    }

    void OnEnable() {
        // Subscribe to input events
        InputBroadcaster.OnLookEvent += HandleLook;
        InputBroadcaster.OnAttackEvent += HandleAttack;
    }

    void OnDisable() {
        // Unsubscribe from input events
        InputBroadcaster.OnLookEvent -= HandleLook;
        InputBroadcaster.OnAttackEvent -= HandleAttack;
    }

    void HandleLook(InputValue inputValue) {
        mousePosition = inputValue.Get<Vector2>();
    }

    void HandleAttack(InputValue inputValue) {
        Vector2 attackDirection = GetDirection();
        Vector3 spawnPosition = transform.position + (Vector3)(attackDirection.normalized * 1f);
        if(projectilePool.Count > 0) {
            GameObject projectile = projectilePool.Dequeue();
            projectile.transform.position = spawnPosition;
            projectile.SetActive(true);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            rb.linearVelocity = attackDirection.normalized * projectileSpeed;
        } else {
            GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            rb.linearVelocity = attackDirection.normalized * projectileSpeed;
        }
    }

    Vector3 GetDirection() {
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, mainCamera.transform.position.y - transform.position.y));
        mouseWorldPosition.z = transform.position.z;
        Vector3 direction = mouseWorldPosition - transform.position;
        return direction;
    }

    public void HandleProjectileHit(GameObject projectile) {
        // Implement logic for when a projectile hits a wall (e.g., play sound, spawn particles, etc.)
        Debug.Log($"Projectile hit at position: {projectile.transform.position}, Queue Size: {projectilePool.Count}");
        projectile.SetActive(false); // Deactivate projectile instead of destroying it for pooling
        if(projectilePool.Count < poolSize) {
            projectilePool.Enqueue(projectile); // Return projectile to pool
        } else {
            Destroy(projectile); // Destroy if pool is full to prevent memory issues
        }
    }

}
