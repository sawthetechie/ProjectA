using UnityEngine;
using UnityEngine.AI;
using Utility;

namespace Enemy
{
    public enum AICombatPatrolState{Idle, Chase, Circle}
    public class CombatState :  State<EnemyController>
    {
        private EnemyController _owner;
        private AICombatPatrolState _combatPatrolState;

        private float Timer;
        [SerializeField] private float rotationSpeed = 20f;
        private float rotationDir = 1f;


        [SerializeField][Tooltip("min time of the Range, max time of Range")] private Vector2 waitTimerBeforeCircleRange = new Vector2(1f, 3f);
        [SerializeField] [Tooltip("min time of the Range, max time of Range")] private Vector2 durationToCircleRange = new Vector2(3f, 6f);

        [SerializeField] float distanceToStand = 5f;
        [SerializeField] [Tooltip("distance must player move before enemy begins Chasing")]float reactionDistanceThreshold = 0.1f;
        
        public override void Enter(EnemyController owner)
        {
           _owner = owner;
           _owner.navMeshAgent.stoppingDistance = distanceToStand;
           _owner.combatStateTimer = 0f;

           _owner.animator.SetBool("CombatMode", true);
        }
        public override void Execute()
        {
            if (Vector3.Distance(_owner.transform.position, _owner.target.transform.position) > distanceToStand + reactionDistanceThreshold)
            {
                ChangePatrolStatetoChase();
            }
            
            if (_combatPatrolState == AICombatPatrolState.Idle)
            {
                if(Timer <= 0)
                {
                    if (Random.Range(0, 2) == 1)
                    {
                        ChangePatrolStatetoIdle();
                    }
                    else
                    {
                        ChangePatrolStatetoCircle();
                    }
                }
            }
            
            else if (_combatPatrolState == AICombatPatrolState.Chase)
            {
                if (Vector3.Distance(_owner.target.transform.position, _owner.transform.position) <=
                    distanceToStand + 0.03f)
                {
                    ChangePatrolStatetoIdle();
                    return;
                }
                
                _owner.navMeshAgent.SetDestination(_owner.target.transform.position);
            }
            
            else if (_combatPatrolState == AICombatPatrolState.Circle)
            {
                if (Timer <= 0)
                {
                    ChangePatrolStatetoIdle();
                    return;
                }
                
                Vector3 vecToTarget = _owner.transform.position - _owner.target.transform.position;
                Vector3 rotationPos =
                    Quaternion.Euler(0, rotationSpeed * rotationDir * Time.deltaTime, 0) * vecToTarget;
                
                _owner.navMeshAgent.Move(rotationPos - vecToTarget);
                transform.rotation = Quaternion.LookRotation(-rotationPos);

            }
            if(Timer > 0) Timer -= Time.deltaTime;
            _owner.combatStateTimer += Time.deltaTime;
        }

        public void ChangePatrolStatetoIdle()
        {
            _combatPatrolState = AICombatPatrolState.Idle;
            Timer = Random.Range(waitTimerBeforeCircleRange.x , waitTimerBeforeCircleRange.y);
        }

        public void ChangePatrolStatetoChase()
        {
            _combatPatrolState = AICombatPatrolState.Chase;
        }

        public void ChangePatrolStatetoCircle()
        {
            _combatPatrolState = AICombatPatrolState.Circle;
            _owner.navMeshAgent.ResetPath();
            Timer = Random.Range(durationToCircleRange.x  , durationToCircleRange.y);

            rotationDir = Random.Range(0, 2) == 1 ? 1 : -1;
        }
        
        public override void Exit()
        {
            _owner.combatStateTimer= 0f;
            
        }
    }
}