using FantasyTown.Entity;
using System.Collections;
using UnityEngine;

namespace FantasyTown.States
{
    public class EnemyIdle : State
    {
        [SerializeField] private float walkingSpeed = 2f;

        private readonly int IdleStateBlendTreeHash = Animator.StringToHash("Idle");
        private readonly int WalkStateBlendTreeHash = Animator.StringToHash("Walk");

        private const float CrossFadeDuration = 0.1f;

        private Enemy m_enemy;
        private Vector3 m_startingPos; //the pos to go back to after the chase
        private Quaternion m_startingRot;
        private Coroutine walkingBackRoutine;
        private float startSpeed;
        private float startingPosOffset = 7f;
       
        private void Awake()
        {
            m_enemy = GetComponent<Enemy>();
            m_startingPos = transform.position;
            m_startingRot= transform.rotation;
        }
        public EnemyIdle()
        {
            base.CurrentStateName = StateName.NORMAL;
        }

        public override void EnterState()
        {
            startSpeed = m_enemy.StateMachine.Agent.speed;
            ProcessAnimationAssign();
            m_enemy.StateMachine.Agent.isStopped = false;

        }

        public override void ExitState()
        {
            if (walkingBackRoutine != null)
            {
                StopCoroutine(walkingBackRoutine);
            }

            m_enemy.StateMachine.Agent.speed = startSpeed;
        }

        public override void Tick(float deltaTime)
        {
           //
        }

        private void ProcessAnimationAssign()
        {
            if(UtilsHelper.SqrDistance(m_startingPos, transform.position) >= startingPosOffset)
            {
                walkingBackRoutine = StartCoroutine(WalkBackToBase());
            }
            else
            {
                m_enemy.StateMachine.Animator.CrossFadeInFixedTime(IdleStateBlendTreeHash, CrossFadeDuration);
            }
        }

        private IEnumerator WalkBackToBase()
        {
            m_enemy.StateMachine.Animator.CrossFadeInFixedTime(WalkStateBlendTreeHash, CrossFadeDuration);
            m_enemy.StateMachine.Agent.SetDestination(m_startingPos);
            m_enemy.StateMachine.Agent.speed = walkingSpeed;

            while (UtilsHelper.SqrDistance(m_startingPos,transform.position) > 0.1f)
            {
                yield return null;
            }
            while(Quaternion.Angle(transform.rotation, m_startingRot) >2f)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, m_startingRot, Time.deltaTime * 3f);
                yield return null;
            }
            

            m_enemy.StateMachine.Animator.CrossFadeInFixedTime(IdleStateBlendTreeHash, CrossFadeDuration);
            m_enemy.StateMachine.Agent.speed = startSpeed;
        }
    }

}
