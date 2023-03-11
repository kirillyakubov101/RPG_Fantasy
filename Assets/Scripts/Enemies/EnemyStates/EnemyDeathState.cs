using FantasyTown.Entity;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace FantasyTown.States
{
    public class EnemyDeathState : State
    {
        private readonly int deathAnimHash = Animator.StringToHash("Death");
        private const float CrossFadeDuration = 0.1f;
        [SerializeField] private UnityEvent OnDeath = null;

        private Enemy m_enemy;

        private void Awake()
        {
            m_enemy = GetComponent<Enemy>();
        }

        public EnemyDeathState()
        {
            base.CurrentStateName = StateName.DEATH;
        }

        public override void EnterState()
        {
            m_enemy.StateMachine.Agent.isStopped = true;
            m_enemy.StateMachine.Animator.CrossFadeInFixedTime(deathAnimHash, CrossFadeDuration);
            OnDeath?.Invoke();
            print("dead");
            StartCoroutine(BurryBody());
        }

        public override void ExitState()
        {
            //no exit from death
        }

        public override void Tick(float deltaTime)
        {
            //
        }

        private IEnumerator BurryBody()
        {
            float deathTimer = 0f;
            yield return new WaitForSeconds(2f);
            while(deathTimer <= 10f)
            {
                transform.Translate(Vector3.down * Time.deltaTime * 1.1f);
                deathTimer += Time.deltaTime;
                yield return null;
            }

            Destroy(gameObject);
        }

       
    }
}



