using UnityEngine;

namespace FantasyTown.Weapons
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/New Weapon", order = 2)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] private WEAPON_COLLECTION weaponName;
        [SerializeField] private AnimatorOverrideController controller;
        [SerializeField] private float weaponRange;
        [SerializeField] private float weaponDamage;
        [SerializeField] private Sprite sprite;
        [SerializeField] private AudioClip weaponHitSound;
        [SerializeField] private AudioClip weaponLaunchSound;

        public AudioClip GetWeaponHitSound()
        {
            return weaponHitSound;
        }
        public AudioClip GetWaponLaunchSound()
        {
            return weaponLaunchSound;
        }

        public Sprite Sprite { get => sprite; }

        public float GetWeaponRange()
        {
            return weaponRange;
        }

        public float GetWeaponDamage()
        {
            return weaponDamage;
        }

        public AnimatorOverrideController GetAnimatorOverrideController()
        {
            return controller;
        }

        public WEAPON_COLLECTION GetWeaponName()
        {
            return weaponName;
        }
    }

}
