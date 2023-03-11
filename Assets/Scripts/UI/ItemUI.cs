using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using FantasyTown.Equiper;

/// <summary>
/// THE EQUIP SLOTS OF THE PLAYER AKA LEFT RIGHT HANDS AND MORE
/// </summary>
public class ItemUI : MonoBehaviour,IPointerClickHandler
{
    [SerializeField] private Image currentImage = null;
    [SerializeField] private Sprite defaultSprite = null;
    [SerializeField] private bool isRightHand;
    [SerializeField] private ItemSlot referencedItemSlot; //TODO: Remove serielized

    public bool IsRightHand { get => isRightHand; }
    public ItemSlot ReferencedItemSlot { get => referencedItemSlot;  }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(referencedItemSlot == null) { return; }
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            EquipeHandler.UnEquipWeapon();
            currentImage.sprite = defaultSprite;
            referencedItemSlot.EnableItemSlot();
            referencedItemSlot = null;
        }
    }

    public void UpdateItemUI(ItemSlot slot,Sprite sprite)
    {
        if(referencedItemSlot != null)
        {
            referencedItemSlot.EnableItemSlot();
        }
        
        referencedItemSlot = slot;
        currentImage.sprite = sprite;
    }
}
