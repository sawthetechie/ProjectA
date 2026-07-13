using System;
using UnityEngine;
using Utility;

namespace Enemy
{
    public class RetreatAfterAttackState : State<EnemyController>
    {
        EnemyController _owner;
        [SerializeField] private float retreatDistance = 3f;
        [SerializeField] private float backwardSpeed = 1f;
        [SerializeField] private float rotationSpeed = 720f;
        public override void Enter(EnemyController owner)
        {
            _owner = owner;
            _owner.navMeshAgent.stoppingDistance = retreatDistance;
        }

        public override void Execute()
        {
            if (Vector3.Distance(_owner.transform.position, _owner.target.transform.position) >= retreatDistance)
            {
                _owner.ChangeState(EnemyStates.Combat);
                return;
            }
            var vecToTarget = _owner.target.transform.position - _owner.transform.position;
            _owner.navMeshAgent.Move(-vecToTarget.normalized * backwardSpeed * Time.deltaTime);
            vecToTarget.y = 0f;
            
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(vecToTarget), rotationSpeed * Time.deltaTime);
        }

        public override void Exit()
        {
            
        }
    }
}