using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }
    [SerializeField] private float levelWidth = 10f;
    [SerializeField] private float levelHeight = 6f;
    public GameObject playerObject;
    public PlayerController playerController;
    public WeaponController weaponController;

    void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        } else {
            Instance = this;
        }   
    }

    public void HandleProjectileHit(GameObject projectile) {
        // Implement logic for when a projectile hits a wall (e.g., play sound, spawn particles, etc.)
        Debug.Log($"Projectile hit at position: {projectile.transform.position}");

        if(weaponController != null) {
            weaponController.HandleProjectileHit(projectile);
        } else {
            Debug.LogWarning("WeaponController reference is null in LevelManager.");
            Destroy(projectile); // Fallback to destroying the projectile if WeaponController is not set
        }

    }

}
