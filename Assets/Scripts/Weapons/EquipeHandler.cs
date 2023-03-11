using UnityEngine;
using System;
using FantasyTown.Weapons;
using FantasyTown.Saving;

namespace FantasyTown.Equiper
{
    public class EquipeHandler : MonoBehaviour,ISaveable
    {
        [SerializeField] private Fighter m_fighter;

        public static event Action OnUnEquipWeapon;
        public static event Action<WeaponWrapper> OnEquipWeapon;
        public static event Action OnLoadWeapon;

        public static void EquipWeapon(WeaponWrapper weapon)
        {
            OnEquipWeapon?.Invoke(weapon);
        }

        public static void UnEquipWeapon()
        {
            OnUnEquipWeapon?.Invoke();
        }

        public void CaptureState()
        {
            SavingWrapper.Instance.Data.playerWeapon = m_fighter.GetCurrentWeapon().WeaponStats.GetWeaponName();
        }

        public void RestoreState()
        {
            WeaponWrapper weapon = Armory.Instance.GetWeaponFromGlobalArmory(SavingWrapper.Instance.Data.playerWeapon);
            EquipeHandler.EquipWeapon(weapon);
            OnLoadWeapon?.Invoke();
        }
    }
}

