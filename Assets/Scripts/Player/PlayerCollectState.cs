using System.Collections;
using UnityEngine;

namespace FantasyTown.States
{
    public class PlayerCollectState : State
    {
        private readonly int NormalStateSpeedHash = Animator.StringToHash("NormalSpeed");
        private readonly int NormalStateBlendTreeHash = Animator.StringToHash("Normal State BlendTree");

        private const float AnimatorDampTime = 0.1f;
        private const float CrossFadeDuration = 0.1f;

        private Coroutine current;

        public PlayerCollectState()
        {
            base.CurrentStateName = StateName.COLLECT;
        }

        public override void EnterState()
        {
            print("ENTER COLLECT");
            PlayerStateMachine.GetInstance().Agent.isStopped = false;
            PlayerStateMachine.GetInstance().Animator.CrossFadeInFixedTime(NormalStateBlendTreeHash, CrossFadeDuration);

           

            current = StartCoroutine(WalkTowardsCollectable());
        }

        public override void ExitState()
        {
            print("exit COLLECT");
            PlayerStateMachine.GetInstance().Agent.isStopped = true;

            if(current != null)
            {
                StopCoroutine(current);
            }
        }

        public override void Tick(float deltaTime)
        {
            float speed = PlayerStateMachine.GetInstance().IsAgentMoving() ? 1f : 0f;
            PlayerStateMachine.GetInstance().Animator.SetFloat(NormalStateSpeedHash, speed, AnimatorDampTime, deltaTime);
        }

        private IEnumerator WalkTowardsCollectable()
        {
            Vector3 movePos = PlayerActionHandler.GetInstance().ActionPosition;
            PlayerStateMachine.GetInstance().Agent.SetDestination(movePos);

            while (UtilsHelper.SqrDistance(movePos,transform.position) > 2f)
            {
                yield return null;
            }

            PlayerActionHandler.GetInstance().CollectItem();
            PlayerStateMachine.GetInstance().SwitchState(StateName.NORMAL);
        }
    }
}

