using UnityEngine;
using UnityEngine.UIElements;
using Utility;

namespace Enemy
{
    public class IdleState :  State<EnemyController>
    {
        private EnemyController _owner;
        public override void Enter(EnemyController owner)
        {
            _owner = owner;
            _owner.animator.SetBool("CombatMode", false);
        }
        public override void Execute()
        {
            foreach (var target in _owner.targetsInRange)
            {
                var vecToTarget = target.transform.position - _owner.transform.position;
                vecToTarget.y = 0;
                float  angle = Vector3.Angle(transform.forward,vecToTarget);
                if (angle < _owner.FOV / 2)
                {
                    _owner.target = target;
                    _owner.ChangeState(EnemyStates.Combat);
                    break;
                }
            }

        }
        public override void Exit()
        {
            
        }
    }
}