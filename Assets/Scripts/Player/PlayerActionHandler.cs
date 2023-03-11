using UnityEngine;
using FantasyTown.Input;
using FantasyTown.States;
using FantasyTown.Inventory;
using FantasyTown.Stats;
using FantasyTown.Weapons;
using System;

public class PlayerActionHandler : MonoBehaviour
{
    [SerializeField] private LayerMask m_Mask = new LayerMask();
    [SerializeField] private Inventory m_inventory;
    [SerializeField] private Fighter m_fighter;

    private static PlayerActionHandler instance = null;

    private PlayerInput m_input;
    private Camera m_mainCamera;
    private PlayerStateMachine m_stateMachine;
    private Vector3 actionPosition = Vector3.zero;
    private RaycastHit[] raycastHits;
    private CombatTarget m_target;
    private Item m_item;

    public event Action<Vector3> OnMoveClick;

    public Vector3 ActionPosition { get => actionPosition; }
    public CombatTarget Target { get => m_target; set => m_target = value; }
    public Fighter Fighter { get => m_fighter;  }

    private void Awake()
    {
        if (instance != null) { Destroy(this); return; }
        instance = this;

        m_input = GetComponent<PlayerInput>();
        m_mainCamera = Camera.main;
        m_stateMachine= GetComponent<PlayerStateMachine>();
    }

    private void OnEnable()
    {
        m_input.OnMouseRightClickEvent += RayCastRightMouseClick;
        m_fighter.OnWeaponEquip += UpdateAnimator;
    }

    private void OnDestroy()
    {
        m_input.OnMouseRightClickEvent -= RayCastRightMouseClick;
        m_fighter.OnWeaponEquip -= UpdateAnimator;
    }

    private void RayCastRightMouseClick()
    {
        m_target = null;
        Vector2 mousePos = m_input.GetMousePos();
        Ray ray = m_mainCamera.ScreenPointToRay(mousePos);
        raycastHits = null;
        raycastHits = Physics.RaycastAll(ray, Mathf.Infinity, m_Mask);

        if(raycastHits.Length == 0) { return; }

        actionPosition = raycastHits[0].point; //get the first position of the hits

        //TODO: USE PREDICATE TO CHECK ENEMIES -> COLLECTABLES -> GATHER -> QUEST -> WALKABLE
        foreach (var ele in raycastHits)
        {
            //If an Enemy
            if(ele.transform.TryGetComponent(out CombatTarget combatTarget))
            {
                m_target = combatTarget;

                if (IsEnemyInRange(combatTarget.transform.position))
                {
                    m_stateMachine.SwitchState(State.StateName.ATTACK);
                    return;
                }
                else
                {
                    m_stateMachine.SwitchState(State.StateName.CHASE);
                    return;
                }
            }

            else if(ele.transform.TryGetComponent(out Item item))
            {
                m_item = item;
                m_stateMachine.SwitchState(State.StateName.COLLECT);
                return;
            }
        }

        m_stateMachine.SwitchState(State.StateName.NORMAL);
        OnMoveClick?.Invoke(actionPosition);


    }

    public static PlayerActionHandler GetInstance()
    {
        return instance;
    }

    public bool IsEnemyInRange(Vector3 enemyPos)
    {
        float range = m_fighter.GetCurrentWeapon().WeaponStats.GetWeaponRange();
        return UtilsHelper.SqrDistance(transform.position,enemyPos) <= range * range;
    }

    public void CollectItem()
    {
        m_inventory.CollectItem(m_item);
        Destroy(m_item.gameObject);
        m_item = null;
    }

    public void UpdateAnimator()
    {
        m_stateMachine.SwitchAnimatorController(m_fighter.GetCurrentWeapon().WeaponStats.GetAnimatorOverrideController());
    }

}
