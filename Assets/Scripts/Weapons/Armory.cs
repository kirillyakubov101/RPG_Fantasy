using UnityEngine;

namespace FantasyTown.Weapons
{
    public class Armory : MonoBehaviour
    {
        [SerializeField] private WeaponWrapper[] allWeapons;
       

        private static Armory instance;

        public static Armory Instance { get => instance; }

        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        public WeaponWrapper GetWeaponFromGlobalArmory(WEAPON_COLLECTION weaponName)
        {
            foreach (var ele in allWeapons)
            {
                //ele.gameObject.SetActive(false);
                if (ele.WeaponStats.GetWeaponName() == weaponName)
                {
                    return ele;
                }
            }
            print("error armory");
            return null;
        }


    }
}

