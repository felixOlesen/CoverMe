using UnityEngine;

public class Enemy_AttackingState : EnemyState
{
    public Transform currentTarget;
    private bool enemyInRange = false;
    public float nextAtackTime = 0f;

    public void EnterState(EnemyController enemy)
    {
        // Initialize attacking behavior (e.g., set attack parameters, start attack animation, etc.)
    }

    public void UpdateState(EnemyController enemy)
    {
        // Implement attacking logic (e.g., move towards target enemy, check for attack range, etc.)
        if (currentTarget == null && enemy.nearbyTargets.Count > 0)
        {
            CleanDeadEnemies(enemy);
            FindNewTarget(enemy);
        }

        // If we have a valid target, do your attack/rotation logic here!
        if (currentTarget != null)
        {
            Debug.DrawLine(enemy.transform.position, currentTarget.position, Color.red);
        }

        if(enemyInRange) {
            // Perform attack logic
            Attack(enemy);
        }
    }

    public void FixedUpdateState(EnemyController enemy)
    {
        RotateTowardsTarget(enemy);
        MoveTowardsTarget(enemy);
    }

    public void ExitState(EnemyController enemy)
    {
        // Clean up attacking behavior (e.g., stop attack animation, reset variables, etc.)
    }

    public void HandleCollisionEnter(EnemyController enemy, Collision2D collision)
    {
        // Implement logic for when the enemy enters a collision while attacking (e.g., take damage, etc.)
    }

    public void HandleCollisionExit(EnemyController enemy, Collision2D collision)
    {
        // Implement logic for when the enemy exits a collision while attacking (e.g., stop taking damage, etc.)
    }

    public void HandleTriggerEnter(EnemyController enemy, Collider2D collider)
    {
        if(collider.gameObject.CompareTag("GoodGuy") || collider.gameObject.CompareTag("Player")) {
            enemy.nearbyTargets.Add(collider.transform);
            if(currentTarget == null) {
                currentTarget = collider.transform;
            }
        }    
    }

    public void HandleTriggerExit(EnemyController enemy, Collider2D collider)
    {
        // Implement logic for when the enemy exits a trigger while attacking (e.g., stop detecting player proximity, etc.)
        if(collider.gameObject.CompareTag("GoodGuy") || collider.gameObject.CompareTag("Player")) {
            enemy.nearbyTargets.Remove(collider.transform);
            if(currentTarget == collider.transform) {
                currentTarget = null;
                FindNewTarget(enemy);
            }
        }
    }

    void RotateTowardsTarget(EnemyController enemy) {
        if (currentTarget == null) return;

        Vector2 direction = currentTarget.position - enemy.transform.position;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        targetAngle -= 90f; 

        // Smoothly interpolate the angle (Mathf.LerpAngle safely handles the 360-to-0 degree wrap-around)
        float smoothedAngle = Mathf.LerpAngle(enemy.EntityRB.rotation, targetAngle, enemy.rotationSpeed * Time.fixedDeltaTime);
        // Tell the physics engine to move the rotation
        enemy.EntityRB.MoveRotation(smoothedAngle);
    }

    void MoveTowardsTarget(EnemyController enemy) {
        if (currentTarget == null) return;

        if (Vector2.Distance(enemy.transform.position, currentTarget.position) <= enemy.attackRange) {
            // We are in attack range, so we can stop moving and attack
            enemy.EntityRB.linearVelocity = Vector2.zero; // Stop moving
            enemyInRange = true;

        } else {
            Vector2 direction = currentTarget.position - enemy.transform.position;
            enemy.EntityRB.linearVelocity = direction.normalized * enemy.moveSpeed; // Move towards the target enemy at a constant speed
            enemyInRange = false;
        }
    }

    void Attack(EnemyController enemy) {
        // Implement attack logic (e.g., reduce enemy health, play attack animation, etc.)
        if (Time.time >= nextAtackTime) {
            Debug.Log($"Attacking {currentTarget.name}!");
            nextAtackTime = Time.time + enemy.attackCooldown;
        }
    }

    void FindNewTarget(EnemyController enemy) {
        if (enemy.nearbyTargets.Count > 0) {
            float closestDistance = Mathf.Infinity;
            foreach (Transform target in enemy.nearbyTargets) {
                if(target == null) continue; // Skip if the target has been destroyed
                float distance = Vector2.Distance(enemy.transform.position, target.position);
                if (distance < closestDistance) {
                    closestDistance = distance;
                    currentTarget = target;
                }
            }
        } else {
            enemy.SwitchState(EnemyController.EnemyStateLabel.Searching);
        }
    }

    private void CleanDeadEnemies(EnemyController enemy)
    {
        // RemoveWhere is a highly optimized HashSet function that instantly 
        // wipes out any element that matches our condition (in this case, being null)
        enemy.nearbyTargets.RemoveWhere(target => target == null);
    }
}
