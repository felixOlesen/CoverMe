using UnityEngine;

public class AttackingState : HeroState
{
    private GameObject hero;
    private Transform heroTransform;
    public GameObject targetEnemy;

    public AttackingState(GameObject hero)
    {
        this.hero = hero;
        this.heroTransform = hero.transform;
    }

    public void EnterState(HeroController hero)
    {
        // Initialize attacking behavior (e.g., set target enemy, start attack animation, etc.)
    }

    public void UpdateState(HeroController hero)
    {
        // Implement attacking logic (e.g., move towards target enemy, check for attack range, etc.)
        
    }

    public void FixedUpdateState(HeroController hero)
    {
        RotateTowardsTarget(hero);
        MoveTowardsTarget(hero);
    }

    public void ExitState(HeroController hero)
    {
        // Clean up attacking behavior (e.g., stop attack animation, reset variables, etc.)
    }

    public void HandleCollision(HeroController hero, Collision2D collision) {
        if(collision.gameObject.CompareTag("Projectile")) {
            LevelManager.Instance.HandleProjectileHit(collision.gameObject);
        } 
        if(collision.gameObject.CompareTag("BadGuy")) {
            // Handle collision with enemy (e.g., take damage, play sound, etc.)
        }
    }

    public void HandleTrigger(HeroController hero, Collider2D collider) {
        // Implement logic for when the hero enters a trigger (e.g., detect player proximity, pick up items, etc.)
    }

    void RotateTowardsTarget(HeroController hero) {
        if (targetEnemy == null) return;

        Vector2 direction = targetEnemy.transform.position - heroTransform.position;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        targetAngle -= 90f; 

        // Smoothly interpolate the angle (Mathf.LerpAngle safely handles the 360-to-0 degree wrap-around)
        float smoothedAngle = Mathf.LerpAngle(hero.EntityRB.rotation, targetAngle, hero.rotationSpeed * Time.fixedDeltaTime);
        // Tell the physics engine to move the rotation
        hero.EntityRB.MoveRotation(smoothedAngle);
    }

    void MoveTowardsTarget(HeroController hero) {
        if (targetEnemy == null) return;

        if (Vector2.Distance(heroTransform.position, targetEnemy.transform.position) <= hero.attackRange) {
            // We are in attack range, so we can stop moving and attack
            hero.EntityRB.linearVelocity = Vector2.zero; // Stop moving
        } else {
            Vector2 direction = targetEnemy.transform.position - heroTransform.position;
            hero.EntityRB.linearVelocity = direction.normalized * hero.moveSpeed; // Move towards the target enemy at a constant speed
        }
    }
    
}
