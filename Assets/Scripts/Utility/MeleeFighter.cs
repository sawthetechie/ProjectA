using System;
using System.Collections;
using System.Collections.Generic;
using Enemy;
using Player;
using UnityEngine;

namespace Utility
{
    public enum ActionStates {Idle, Warmup, Impact, CoolDown}
    public class MeleeFighter :MonoBehaviour
    {
        public bool InAction { get; private set; } = false;
        private bool _doCombo = false;
        private int _comboCount = 0;
        Animator _animator;
        public List<AttackData> attacks = new List<AttackData>();
        
        public ActionStates actionState;
        [SerializeField] private float attackDistance;
        [SerializeField] private float attackingRotationSpeed;
        private ColliderControl _colControl;
        
        public bool InCounter{get; private set;} = false;

        void Start()
        {
            _animator = GetComponent<Animator>();
            _colControl = GetComponent<ColliderControl>();
            if (_colControl != null)
            {
                _colControl.DisableColliders();
            }
        }
        
        public void TryToAttack()
        {
            
            if (!InAction)
            {
                StartCoroutine(Attack());
            } 
            else if (actionState == ActionStates.Impact || actionState == ActionStates.CoolDown)
            {
                _doCombo = true;
            }
        }
        

        IEnumerator Attack()
        {
            InAction = true;
            actionState = ActionStates.Warmup;
 
            _animator.CrossFade(attacks[_comboCount].animName, 0.1f);

            yield return null; 
            
            var animState = _animator.GetNextAnimatorStateInfo(1);
            
            float Timer = 0f;

            while (Timer <= animState.length)
            {
                Timer += Time.deltaTime;
                float normalizedTime = Timer / animState.length;

                if (actionState == ActionStates.Warmup)
                {
                    if (InCounter)
                    {
                        break;
                    }  
                    if (normalizedTime >= attacks[_comboCount].impactStartTime)
                    {
                        actionState = ActionStates.Impact;
                        _colControl.EnableColliders(attacks[_comboCount].hitbox);
                    }
                }
                else if (actionState == ActionStates.Impact)
                {
                    if (normalizedTime >= attacks[_comboCount].impactEndTime)
                    {
                        actionState = ActionStates.CoolDown;
                        _colControl.DisableColliders();
                    }
                }
                else if (actionState == ActionStates.CoolDown)
                {
                    if (_doCombo)
                    {
                        _doCombo = false;
                        _comboCount = (_comboCount + 1) % attacks.Count;

                        StartCoroutine(Attack());
                        yield break;
                    }
                }
                
                yield return null;
            }
            
            actionState = ActionStates.Idle;
            _comboCount = 0;
            InAction = false;
        }
        

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Hitbox") && !InAction)
            {
                StartCoroutine(PlayHitReaction());
            }
        }

        IEnumerator PlayHitReaction()
        {
            InAction = true;
            
            _animator.CrossFade("Side Hit", 0.2f);
            yield return null;

            var animState = _animator.GetNextAnimatorStateInfo(1);

            yield return new WaitForSeconds(animState.length * 0.4f);
            
            InAction = false;
        }
        
        public IEnumerator PlayCounterAttack(EnemyController enemy)
        {
            InAction = true;
            InCounter =  true;
            enemy.meleeFighter.InCounter = true;

            var dispVec = enemy.transform.position - transform.position;
            transform.rotation = RotateEntitytoTarget(enemy.transform);
            enemy.transform.rotation = RotateEntitytoTarget(enemy.transform, true); 
           
            var targetPosition = enemy.transform.position - dispVec.normalized * attackDistance;
            
            _animator.CrossFade("UpperCut", 0.2f);
            enemy.animator.CrossFade("Knocked Out",0.2f);
            enemy.ChangeState(EnemyStates.Dead);
            yield return null;

            var animState = _animator.GetNextAnimatorStateInfo(1);

            float Timer = 0f;

            while (Timer <= animState.length)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, 1f * Time.deltaTime);
                
                yield return null;
                Timer += Time.deltaTime;
            }
            
            
            enemy.meleeFighter.InCounter = false;
            InCounter = false;
            InAction = false;
        }

        public Quaternion RotateEntitytoTarget(Transform target, bool reverse = false)
        {
            var dispVec = target.position - transform.position;
            dispVec.y = 0;
            Quaternion rotateQuat = Quaternion.RotateTowards(Quaternion.LookRotation(dispVec), transform.rotation, attackingRotationSpeed * Time.deltaTime);
            if (reverse)
            {
                rotateQuat = Quaternion.RotateTowards(Quaternion.LookRotation(- dispVec), transform.rotation, attackingRotationSpeed * Time.deltaTime);
            }
           
            return rotateQuat;
        }
        
        private void OnAnimatorMove()
        {
            if (!InCounter && !InAction)
            {
                transform.position += _animator.deltaPosition;
            }
            
            transform.rotation *= _animator.deltaRotation;
        }
        
        public List<AttackData> Attacks =>  attacks;
        public bool IsCounterAble => actionState == ActionStates.Warmup && _comboCount == 0;
        
    }
}