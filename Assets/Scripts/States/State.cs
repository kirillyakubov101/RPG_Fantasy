using UnityEngine;

namespace FantasyTown.States
{
    public abstract class State : MonoBehaviour
    {
        public enum StateName
        {
            NORMAL,
            ATTACK,
            COLLECT,
            CHASE,
            PATROL,
            DEATH
        }
        public StateName CurrentStateName { get; protected set; }

        public abstract void EnterState();
        public abstract void ExitState();
        public abstract void Tick(float deltaTime);

    
    }
}


