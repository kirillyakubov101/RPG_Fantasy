using UnityEngine;
using UnityEngine.UI;
using FantasyTown.Input;

namespace FantasyTown.PlayerUI
{
    public class Selector : MonoBehaviour
    {
        public static Selector Instance { get; private set; } = null;
        [field: SerializeField] public ItemSlot Slot { get; set; } = null;
        [field: SerializeField] public Image DraggedItemImage { get; set; } = null;

        private void Awake()
        {
            if (Instance == null) { Instance = this; }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnDisable()
        {
            DraggedItemImage.sprite = null;
           
        }

        private void OnEnable()
        {
            DraggedItemImage.enabled = false;
        }

        private void Update()
        {
            if (DraggedItemImage.sprite == null) { return; }

            DraggedItemImage.transform.position = PlayerInput.Instance.GetMouseUIPosition();
        }

        public void ClearSelector()
        {
            DraggedItemImage.enabled = false;
            Instance.DraggedItemImage.sprite = null;
            Instance.Slot = null;
        }
    }
}

