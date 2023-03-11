using FantasyTown.Projectiles;
using System;
using UnityEngine;
using FantasyTown.Equiper;

namespace FantasyTown.Weapons
{
    public enum WEAPON_COLLECTION
    {
        NONE,
        FISTS,
        SWORD,
        BOW
    }

    public class Fighter : MonoBehaviour
    {
        [SerializeField] private WeaponWrapper currentWeapon;
        [SerializeField] private Projectile currentProjectile;
        [SerializeField] private Transform shootPoint;
       
        public Projectile CurrentProjectile { get => currentProjectile;  }
        public Transform ShootPoint { get => shootPoint;  }

        public event Action OnWeaponEquip;

        private void Start()
        {
           EquipWeapon(currentWeapon);
        }

        public WeaponWrapper GetCurrentWeapon()
        {
            return currentWeapon;
        }

        private void OnEnable()
        {
            EquipeHandler.OnEquipWeapon += EquipWeapon;
            EquipeHandler.OnUnEquipWeapon += UnEquipWeapon;
        }

        private void OnDestroy()
        {
            EquipeHandler.OnEquipWeapon -= EquipWeapon;
            EquipeHandler.OnUnEquipWeapon += UnEquipWeapon;
        }

        private void UnEquipWeapon()
        {
            EquipWeapon(null);
        }

        public void EquipWeapon(WeaponWrapper newWeapon)
        {
            if(newWeapon == null) //default weapon
            {
                if(currentWeapon != null)
                {
                    currentWeapon.gameObject.SetActive(false);
                }
                currentWeapon = PlayerEquipments.Instance.DefaultWeaponFists;
            }
            else
            {
                currentWeapon.gameObject.SetActive(false);
                currentWeapon = newWeapon;
                currentWeapon.gameObject.SetActive(true);
            }
           
            OnWeaponEquip?.Invoke();
        }
    }
}

