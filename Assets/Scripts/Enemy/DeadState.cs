using Player;
using Utility;

namespace Enemy
{
    public class DeadState : State<EnemyController>
    {
        public override void Enter(EnemyController owner)
        {
            owner.visionSensor.enabled = false;
            owner.navMeshAgent.enabled = false;
            owner.characterController.enabled = false;
            EnemyManager.Instance.RemoveEnemy(owner);
        }
    }
}