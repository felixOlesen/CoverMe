using UnityEngine;
using System.Collections.Generic; 

public interface HeroState
{
    public void EnterState(HeroController hero);
    public void UpdateState(HeroController hero);
    public void FixedUpdateState(HeroController hero);
    public void ExitState(HeroController hero);
    public void HandleCollisionEnter(HeroController hero, Collision2D collision);
    public void HandleCollisionExit(HeroController hero, Collision2D collision);
    public void HandleTriggerEnter(HeroController hero, Collider2D collider);
    public void HandleTriggerExit(HeroController hero, Collider2D collider);
}

public class HeroController : Entity
{
    
    public enum HeroStateLabel { Idle, Patrolling, Attacking }
    private HeroStateLabel currentStateLabel = HeroStateLabel.Idle;
    private PatrollingState patrollingState;
    private AttackingState attackingState;
    private HeroState currentState;
    public float sightDistance = 2f; 
    public float attackRange = 1f;

    public HashSet<Transform> nearbyEnemies;

    void Awake() {
        patrollingState = new PatrollingState(gameObject);
        attackingState = new AttackingState(gameObject);
        EntityRB = gameObject.GetComponent<Rigidbody2D>();
        nearbyEnemies = new HashSet<Transform>();
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
                currentState = attackingState;
                break;
        }
        currentState.EnterState(this);
    }


    void OnCollisionEnter2D(Collision2D collision) {
        currentState.HandleCollisionEnter(this, collision);
    }

    void OnCollisionExit2D(Collision2D collision) {
        currentState.HandleCollisionExit(this, collision);
    }

    void OnTriggerEnter2D(Collider2D collider) {
        currentState.HandleTriggerEnter(this, collider);
    }

    void OnTriggerExit2D(Collider2D collider) {
        currentState.HandleTriggerExit(this, collider);
    }
}
