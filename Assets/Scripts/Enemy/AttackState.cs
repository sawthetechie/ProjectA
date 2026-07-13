using System.Collections;
using UnityEngine;
using Utility;

namespace Enemy
{
    public class AttackState :  State<EnemyController>
    {
        EnemyController _owner;
        [SerializeField] float _attackingDistance;
        bool isAttacking;
        public override void Enter(EnemyController owner)
        {
           _owner = owner;
           _owner.navMeshAgent.stoppingDistance = _attackingDistance;
        }
        public override void Execute()
        {
            if (isAttacking || _owner.meleeFighter.InAction)
            {
                return;
            }

            if (_owner.target == null)
            {
                return;
            }
            _owner.navMeshAgent.SetDestination(_owner.target.transform.position);

            if (Vector3.Distance(_owner.transform.position, _owner.target.transform.position) <
                _attackingDistance + 0.03f)
           {
               var GeneratedComboCount = Random.Range(1, _owner.meleeFighter.Attacks.Count);
               StartCoroutine(Attack(GeneratedComboCount));
           }
        }

        IEnumerator Attack(int ComboCount)
        {
            isAttacking = true;
            _owner.animator.applyRootMotion = true;
            _owner.navMeshAgent.updatePosition = false;

            for (int i = 0; i < ComboCount; i++)
            {
                _owner.meleeFighter.TryToAttack();
                yield return new WaitUntil(() => _owner.meleeFighter.actionState == ActionStates.CoolDown);
            }
             
            yield return new WaitUntil(() => _owner.meleeFighter.actionState == ActionStates.Idle);

            isAttacking = false;
            _owner.navMeshAgent.nextPosition = _owner.transform.position;
            _owner.animator.applyRootMotion = false;
            _owner.navMeshAgent.updatePosition = true;

            if (_owner.IsInState(EnemyStates.Attack))
            {
                _owner.ChangeState(EnemyStates.RetreatAfterAttack);
            }
           
        }
        
        public override void Exit()
        {
            _owner.navMeshAgent.ResetPath();
        }
    }
}