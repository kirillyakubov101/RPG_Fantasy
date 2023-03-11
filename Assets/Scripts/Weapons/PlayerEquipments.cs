using FantasyTown.PlayerUI;
using FantasyTown.Saving;
using System.Collections.Generic;
using FantasyTown.Equiper;
using UnityEngine;

namespace FantasyTown.Weapons
{
    public class PlayerEquipments : MonoBehaviour,ISaveable
    {
        [SerializeField] private Fighter m_Fighter;
        [Header("Found/Earned Weapons")]
        [SerializeField] private WeaponWrapper[] allAvailableWeapons = new WeaponWrapper[Max_Available_Weapons];
        [Header("UI")]
        [SerializeField] private GameObject equipmentsCanvasObject; //canvas for the equipments
        [Header("UI for Found/Earned Weapons")]
        [SerializeField] private ItemSlot[] slots = new ItemSlot[Max_Available_Weapons];  //Weapon slots  -0 , 1 ,2 
        [Header("UI Equiped Weapons")]
        [SerializeField] private ItemUI leftHand;    //left hand that holds the weapon
        [SerializeField] private ItemUI rightHand;   //right hand that holds the weapon

        private const int Max_Available_Weapons = 3; //so far, I only have 3 avaialble slots...so far
        private Dictionary<int, WEAPON_COLLECTION> m_weaponsInEquipSlots; //it's for the saving system
        private static PlayerEquipments instance;
        public static PlayerEquipments Instance { get => instance; }

        private void Awake()
        {
            if(instance == null) { instance = this; }
            else
            {
                Destroy(this);
            }
        }

        private void Start()
        {
            UpdateWeaponDisplay();
        }

        private void OnEnable()
        {
            EquipeHandler.OnLoadWeapon += UpdateRightHandUIDisplay;
        }

        private void OnDestroy()
        {
            EquipeHandler.OnLoadWeapon -= UpdateRightHandUIDisplay;
        }

        private void UpdateRightHandUIDisplay()
        {
            //Populate right hand UI
            WeaponWrapper currentEquippedWeapon = m_Fighter.GetCurrentWeapon();
            if (currentEquippedWeapon != null)
            {
                foreach(ItemSlot slot in slots)
                {
                    if(slot.Weapon == currentEquippedWeapon)
                    {
                        rightHand.UpdateItemUI(slot, currentEquippedWeapon.WeaponStats.Sprite);
                        slot.DisableItemSlot();
                        break;
                    }
                }
            }
        }

        private void UpdateWeaponDisplay()
        {
            for (int i = 0; i < Max_Available_Weapons; i++)
            {
                if (!slots[i].IsPopulated && allAvailableWeapons[i] != null)
                {
                    //populate slot
                    slots[i].PopulateItemSlot(allAvailableWeapons[i]);
                }    
            }
        }

        public WeaponWrapper DefaultWeaponFists
        {
            get => Armory.Instance.GetWeaponFromGlobalArmory(WEAPON_COLLECTION.FISTS);
        }

        public void ShowHidePlayerEquipments()
        {
            equipmentsCanvasObject.SetActive(!equipmentsCanvasObject.activeSelf);

            if (equipmentsCanvasObject.activeSelf)
            {
                Selector.Instance.enabled = true;
            }
            else
            {
                Selector.Instance.enabled = false;
            }

            UtilsHelper.AdjustTimeScale(equipmentsCanvasObject.activeSelf);
        }

        public void CaptureState()
        {
            m_weaponsInEquipSlots = new Dictionary<int, WEAPON_COLLECTION>();

            for (int i = 0; i < slots.Length; i++)
            {
                if(slots[i].Weapon != null)
                {
                    m_weaponsInEquipSlots.Add(i, slots[i].Weapon.WeaponStats.GetWeaponName());
                }
                else
                {
                    m_weaponsInEquipSlots.Add(i, WEAPON_COLLECTION.NONE);
                }
            }

            SavingWrapper.Instance.Data.allPlayerWeapons = m_weaponsInEquipSlots;
        }

        public void RestoreState()
        {
            m_weaponsInEquipSlots = SavingWrapper.Instance.Data.allPlayerWeapons;

            for (int i = 0; i < slots.Length; i++)
            {
                //Populate the weapons list
                if (m_weaponsInEquipSlots.ContainsKey(i) && m_weaponsInEquipSlots[i] != WEAPON_COLLECTION.NONE)
                {
                    allAvailableWeapons[i] = Armory.Instance.GetWeaponFromGlobalArmory(m_weaponsInEquipSlots[i]);
                }

                //populate the UI
                if (m_weaponsInEquipSlots.ContainsKey(i) && m_weaponsInEquipSlots[i] != WEAPON_COLLECTION.NONE)
                {
                    WeaponWrapper weaponFromSaving = Armory.Instance.GetWeaponFromGlobalArmory(m_weaponsInEquipSlots[i]);
                    slots[i].PopulateItemSlot(weaponFromSaving);
                }
            }
        }
    }
}

