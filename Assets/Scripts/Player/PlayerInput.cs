using System;
using UnityEngine;
using UnityEngine.InputSystem;
using FantasyTown.Saving;
using FantasyTown.Weapons;

namespace FantasyTown.Input
{
    public class PlayerInput : MonoBehaviour, MasterInput.IPlayerActions
    {
        private static PlayerInput instance;
        private MasterInput m_masterInput;
        private Vector2 m_mousePos;
        private Vector2 m_mouseUI_Pos;

        public static PlayerInput Instance { get => instance;  }

        public event Action OnMouseRightClickEvent;

        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            m_mouseUI_Pos = Mouse.current.position.ReadValue();
        }

        #region Subscribe_To_Input_System
        private void OnEnable()
        {
            m_masterInput = new MasterInput();
            m_masterInput.Player.SetCallbacks(this);
            m_masterInput.Player.Enable();
        }
        private void OnDestroy()
        {
            m_masterInput.Player.Disable();
        }
        #endregion

        //----------MOUSE RIGHT CLICK-----------
        public void OnMouseRightClick(InputAction.CallbackContext context)
        {
            if (!context.performed) { return; }

            m_mousePos = Mouse.current.position.ReadValue();
            OnMouseRightClickEvent?.Invoke();
        }

        public Vector2 GetMousePos()
        {
            return m_mousePos;
        }

        public Vector2 GetMouseUIPosition()
        {
            return m_mouseUI_Pos;
        }

        public void OnSave(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                SavingWrapper.Instance.SaveAll();
            }
            
        }

        public void OnLoad(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                SceneHandler.Instance.LoadGame();
            }
        }

        public void OnPlayerEquipments(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                PlayerEquipments.Instance.ShowHidePlayerEquipments();
            }
        }
    }
}

