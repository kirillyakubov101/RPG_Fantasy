using FantasyTown.Entity;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace FantasyTown.Stats
{
    public class CombatTarget : MonoBehaviour
    {
        [SerializeField] private UniqueIdentifier uniqueIdentifier;
        [Header("Stats")]
        [SerializeField] private BaseStats baseStats;
        [SerializeField] private float maxHp;
        [SerializeField] private float currentHp;
        [SerializeField] private int currentLevel;

        private BaseStats.Stats m_currentStats;
        private bool isAlive = true;
        private EnemyProperties m_enemyProperties;

        public bool IsAlive { get => isAlive; set => isAlive = value; }
        public EnemyProperties CurrentEnemyProperties { get => m_enemyProperties; set => m_enemyProperties = value; }
        public UniqueIdentifier UniqueIdentifier { get => uniqueIdentifier; }

        private void InitStats()
        {
            m_currentStats = baseStats.GetCurrentStats(currentLevel);
            this.maxHp = m_currentStats.healthpts;
            this.currentHp = this.maxHp;
        }

        [System.Serializable]
        public struct EnemyProperties
        {
            public SerializableVector3 enemyPos;
            public bool isEnemyAlive;

            public EnemyProperties(SerializableVector3 enemyPos, bool isEnemyAlive)
            {
                this.enemyPos = enemyPos;
                this.isEnemyAlive = isEnemyAlive;
            }
        }

        private void Start()
        {
            InitStats();
            InitProperties();
            EnemyDataBase.Instance.PopulateDataBase(this);
        }

        private void InitProperties()
        {
            m_enemyProperties = new EnemyProperties(new SerializableVector3(transform.position), isAlive);

            this.SaveProperties(transform.position);
        }

        public void SaveProperties(Vector3 currentPosition)
        {
            m_enemyProperties.isEnemyAlive = isAlive;
            m_enemyProperties.enemyPos.UpdateVector(currentPosition);
        }

        public void LoadProperties(EnemyProperties newPorperties)
        {
            this.m_enemyProperties = newPorperties;
            this.isAlive = newPorperties.isEnemyAlive;
            StartCoroutine(ProcessLoad());
        }

        private IEnumerator ProcessLoad()
        {
            NavMeshAgent agent = GetComponent<EnemyStateMachine>().Agent;
            agent.enabled = false;
            transform.position = m_enemyProperties.enemyPos.ToVector();
            yield return null;
            agent.enabled = true;

        }

        //Just removes the object upon loading
        public void ManualDeath()
        {
            Destroy(gameObject);
        }

        public void TakeDamage(float damage)
        {
            currentHp = Mathf.Max(0f, currentHp - damage);
            if(currentHp <= 0f)
            {
                this.isAlive = false;
                ProcessDeath();
            }
        }

        private void ProcessDeath()
        {
            this.SaveProperties(transform.position);
            EnemyDataBase.Instance.UpdateDataBase(this);

            //Death script
            GetComponent<Enemy>().StateMachine.SwitchState(States.State.StateName.DEATH);
            
        }
    }
}


