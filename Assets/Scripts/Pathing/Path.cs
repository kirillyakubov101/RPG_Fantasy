using UnityEngine;

namespace FantasyTown.Pathing
{
    public class Path : MonoBehaviour
    {
        [SerializeField] Transform[] Waypoints = null;

        private int index = -1;

        public Vector3 GetNextWaypoint()
        {
            index = (index + 1) % Waypoints.Length;
            return Waypoints[index].position;
        }
    }
}

