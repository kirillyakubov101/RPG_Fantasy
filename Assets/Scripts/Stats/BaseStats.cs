using UnityEngine;

namespace FantasyTown.Stats
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/NewStat", order = 1)]
    public class BaseStats : ScriptableObject
    {
        [SerializeField] private Stats[] stats;

        [System.Serializable]
        public struct Stats
        {
            [SerializeField] public int level;
            [SerializeField] public float healthpts;
        }

        public Stats GetCurrentStats(int currentLevel)
        {
            if(stats.Length == 0 || stats.Length < currentLevel) { return default(Stats); }
            return stats[currentLevel - 1];
        }
    
    }

}
