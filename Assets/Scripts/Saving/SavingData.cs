using FantasyTown.Stats;
using System.Collections.Generic;
using UnityEngine;
using FantasyTown.Weapons;

namespace FantasyTown.Saving
{
    [System.Serializable]
    public class SavingData
    {
        private static SavingData s_instance;
        public static SavingData Instance
        {
            get
            {
                if(s_instance == null) { s_instance = new SavingData(); }
                return s_instance;
            }
            set
            {
                s_instance = value;
            }
        }

        private SavingData() 
        {
            combatTargetsDict = new Dictionary<string, CombatTarget.EnemyProperties>();
            playerPosition = new SerializableVector3(new Vector3(10,50,10));
            savedItems = new Dictionary<string, int>();
            allPlayerWeapons = new Dictionary<int, WEAPON_COLLECTION>();
        }

        //Combat Targets
        public Dictionary<string, CombatTarget.EnemyProperties> combatTargetsDict;

        //Player Position
        public SerializableVector3 playerPosition;

        //Inventory
        public Dictionary<string, int> savedItems;

        //Player Stats
        public int playerLevel;
        public float playerCurrentHealth;
        public float playerExp;

        //Current Player' Equipped Weapon
        public WEAPON_COLLECTION playerWeapon;

        //All Player' Available Weapons and their UI slot number
        public Dictionary<int, WEAPON_COLLECTION> allPlayerWeapons;

        //Level index
        public int levelIndex;
    }

}

