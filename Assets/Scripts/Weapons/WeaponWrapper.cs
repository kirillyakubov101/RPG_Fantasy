using UnityEngine;
namespace FantasyTown.Weapons
{
    public class WeaponWrapper : MonoBehaviour
    {
        [SerializeField] private Weapon weaponStats;
        public Weapon WeaponStats { get => weaponStats; }

    }

}
