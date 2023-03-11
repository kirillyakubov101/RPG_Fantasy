using FantasyTown.Equiper;
using FantasyTown.PlayerUI;
using FantasyTown.Weapons;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour,IPointerUpHandler,IPointerDownHandler
{
    [SerializeField] private WeaponWrapper weapon; //TODO: needs to be assigned by armory | not like this
    [SerializeField] private Image image;

    private bool isPopulated = false; //is there is a weapon in the slot
    private bool isClickable = false; // there can be a weapon, but it is not clickable due to the player using this item
    public bool IsClickable { get => isClickable; set => isClickable = value; }
    public WeaponWrapper Weapon { get => weapon;
        set
        {
            weapon = value;
        }
    }

    public Image Image { get => image; }
    public bool IsPopulated { get => isPopulated; set => isPopulated = value; }


    /// <summary>
    /// This is populated by the PlayerEquipments
    /// </summary>
    public void PopulateItemSlot(WeaponWrapper weapon)
    {
        if(weapon == null) { Debug.LogError("Empty weapon pass"); return; }

        this.isClickable = true;                            //now you can interact with the slot by dragging it
        this.weapon = weapon;                               //Update the current weapon
        this.image.sprite = weapon.WeaponStats.Sprite;      //update the image
        this.isPopulated = true;                            //the itemSlot is not populated
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isClickable || !isPopulated) { return; }
        Selector.Instance.Slot = this;
        Selector.Instance.DraggedItemImage.sprite = weapon.WeaponStats.Sprite;
        Selector.Instance.DraggedItemImage.enabled = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isClickable) { return; }
        ClearSelector();
        if (eventData.pointerEnter == null) { return; }
        if(eventData.pointerEnter.TryGetComponent(out ItemUI itemUI))
        {
            this.DisableItemSlot();
            itemUI.UpdateItemUI(this, weapon.WeaponStats.Sprite);

            //Equip the weapon to the player fighter
            if (itemUI.IsRightHand)
            {
                EquipeHandler.EquipWeapon(weapon);
            }
        }
    }

    /// <summary>
    /// Make this slot availabe for dragging and equipping again
    /// </summary>
    public void EnableItemSlot()
    {
        this.isClickable = true;
        this.image.enabled = true;
    }

    /// <summary>
    /// Make this slot unavailabe for dragging
    /// </summary>
    public void DisableItemSlot()
    {
        this.isClickable = false;
        this.image.enabled = false; //TODO: SHOW DEFAULT IMAGE
    }

    private void ClearSelector()
    {
        Selector.Instance.ClearSelector();
    }
}
