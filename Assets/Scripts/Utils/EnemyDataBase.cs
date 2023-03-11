using FantasyTown.Saving;
using FantasyTown.Stats;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDataBase : MonoBehaviour, ISaveable
{
    public static EnemyDataBase Instance;

    private Dictionary<string, CombatTarget.EnemyProperties> m_enemies = new Dictionary<string, CombatTarget.EnemyProperties>();
    private CombatTarget[] m_listOfEnemies;

    private void Awake()
    {
        m_listOfEnemies = FindObjectsOfType<CombatTarget>();

        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //first time the enemy sends its information to the database
    public void PopulateDataBase(CombatTarget target)
    {
        if (m_enemies.ContainsKey(target.UniqueIdentifier.GetUniqueId())) { return; }
        m_enemies.Add(target.UniqueIdentifier.GetUniqueId(), target.CurrentEnemyProperties);
    }

    //the enemy updates its existing info on the data base
    public void UpdateDataBase(CombatTarget target)
    {
        target.SaveProperties(target.transform.position);
        m_enemies[target.UniqueIdentifier.GetUniqueId()] = target.CurrentEnemyProperties;
    }

    public void CaptureState()
    { 
        //All the Enemies in the scene
        foreach(CombatTarget target in m_listOfEnemies)
        {
            if (target == null) { continue; }
            this.UpdateDataBase(target);
        }

        SavingWrapper.Instance.Data.combatTargetsDict = m_enemies;
    }

    public void RestoreState()
    {
        //All the enemies
        m_enemies.Clear();
        m_listOfEnemies = FindObjectsOfType<CombatTarget>();
        m_enemies = SavingWrapper.Instance.Data.combatTargetsDict;

        foreach (var target in m_listOfEnemies)
        {
            if (target == null) { continue; }
            if (m_enemies.ContainsKey(target.UniqueIdentifier.GetUniqueId()))
            {
                target.LoadProperties(m_enemies[target.UniqueIdentifier.GetUniqueId()]);
                if (!target.IsAlive)
                {
                    target.ManualDeath();
                }
            }
        }
    }
}
