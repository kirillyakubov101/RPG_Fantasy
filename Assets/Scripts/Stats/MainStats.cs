using FantasyTown.Saving;
using System;
using UnityEngine;

namespace FantasyTown.Stats
{
    public class MainStats : MonoBehaviour,ISaveable
    {
        [SerializeField] private BaseStats baseStats;

        [SerializeField] private float maxHp;
        [SerializeField] private float currentHp;
        [SerializeField] private int currentLevel;
        [SerializeField] private float currentExp;

        private BaseStats.Stats m_currentStats;
        public event Action<float> OnStatsInit; //this event is called to activate the player's stats health after init

        private void Start()
        {
            InitStats();
        }

        private void InitStats()
        {
            m_currentStats = baseStats.GetCurrentStats(currentLevel);
            this.maxHp = m_currentStats.healthpts;
            this.currentHp = this.maxHp;

            OnStatsInit?.Invoke(this.currentHp);
        }

        //TODO: LIST BELOW
        //GET HIT
        //LEVEL UP
        //GET EXP REWARD
        //IS DEAD


        //SAVING SYSTEM
        public void CaptureState()
        {
            SavingWrapper.Instance.Data.playerLevel = currentLevel;
            SavingWrapper.Instance.Data.playerCurrentHealth = currentHp;
            SavingWrapper.Instance.Data.playerExp = currentExp;
        }

        public void RestoreState()
        {
            currentLevel = SavingWrapper.Instance.Data.playerLevel;
            InitStats();
            currentHp = SavingWrapper.Instance.Data.playerCurrentHealth;
            currentExp = SavingWrapper.Instance.Data.playerExp;
        }

    }
}

