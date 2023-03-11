using FantasyTown.States;
using FantasyTown.Stats;
using System.Collections;
using UnityEngine;

namespace FantasyTown.Projectiles
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float speed;
      
        private PlayerAttackState playerAttack;
        private EnemyAttack enemyAttack;

        private CombatTarget target; //player's target
        private Transform targetTransform;

        private void Start()
        {
            Destroy(gameObject, 7f);
        }

        //PLAYER IS SHOOTING
        public void AssignOwner(PlayerAttackState _PlayerAttackState, CombatTarget target)
        {
            playerAttack = _PlayerAttackState;
            this.target = target;
            targetTransform = target.transform;
            

            StartCoroutine(ProcessPorjectileMove());
        }
        
        //ENEMY IS SHOOTING
        public void AssignOwner(EnemyAttack _enemyAttack)
        {
            enemyAttack = _enemyAttack;
            targetTransform = target.transform;
        }

        private void ArrowHit()
        {
            playerAttack.Hit(); //player hits his target
            Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out CombatTarget enemy))
            {
                if (enemy == target)
                {
                    ArrowHit();
                }

            }
        }

        private IEnumerator ProcessPorjectileMove()
        {
            while(true)
            {
                if(targetTransform == null) { Destroy(gameObject); }
                try
                {
                    Quaternion rotationToTarget = Quaternion.LookRotation(targetTransform.position - transform.position);
                    Quaternion fullRotation = Quaternion.Lerp(transform.rotation, rotationToTarget, Time.deltaTime * 20f);

                    fullRotation.eulerAngles = new Vector3(0, fullRotation.eulerAngles.y, 0);
                    transform.rotation = fullRotation;

                    transform.Translate(Vector3.forward * Time.deltaTime * speed);
                }
                catch
                {
                    Debug.LogError("The Enemy Died");
                }
                

                yield return null;
            }
        }

    }

}
