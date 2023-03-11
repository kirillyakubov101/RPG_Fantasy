using UnityEngine;

namespace FantasyTown.States
{
    public class PlayerAttackState : State
    {
        [SerializeField] private float rotationSpeed = 2f;
        [SerializeField] private HitSounds hitSounds;

        private readonly int AttackStateHash = Animator.StringToHash("Attack");
        private const float CrossFadeDuration = 0.1f;

        private Transform targetTransform = null;

        public PlayerAttackState()
        {
            base.CurrentStateName = StateName.ATTACK;
        }

        public override void EnterState()
        {
            PlayerStateMachine.GetInstance().Animator.CrossFadeInFixedTime(AttackStateHash, CrossFadeDuration);
            PlayerStateMachine.GetInstance().Agent.isStopped = true;

            targetTransform = PlayerActionHandler.GetInstance().Target.transform;
        }

        public override void ExitState()
        {
            targetTransform = null;
        }

        public override void Tick(float deltaTime)
        {
            if(targetTransform == null)
            {
                PlayerStateMachine.GetInstance().SwitchState(StateName.NORMAL); return;
            }

            //Get the direction and rotation
            Vector3 directionToTarget = (targetTransform.position - transform.position).normalized;
            Quaternion lookDirection = Quaternion.LookRotation(directionToTarget);
            Quaternion goalRotation = Quaternion.Lerp(transform.rotation, lookDirection, deltaTime * rotationSpeed);
            Vector3 goalEuler = goalRotation.eulerAngles;
            transform.rotation = Quaternion.Euler(0f, goalEuler.y, 0f);
        }

        //Animation Event
        public void Hit()
        {
            if (PlayerActionHandler.GetInstance().Target == null || !PlayerActionHandler.GetInstance().Target.IsAlive) { PlayerStateMachine.GetInstance().SwitchState(StateName.NORMAL); return; }
            if (PlayerActionHandler.GetInstance().Target.IsAlive)
            {
                hitSounds.AssignNewSoundClip(PlayerActionHandler.GetInstance().Fighter.GetCurrentWeapon().WeaponStats.GetWeaponHitSound()); //Play hit sound

                //damage target
                PlayerActionHandler.GetInstance().Target.TakeDamage(PlayerActionHandler.GetInstance().Fighter.GetCurrentWeapon().WeaponStats.GetWeaponDamage());

                //if the taget is dead
                if (!PlayerActionHandler.GetInstance().Target.IsAlive)
                {
                    PlayerActionHandler.GetInstance().Target = null; 
                    PlayerStateMachine.GetInstance().SwitchState(StateName.NORMAL);
                }
            }
        }

        //For Bow
        public void ShootArrow()
        {
            hitSounds.AssignNewSoundClip(PlayerActionHandler.GetInstance().Fighter.GetCurrentWeapon().WeaponStats.GetWaponLaunchSound()); //Play Launch sound
            var newProjectile =  Instantiate(PlayerActionHandler.GetInstance().Fighter.CurrentProjectile, PlayerActionHandler.GetInstance().Fighter.ShootPoint);
            newProjectile.transform.SetParent(null);
            newProjectile.AssignOwner(this, PlayerActionHandler.GetInstance().Target);
        }   
    }
}


