using UnityEngine;

public class Enemy_SearchingState : EnemyState
{
    private LayerMask obstacleLayer = LayerMask.GetMask("Wall");
    private float turnAngle = 95f; // Angle to turn when a wall is detected
    private bool isTurning = false;
    private float turnTimer = 0f;
    private float turnDuration = 0.5f; // Duration of the turn in seconds

    public void EnterState(EnemyController enemy)
    {
        // Initialize searching behavior (e.g., set search parameters, start search animation, etc.)
    }

    public void UpdateState(EnemyController enemy)
    {
        // Implement searching logic (e.g., move in a pattern, check for player detection, etc.)
    }

    public void FixedUpdateState(EnemyController enemy)
    {
        // Move around the room randomly.
        LookForWalls(enemy);
        HandleTurning(enemy);
        
    }

    public void ExitState(EnemyController enemy)
    {
        // Clean up searching behavior (e.g., stop search animation, reset variables, etc.)
    }

    public void HandleCollisionEnter(EnemyController enemy, Collision2D collision)
    {
        // Implement logic for when the enemy enters a collision (e.g., take damage, etc.)
    }

    public void HandleCollisionExit(EnemyController enemy, Collision2D collision)
    {
        // Implement logic for when the enemy exits a collision (e.g., stop taking damage, etc.)
    }

    public void HandleTriggerEnter(EnemyController enemy, Collider2D collider)
    {
        // Implement logic for when the enemy enters a trigger (e.g., detect player proximity, etc.)
        if (collider.gameObject.CompareTag("GoodGuy") || collider.gameObject.CompareTag("Player")) {
            enemy.nearbyTargets.Add(collider.transform);
            enemy.SwitchState(EnemyController.EnemyStateLabel.Attacking);
        }
    }

    public void HandleTriggerExit(EnemyController enemy, Collider2D collider)
    {
        // Implement logic for when the enemy exits a trigger (e.g., stop detecting player proximity, etc.)
    }

    private void LookForWalls(EnemyController enemy)
    {
        // 1. Define the origin and direction (assuming transform.up is "forward" for your 2D sprite)
        Vector2 origin = enemy.transform.position;
        Vector2 direction = enemy.transform.up;

        // 2. Shoot the Raycast!

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, enemy.sightDistance, obstacleLayer);

        // 3. Check if we hit anything
        if (hit.collider != null)
        {
            // WE HIT A WALL!
            Debug.Log($"Wall detected at {hit.point}. It is {hit.distance} units away.");
            
            // Draw a RED line in the Scene view so you can debug it
            Debug.DrawRay(origin, direction * hit.distance, Color.red);
            
            // TODO: Put your turning logic here (e.g., rotate 90 degrees)
            // Start a coroutine to handle the turn over time
            if (!isTurning) {
                isTurning = true;
                turnTimer = 0f;
            }
        }
        else
        {
            // Path is clear
            // Draw a GREEN line in the Scene view to show the clear path
            Debug.DrawRay(origin, direction * enemy.sightDistance, Color.green);
            
            // TODO: Move forward
            enemy.EntityRB.linearVelocity = enemy.transform.up * enemy.moveSpeed; // Move forward at a constant speed
        }
    }

    private void HandleTurning(EnemyController enemy) {
        if (isTurning) {
            turnTimer += Time.fixedDeltaTime;
            if (turnTimer >= turnDuration) {
                // Finish the turn
                isTurning = false;
                turnTimer = 0f;
            } else {
                // Rotate over time
                float angleThisFrame = (turnAngle / turnDuration) * Time.fixedDeltaTime;
                enemy.EntityRB.MoveRotation(enemy.EntityRB.rotation + angleThisFrame);
            }
        }
    }
}
