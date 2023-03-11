using FantasyTown.Pathing;
using FantasyTown.States;
using FantasyTown.Stats;
using System.Collections;
using UnityEngine;
using FantasyTown.Weapons;

namespace FantasyTown.Entity
{
    public abstract class Enemy : MonoBehaviour
    {
        [SerializeField] private EnemyStateMachine m_stateMachine;
        [Header("Patrol Path")]
        [SerializeField] protected Path m_path;
        [Header("AGRO ZONE")]
        [SerializeField] private float m_agroRadius = 10f;
        [SerializeField] private LayerMask m_layerMask;
        [SerializeField] private Transform m_castingPoint = null;
        [Header("Weapon")]
        [SerializeField] private WeaponWrapper m_weaponWrapper = null;

        private Collider[] _colliders;
        private Player _player;
        private bool _isLookingForPlayer = true;
        private bool _isGrace = true;
        private float _timer = 0f;

        public Path Path { get => m_path; }
        public EnemyStateMachine StateMachine { get => m_stateMachine; }
        public bool IsLookingForPlayer { get => _isLookingForPlayer; set => _isLookingForPlayer = value; }
        public Player Player { get => _player;  }
        public WeaponWrapper WeaponWrapper { get => m_weaponWrapper; }

        private void Start()
        {
            StartCoroutine(GracePeriod());
        }
        //enemies are afk and waiting for the Navagent to resume work
        private IEnumerator GracePeriod()
        {
            yield return new WaitForSeconds(2f);
            _isGrace = false;
        }


        private void FixedUpdate()
        {
            if (!_isLookingForPlayer || _isGrace) { return; }

            SearchForPlayer();
        }

        private void SearchForPlayer()
        {
            _timer += Time.fixedDeltaTime;
            if (_timer >= 1.5f)
            {
                _colliders = Physics.OverlapSphere(m_castingPoint.position, m_agroRadius, m_layerMask);
                if (_colliders.Length > 0)
                {
                    _player = _colliders[0].GetComponent<Player>();
                    _isLookingForPlayer = false;
                    m_stateMachine.SwitchState(State.StateName.CHASE);
                }
                _timer = 0f;
            }
        }
    }

}
