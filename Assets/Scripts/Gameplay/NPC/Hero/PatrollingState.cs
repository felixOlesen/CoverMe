using UnityEngine;

public class PatrollingState : HeroState
{

    private GameObject hero;
    private Transform heroTransform;
    private LayerMask obstacleLayer = LayerMask.GetMask("Wall");
    private float turnAngle = 90f; // Angle to turn when a wall is detected
    private bool isTurning = false;
    private float turnTimer = 0f;
    private float turnDuration = 0.5f; // Duration of the turn in seconds

    public PatrollingState(GameObject hero)
    {
        this.hero = hero;
        this.heroTransform = hero.transform;
    }

    public void EnterState(HeroController hero)
    {
        // Initialize patrolling behavior (e.g., set waypoints, start walking animation, etc.)
    }

    public void UpdateState(HeroController hero)
    {
        // Implement patrolling logic (e.g., move between waypoints, check for player proximity, etc.)
    }

    public void FixedUpdateState(HeroController hero)
    {
        // Implement patrolling logic (e.g., move between waypoints, check for player proximity, etc.)
        LookForWalls(hero);
        HandleTurning(hero);
    }

    public void ExitState(HeroController hero)
    {
        // Clean up patrolling behavior (e.g., stop walking animation, reset variables, etc.)
    }

    private void LookForWalls(HeroController hero)
    {
        // 1. Define the origin and direction (assuming transform.up is "forward" for your 2D sprite)
        Vector2 origin = heroTransform.position;
        Vector2 direction = heroTransform.up;

        // 2. Shoot the Raycast!

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, hero.sightDistance, obstacleLayer);

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
            Debug.DrawRay(origin, direction * hero.sightDistance, Color.green);
            
            // TODO: Move forward
            hero.EntityRB.linearVelocity = heroTransform.up * hero.moveSpeed; // Move forward at a constant speed
        }
    }

    public void HandleCollision(HeroController hero, Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Projectile")) {
            LevelManager.Instance.HandleProjectileHit(collision.gameObject);
        } 
        if(collision.gameObject.CompareTag("BadGuy")) {
            // Handle collision with enemy (e.g., take damage, play sound, etc.)

        }
        if(collision.gameObject.CompareTag("Wall")) {
            // Handle collision with wall (e.g., bounce, play sound, etc.)
        }
    }

    public void HandleTrigger(HeroController hero, Collider2D collider)
    {
        if(collider.gameObject.CompareTag("BadGuy")) {
            hero.SetTargetEnemy(collider.gameObject);
            hero.SwitchState(HeroController.HeroStateLabel.Attacking);
        }
    }

    private void HandleTurning(HeroController hero) {
        if (isTurning) {
            turnTimer += Time.fixedDeltaTime;
            if (turnTimer >= turnDuration) {
                // Finish the turn
                isTurning = false;
                turnTimer = 0f;
            } else {
                // Rotate over time
                float angleThisFrame = (turnAngle / turnDuration) * Time.fixedDeltaTime;
                hero.EntityRB.MoveRotation(hero.EntityRB.rotation + angleThisFrame);
            }
        }
    }
}
