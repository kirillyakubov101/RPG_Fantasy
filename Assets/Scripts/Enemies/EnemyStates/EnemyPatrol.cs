using System.Collections;
using FantasyTown.Entity;
using FantasyTown.Pathing;
using UnityEngine;

namespace FantasyTown.States
{
    public class EnemyPatrol : State
    {
        [SerializeField] private float patrolSpeed = 2f;

        private Coroutine current;
        private Enemy m_enemy;

        private readonly int WalkStateBlendTreeHash = Animator.StringToHash("Walk");
        private const float CrossFadeDuration = 0.1f;
        private float startingSpeed;
       

        private void Awake()
        {
            m_enemy = GetComponent<Enemy>();
        }
        public EnemyPatrol()
        {
            base.CurrentStateName = StateName.PATROL;
        }

        public override void EnterState()
        {
            m_enemy.StateMachine.Agent.isStopped = false;
            startingSpeed = m_enemy.StateMachine.Agent.speed; //cache speed
            m_enemy.StateMachine.Agent.speed = patrolSpeed; //set patrol speed
            m_enemy.StateMachine.Animator.CrossFadeInFixedTime(WalkStateBlendTreeHash, CrossFadeDuration);
            current = StartCoroutine(StartPatroling());
        }

        public override void ExitState()
        {
            m_enemy.StateMachine.Agent.speed = startingSpeed;
            StopCoroutine(current);
        }

        public override void Tick(float deltaTime)
        {
            //
        }

        private IEnumerator StartPatroling()
        {
            Entity.Enemy currentEnemy = GetComponent<Entity.Enemy>();
            Path currentPath = currentEnemy.Path;
            Vector3 nextPos = currentPath.GetNextWaypoint();

            m_enemy.StateMachine.Agent.SetDestination(nextPos);

            while (true)
            {
                if (UtilsHelper.SqrDistance(transform.position, nextPos) <= 1f)
                {
                    nextPos = currentPath.GetNextWaypoint();
                    m_enemy.StateMachine.Agent.SetDestination(nextPos);
                }

                yield return null;
            }
        }
    }

}
