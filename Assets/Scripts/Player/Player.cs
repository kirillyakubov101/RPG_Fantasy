using UnityEngine;

namespace FantasyTown.Stats
{
    [RequireComponent(typeof(MainStats))]
    public class Player : MonoBehaviour
    {
        [SerializeField] private MainStats m_mainStats;
        [SerializeField] private float m_playerHp = 0;

        private void OnEnable()
        {
            m_mainStats.OnStatsInit += InitPlayerHp;
        }

        private void OnDestroy()
        {
            m_mainStats.OnStatsInit -= InitPlayerHp;
        }

        private void InitPlayerHp(float newHp)
        {
            m_playerHp = newHp;
        }

        public void TakeDamage(float damage)
        {
            print($"i took {damage} damage and my hp is now {m_playerHp - damage}");
        }
    }

}

