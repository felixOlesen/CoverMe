using UnityEngine;

public class WallController : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.CompareTag("Projectile")) {
            // Handle projectile collision with wall (e.g., destroy projectile, play sound, etc.)
            LevelManager.Instance.HandleProjectileHit(collision.gameObject);
        }
    }
}
