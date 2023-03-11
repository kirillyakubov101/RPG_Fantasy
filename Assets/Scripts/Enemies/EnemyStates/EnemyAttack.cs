using FantasyTown.Entity;
using UnityEngine;

namespace FantasyTown.States
{
    public class EnemyAttack : State
    {
        [SerializeField] private float m_attackRange = 3f; //TODO scriptable object, not here
        [SerializeField] private float m_rotationSpeed = 2f;

        private readonly float readyForChaseMaxTime = 0.15f;
        private float readyForChaseTimer = 0f;

        private Enemy m_enemy;
        private float m_weaponDamage;

        private readonly int PatrolStateBlendTreeHash = Animator.StringToHash("Attack");
        private const float CrossFadeDuration = 0.1f;

        private void Awake()
        {
            m_enemy = GetComponent<Enemy>();
        }

        public EnemyAttack()
        {
            base.CurrentStateName = StateName.ATTACK;
        }

        public override void EnterState()
        {
            m_enemy.StateMachine.Agent.isStopped = true;
            m_enemy.StateMachine.Animator.CrossFadeInFixedTime(PatrolStateBlendTreeHash, CrossFadeDuration);

            if(m_weaponDamage ==0 || m_attackRange == 0)
            {
                m_weaponDamage = m_enemy.WeaponWrapper.WeaponStats.GetWeaponDamage();
                m_attackRange = m_enemy.WeaponWrapper.WeaponStats.GetWeaponRange();
            }
           
        }

        public override void ExitState()
        {
            m_enemy.StateMachine.Agent.isStopped = false;
            readyForChaseTimer = 0f;
        }

        public override void Tick(float deltaTime)
        {
            if((m_enemy.Player.transform.position - transform.position).sqrMagnitude >= m_attackRange)
            {
                readyForChaseTimer += Time.deltaTime;
                if(readyForChaseTimer >= readyForChaseMaxTime)
                {
                    m_enemy.StateMachine.SwitchState(StateName.CHASE);
                }
            }
            else
            {
                readyForChaseTimer = 0f;
                Vector3 directionToPlayer = (m_enemy.Player.transform.position - transform.position).normalized;
                Quaternion goalRotation = Quaternion.LookRotation(directionToPlayer);
                goalRotation.eulerAngles = new Vector3(0f,goalRotation.eulerAngles.y, goalRotation.eulerAngles.z);
                transform.rotation = Quaternion.Slerp(transform.rotation, goalRotation, deltaTime * m_rotationSpeed);
            }
        }

        //Animation Event
        public void Hit()
        {
            m_enemy.Player.TakeDamage(m_weaponDamage);
        }
    }
}

