using UnityEngine;
using System.Collections.Generic;

public interface EnemyState
{
    public void EnterState(EnemyController enemy);
    public void UpdateState(EnemyController enemy);
    public void FixedUpdateState(EnemyController enemy);
    public void ExitState(EnemyController enemy);
    public void HandleCollisionEnter(EnemyController enemy, Collision2D collision);
    public void HandleCollisionExit(EnemyController enemy, Collision2D collision);
    public void HandleTriggerEnter(EnemyController enemy, Collider2D collider);
    public void HandleTriggerExit(EnemyController enemy, Collider2D collider);
}

public class EnemyController : Entity
{
    public enum EnemyStateLabel { Idle, Searching, Attacking }
    public EnemyStateLabel currentStateLabel;
    public float sightDistance = 2f; 
    public float attackRange = 1f;

    private EnemyState currentState;
    private Enemy_SearchingState searchingState;
    private Enemy_AttackingState attackingState;

    public HashSet<Transform> nearbyTargets;

    void Awake() {
        EntityRB = gameObject.GetComponent<Rigidbody2D>();
        searchingState = new Enemy_SearchingState();
        attackingState = new Enemy_AttackingState();
    }

    void Start() {
        nearbyTargets = new HashSet<Transform>();
        SwitchState(EnemyStateLabel.Searching);
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

    public void SwitchState(EnemyStateLabel newStateLabel) {
        if (currentState != null){
            currentState.ExitState(this);
        }
        currentStateLabel = newStateLabel;
        switch (newStateLabel) {
            case EnemyStateLabel.Idle:
                // Set currentState to IdleState (not implemented yet)
                break;
            case EnemyStateLabel.Searching:
                currentState = searchingState;
                break;
            case EnemyStateLabel.Attacking:
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
