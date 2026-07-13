using System.Collections.Generic;
using System.Linq;
using Enemy;
using UnityEngine;

namespace Player
{
    public class EnemyManager: MonoBehaviour
    {
        public static EnemyManager Instance;
        [SerializeField] private float nonAttackTimer;
        [SerializeField] private Vector2 nonAttackTimerRange =  new Vector2(1f, 4f);
        [SerializeField] private List<EnemyController> enemiesInRange = new List<EnemyController>();
        private CombatController player;
        private void Start()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            enemiesInRange = new List<EnemyController>();
            player = CombatController.Instance;
        }

        public void AddEnemy(EnemyController enemy)
        {
            if (!enemiesInRange.Contains(enemy))
            {
                enemiesInRange.Add(enemy);
            }
            
        }

        public void RemoveEnemy(EnemyController enemy)
        {
            if (enemiesInRange.Contains(enemy))
            {
                enemiesInRange.Remove(enemy);
                var targetEnemy = CombatController.Instance.TargetEnemy;
                if (targetEnemy == enemy)
                {
                    enemy.shaderController.HighlightMaterial(true);
                    targetEnemy = GetClosestEnemy();
                    targetEnemy?.shaderController?.HighlightMaterial(false);
                }
            }
            
        }

        float timer = 0f;
        void Update()
        {
            if (enemiesInRange.Count == 0) return;
            if (!enemiesInRange.Any(e => e.IsInState(EnemyStates.Attack)))
            {
                if(nonAttackTimer > 0)
                {
                    nonAttackTimer -= Time.deltaTime;
                }

                if (nonAttackTimer <= 0)
                {
                    EnemyController attackingEnemy = SelectEnemyForAttack();
                    if (attackingEnemy != null)
                    {
                        attackingEnemy.ChangeState(EnemyStates.Attack);
                        nonAttackTimer = Random.Range(nonAttackTimerRange.x, nonAttackTimerRange.y);
                    }
                }
            }

            if (timer > 0.25f)
            {
                timer = 0f;
                var closestEnemy = GetClosestEnemy();
                if (closestEnemy != null && closestEnemy != player.targetEnemy)
                {
                    var prevEnemy = player.targetEnemy;
                    player.targetEnemy = closestEnemy;
                    
                    player.targetEnemy?.shaderController?.HighlightMaterial(false);
                    prevEnemy?.shaderController?.HighlightMaterial(true);
                }
            }
            timer +=  Time.deltaTime;
        }

        public EnemyController SelectEnemyForAttack()
        {
           return enemiesInRange.OrderByDescending(e => e.combatStateTimer).FirstOrDefault(e => e.target != null); 
        }

        public EnemyController GetAttackingEnemy()
        {
            return enemiesInRange.FirstOrDefault(e => e.IsInState(EnemyStates.Attack));
        }
        
        public EnemyController GetClosestEnemy()
        {
            var targetingDirection = player.GetTargetingDirection();
            float closeDistance = Mathf.Infinity;
            EnemyController closestEnemy = null;
           
            foreach (EnemyController enemy in enemiesInRange)
            {
                if (enemy.IsInState(EnemyStates.Dead)) return null;
                var vectToEnemy = enemy.transform.position - player.transform.position;
                vectToEnemy.y = 0;

                float angle = Vector3.Angle(targetingDirection, vectToEnemy);
                float distance = vectToEnemy.magnitude * Mathf.Sin(angle * Mathf.Deg2Rad);

                if (distance < closeDistance)
                {
                    closestEnemy = enemy;
                    closeDistance = distance;
                }
            }

            return closestEnemy;
        }
    }
}