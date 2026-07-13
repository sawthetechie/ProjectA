using System;
using Enemy;
using UnityEngine;
using UnityEngine.Serialization;
using Utility;

namespace Player
{
    [RequireComponent(typeof(MeleeFighter))]
    public class CombatController : MonoBehaviour
    {
        public static CombatController Instance;
        MeleeFighter playerFighter;
        public EnemyController targetEnemy;
        Animator _anim;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        public EnemyController TargetEnemy
        {
            get => targetEnemy;
            set
            {
                if (targetEnemy == null)
                {
                    combatMode = false;
                }
            }
        }

        private bool combatMode;

        public bool CombatMode
        {
            get => combatMode;
            set
            {
                combatMode = value;
                if (TargetEnemy == null)
                {
                    combatMode = false;
                }
                _anim.SetBool("CombatMode", combatMode);
            }
        }
        
        
        void Start()
        {
            playerFighter = GetComponent<MeleeFighter>();
            _anim = GetComponent<Animator>();
            InputManager.Instance.OnAttackPressed += PerformAttack;
            InputManager.Instance.OnLockOnPressed += LockOnMode;
        }

        void PerformAttack()
        {
            
            var enemy = EnemyManager.Instance.GetAttackingEnemy();
           
            if (enemy != null && enemy.meleeFighter.IsCounterAble && !playerFighter.InAction)
            {
                
                StartCoroutine(playerFighter.PlayCounterAttack(enemy));
            }
            else
            {
                playerFighter.TryToAttack();
                CombatMode = true;
            }
        }

        public void LockOnMode()
        {
            CombatMode = !CombatMode;
        }

        public Vector3 GetTargetingDirection()
        {
            var dispVec = transform.position - PlayerCameraControls.Instance.GetDirection();
            dispVec.y = 0;
            return dispVec.normalized;
        }
    }
}