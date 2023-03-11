using UnityEngine;

namespace FantasyTown.States
{
    public class PlayerChaseState : State
    {
        private readonly int NormalStateSpeedHash = Animator.StringToHash("NormalSpeed");
        private readonly int NormalStateBlendTreeHash = Animator.StringToHash("Normal State BlendTree");

        private const float AnimatorDampTime = 0.1f;
        private const float CrossFadeDuration = 0.1f;

        public PlayerChaseState()
        {
            base.CurrentStateName = StateName.CHASE;
        }

        public override void EnterState()
        {
            PlayerStateMachine.GetInstance().Agent.isStopped = false;
            PlayerStateMachine.GetInstance().Animator.CrossFadeInFixedTime(NormalStateBlendTreeHash, CrossFadeDuration);
        }

        public override void ExitState()
        {
            PlayerStateMachine.GetInstance().Agent.isStopped = true;
        }

        public override void Tick(float deltaTime)
        {
            if(PlayerStateMachine.GetInstance().Agent.isStopped == true) { PlayerStateMachine.GetInstance().SwitchState(StateName.NORMAL); return; }

            int speed = PlayerStateMachine.GetInstance().IsAgentMoving() ? 1 : 0;
            PlayerStateMachine.GetInstance().Animator.SetFloat(NormalStateSpeedHash, speed, AnimatorDampTime, deltaTime);

            PlayerStateMachine.GetInstance().Agent.SetDestination(PlayerActionHandler.GetInstance().Target.transform.position);

            if (UtilsHelper.SqrDistance(transform.position, PlayerActionHandler.GetInstance().Target.transform.position) < PlayerActionHandler.GetInstance().Fighter.GetCurrentWeapon().WeaponStats.GetWeaponRange())
            {
                PlayerStateMachine.GetInstance().Agent.isStopped = true;
                PlayerStateMachine.GetInstance().SwitchState(StateName.ATTACK);
            }
        }

    }
}

