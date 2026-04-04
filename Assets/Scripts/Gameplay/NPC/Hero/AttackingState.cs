using UnityEngine;

public class AttackingState : HeroState
{
    private GameObject hero;
    private Transform heroTransform;
    private bool enemyInRange = false;
    public float nextAtackTime = 0f;
    public Transform currentTarget;

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
        if (currentTarget == null && hero.nearbyEnemies.Count > 0)
        {
            CleanDeadEnemies(hero);
            FindNewTarget(hero);
        }

        // If we have a valid target, do your attack/rotation logic here!
        if (currentTarget != null)
        {
            Debug.DrawLine(heroTransform.position, currentTarget.position, Color.red);
        }

        if(enemyInRange) {
            // Perform attack logic
            Attack(hero);
        }

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

    public void HandleCollisionEnter(HeroController hero, Collision2D collision) {
        if(collision.gameObject.CompareTag("Projectile")) {
            LevelManager.Instance.HandleProjectileHit(collision.gameObject);
        } 
    }

    public void HandleCollisionExit(HeroController hero, Collision2D collision) {
        // Implement logic for when the hero exits a collision (e.g., stop taking damage, etc.)
    }

    public void HandleTriggerEnter(HeroController hero, Collider2D collider) {
        if(collider.gameObject.CompareTag("BadGuy")) {
            hero.nearbyEnemies.Add(collider.transform);
            if(currentTarget == null) {
                currentTarget = collider.transform;
            }
        }
    }

    public void HandleTriggerExit(HeroController hero, Collider2D collider) {
        // Implement logic for when the hero exits a trigger (e.g., stop detecting player proximity, etc.)
        if(collider.gameObject.CompareTag("BadGuy")) {
            hero.nearbyEnemies.Remove(collider.transform);
            if(currentTarget == collider.transform) {
                currentTarget = null;
                FindNewTarget(hero);
            }
        }
    }

    void RotateTowardsTarget(HeroController hero) {
        if (currentTarget == null) return;

        Vector2 direction = currentTarget.position - heroTransform.position;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        targetAngle -= 90f; 

        // Smoothly interpolate the angle (Mathf.LerpAngle safely handles the 360-to-0 degree wrap-around)
        float smoothedAngle = Mathf.LerpAngle(hero.EntityRB.rotation, targetAngle, hero.rotationSpeed * Time.fixedDeltaTime);
        // Tell the physics engine to move the rotation
        hero.EntityRB.MoveRotation(smoothedAngle);
    }

    void MoveTowardsTarget(HeroController hero) {
        if (currentTarget == null) return;

        if (Vector2.Distance(heroTransform.position, currentTarget.position) <= hero.attackRange) {
            // We are in attack range, so we can stop moving and attack
            hero.EntityRB.linearVelocity = Vector2.zero; // Stop moving
            enemyInRange = true;

        } else {
            Vector2 direction = currentTarget.position - heroTransform.position;
            hero.EntityRB.linearVelocity = direction.normalized * hero.moveSpeed; // Move towards the target enemy at a constant speed
            enemyInRange = false;
        }
    }

    void Attack(HeroController hero) {
        // Implement attack logic (e.g., reduce enemy health, play attack animation, etc.)
        if (Time.time >= nextAtackTime) {
            Debug.Log($"Attacking {currentTarget.name}!");
            nextAtackTime = Time.time + hero.attackCooldown;
        }
    }

    void FindNewTarget(HeroController hero) {
        if (hero.nearbyEnemies.Count > 0) {
            float closestDistance = Mathf.Infinity;
            foreach (Transform enemy in hero.nearbyEnemies) {
                if(enemy == null) continue; // Skip if the enemy has been destroyed
                float distance = Vector2.Distance(heroTransform.position, enemy.position);
                if (distance < closestDistance) {
                    closestDistance = distance;
                    currentTarget = enemy;
                }
            }
        } else {
            // No more nearby enemies, switch back to patrolling
            hero.SwitchState(HeroController.HeroStateLabel.Patrolling);
        }
    }

    private void CleanDeadEnemies(HeroController hero)
    {
        // RemoveWhere is a highly optimized HashSet function that instantly 
        // wipes out any element that matches our condition (in this case, being null)
        hero.nearbyEnemies.RemoveWhere(enemy => enemy == null);
    }
    
}
