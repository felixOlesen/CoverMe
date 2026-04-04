using UnityEngine;

public interface HeroState
{
    public void EnterState(HeroController hero);
    public void UpdateState(HeroController hero);
    public void FixedUpdateState(HeroController hero);
    public void ExitState(HeroController hero);
    public void HandleCollision(HeroController hero, Collision2D collision);
    public void HandleTrigger(HeroController hero, Collider2D collider);
}

public class HeroController : Entity
{
    
    public enum HeroStateLabel { Idle, Patrolling, Attacking }
    private HeroStateLabel currentStateLabel = HeroStateLabel.Idle;
    private PatrollingState patrollingState;
    private AttackingState attackingState;
    private HeroState currentState;
    private GameObject targetEnemy;
    public float sightDistance = 2f; 
    public float attackRange = 1f;


    void Awake() {
        patrollingState = new PatrollingState(gameObject);
        attackingState = new AttackingState(gameObject);
        EntityRB = gameObject.GetComponent<Rigidbody2D>();
    }

    void Start() {
        SwitchState(HeroStateLabel.Patrolling);
    }

    void Update() {
        if(currentState != null) {
            currentState.UpdateState(this);
        }
        
    }

    void FixedUpdate() {
        if(currentState != null) {
            currentState.FixedUpdateState(this);
        }
    }

    public void SwitchState(HeroStateLabel newState) {
        if (currentState != null){
            currentState.ExitState(this);
        }
        currentStateLabel = newState;
        switch (currentStateLabel) {
            case HeroStateLabel.Idle:
                // Set currentState to IdleState (not implemented yet)
                break;
            case HeroStateLabel.Patrolling:
                currentState = patrollingState;
                break;
            case HeroStateLabel.Attacking:
                attackingState.targetEnemy = targetEnemy; // Pass the target enemy to the attacking state
                currentState = attackingState;
                break;
        }
        currentState.EnterState(this);
    }


    void OnCollisionEnter2D(Collision2D collision) {
        currentState.HandleCollision(this, collision);
    }

    void OnTriggerEnter2D(Collider2D collider) {
        currentState.HandleTrigger(this, collider);
    }

    public void SetTargetEnemy(GameObject enemy) {
        targetEnemy = enemy;
    }
}
