using FantasyTown.Entity;
using FantasyTown.States;
using UnityEngine;

public class EnemyStateMachine : StateMachine
{
    [SerializeField] private State enemyIdleState = null;
    [SerializeField] private State enemyPatrolState = null;
    [SerializeField] private State enemyAttackState = null;
    [SerializeField] private State enemyChaseState = null;
    [SerializeField] private State enemyDeathState = null;

    public void Start()
    {
        InitStartingState();
    }

    public void SwitchState(State.StateName newState)
    {
        if (newState == m_currentState.CurrentStateName) { return; } //do not enter the same state twice or more
        if (m_currentState != null)                                  //clear the previous state
        {
            m_currentState.ExitState();
        }

        State tempState = null;

        switch (newState)
        {
            case State.StateName.ATTACK:
                tempState = enemyAttackState;
                break;
            case State.StateName.PATROL:
                tempState = enemyPatrolState;
                break;
            case State.StateName.NORMAL:
                tempState = enemyIdleState;
                break;
            case State.StateName.CHASE:
                tempState = enemyChaseState;
                break;
            case State.StateName.DEATH:
                tempState = enemyDeathState;
                break;
        }

        m_currentState = tempState;
        m_currentState.EnterState();
    }

    
    //Starting state init
    private void InitStartingState()
    {
        if (GetComponent<Enemy>().Path != null)
        {
            m_currentState = enemyPatrolState;
        }
        else
        {
            m_currentState = enemyIdleState;
        }
       
        m_currentState.EnterState();
    }


}
