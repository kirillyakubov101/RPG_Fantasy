using FantasyTown.States;
using UnityEngine;
using UnityEngine.AI;

public class StateMachine : MonoBehaviour
{
    private Animator m_animator;
    private NavMeshAgent m_agent;
    protected State m_currentState;
    public Animator Animator { get => m_animator;}
    public NavMeshAgent Agent { get => m_agent;  }

    protected virtual void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_agent = GetComponent<NavMeshAgent>();
    }

    //-------------MAIN LOOP-----------
    private void Update()
    {
        if (m_currentState == null) { return; }

        m_currentState.Tick(Time.deltaTime);
    }
    //---------------------------------------

    public State GetCurrentState()
    {
        return m_currentState;
    }

    //It is just to change the attack animation
    public virtual void SwitchAnimatorController(AnimatorOverrideController controller)
    {
        m_animator.runtimeAnimatorController = controller;
    }

}
