using UnityEngine;
using FantasyTown.States;
using FantasyTown.Saving;

public class PlayerStateMachine : StateMachine,ISaveable
{
    [SerializeField] private State normalState;
    [SerializeField] private State attackState;
    [SerializeField] private State chaseState;
    [SerializeField] private State collectState;

    private static PlayerStateMachine instance = null;

    private PlayerStateMachine() { }

    protected override void Awake()
    {
        base.Awake();

        if(instance != null) { Destroy(this);return; }
        instance = this;
       
    }

    private void Start()
    {
        InitStartingState();
    }

    //-------------MAIN PLAYER LOOP-----------
    private void Update()
    {
        if(m_currentState == null) { return; }

        m_currentState.Tick(Time.deltaTime);
    }

    //---------------------------------------

    public void SwitchState(State.StateName newState)
    {
        if (newState == m_currentState.CurrentStateName) { return; }
        if (m_currentState != null)
        {
            m_currentState.ExitState();
        }

        State tempState = null;

        switch (newState)
        {
            case State.StateName.ATTACK:
                tempState = attackState;
                break;
            case State.StateName.COLLECT:
                tempState = collectState;
                break;
            case State.StateName.NORMAL:
                tempState = normalState;
                break;
            case State.StateName.CHASE:
                tempState = chaseState;
                break;
        }

       
        m_currentState = tempState;
        m_currentState.EnterState();
    }

    private void InitStartingState()
    {
        m_currentState = normalState;
        m_currentState.EnterState();
    }

    public static PlayerStateMachine GetInstance()
    {
        return instance;
    }

    public bool IsAgentMoving()
    {
        return Agent.velocity.magnitude >= 0.1f;
    }

    public void CaptureState()
    {
        SavingWrapper.Instance.Data.playerPosition.UpdateVector(transform.localPosition);
    }

    public void RestoreState()
    {
        this.Agent.enabled = false;
        transform.localPosition = SavingWrapper.Instance.Data.playerPosition.ToVector();
        //this.Agent.SetDestination(transform.localPosition);
        this.Agent.enabled = true;
    }
}
