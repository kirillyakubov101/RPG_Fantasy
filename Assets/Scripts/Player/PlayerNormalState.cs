using UnityEngine;

namespace FantasyTown.States
{
    public class PlayerNormalState : State
    {
        private readonly int NormalStateSpeedHash = Animator.StringToHash("NormalSpeed");
        private readonly int NormalStateBlendTreeHash = Animator.StringToHash("Normal State BlendTree");

        private const float AnimatorDampTime = 0.1f;
        private const float CrossFadeDuration = 0.1f;

        public PlayerNormalState()
        {
            base.CurrentStateName = StateName.NORMAL;
        }

        private void OnEnable()
        {
            PlayerActionHandler.GetInstance().OnMoveClick += DoAction;
        }

        private void OnDestroy()
        {
            PlayerActionHandler.GetInstance().OnMoveClick -= DoAction;
        }

        public override void EnterState()
        {
            PlayerStateMachine.GetInstance().Agent.ResetPath();
            PlayerStateMachine.GetInstance().Agent.isStopped = false;
            PlayerStateMachine.GetInstance().Animator.CrossFadeInFixedTime(NormalStateBlendTreeHash, CrossFadeDuration);
        }

        public override void ExitState()
        {
            //
        }


        //The walk towards action | the player can either stand in place or walk towards
        public void DoAction(Vector3 pos)
        {
            PlayerStateMachine.GetInstance().Agent.SetDestination(pos);
        }

        public override void Tick(float deltaTime)
        {
            float speed = PlayerStateMachine.GetInstance().IsAgentMoving() ? 1f : 0f;
            PlayerStateMachine.GetInstance().Animator.SetFloat(NormalStateSpeedHash, speed, AnimatorDampTime, deltaTime);
        }
    }
}

