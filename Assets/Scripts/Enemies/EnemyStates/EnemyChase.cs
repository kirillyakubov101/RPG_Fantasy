using FantasyTown.Entity;
using UnityEngine;

namespace FantasyTown.States
{
    public class EnemyChase : State
    {
        [SerializeField] private float chaseRange = 10f;

        private readonly int PatrolStateBlendTreeHash = Animator.StringToHash("Chase");
        private const float CrossFadeDuration = 0.05f;

        private float readyForAttackTimer = 0f;
        private readonly float readyForAttackMaxTime = 0.15f;

        private Enemy m_enemy;

        private void Awake()
        {
            m_enemy = GetComponent<Enemy>();
        }

        public EnemyChase()
        {
            base.CurrentStateName = StateName.CHASE;
        }

        public override void EnterState()
        {
            m_enemy.StateMachine.Agent.isStopped = false;
            m_enemy.StateMachine.Animator.CrossFadeInFixedTime(PatrolStateBlendTreeHash, CrossFadeDuration);
        }

        public override void ExitState()
        {
            m_enemy.StateMachine.Agent.isStopped = true;
            readyForAttackTimer = 0f;
        }

        public override void Tick(float deltaTime)
        {
            //Player is too far from the enemy to continue chasing
            if((m_enemy.Player.transform.position - transform.position).sqrMagnitude >= (chaseRange * chaseRange))
            {
                if (m_enemy.Path)
                {
                    m_enemy.IsLookingForPlayer = true;
                    m_enemy.StateMachine.SwitchState(StateName.PATROL);
                    return;
                }
                else //Player is out of range | keep searching for him without chase
                {
                    m_enemy.IsLookingForPlayer = true;
                    m_enemy.StateMachine.SwitchState(StateName.NORMAL);
                    return;
                }
              
            }
            if (m_enemy.StateMachine.Agent)
            {
                m_enemy.StateMachine.Agent.SetDestination(m_enemy.Player.transform.position);
            }
            

            //If player is in range of attack
            if(Vector3.Distance(m_enemy.Player.transform.position,transform.position) < 2f)
            {
                readyForAttackTimer += Time.deltaTime;
                if(readyForAttackTimer >= readyForAttackMaxTime)
                {
                    m_enemy.StateMachine.SwitchState(StateName.ATTACK);
                    return;
                }
                
            }
            else
            {
                readyForAttackTimer = 0f;
            }
        }


    }

}
