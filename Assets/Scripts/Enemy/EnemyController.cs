using System.Collections.Generic;
using UnityEditor.Media;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using Utility;

namespace Enemy
{
    public enum EnemyStates{Idle, Combat, Attack, RetreatAfterAttack, Dead}
    public class EnemyController : MonoBehaviour
    {
        public List<MeleeFighter> targetsInRange = new List<MeleeFighter>();
        public MeleeFighter target;
        public float FOV = 180f;
        public float combatStateTimer;
        public NavMeshAgent navMeshAgent {get; private set;}
        public Animator  animator {get; private set;}
        public MeleeFighter meleeFighter {get; private set;}
        public VisionSensor visionSensor;
        public CharacterController characterController;
        public ShaderController shaderController;
        
        private StateMachine<EnemyController> stateMachine;
        
        private Vector3 prevPos;
        public Dictionary<EnemyStates, State<EnemyController>> statesDictionary { get; private set; } 
        
        void Start()
        {
            prevPos = transform.position;
            statesDictionary = new Dictionary<EnemyStates, State<EnemyController>>();
            stateMachine = new StateMachine<EnemyController>(this);
            navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            meleeFighter = GetComponent<MeleeFighter>();
            characterController = GetComponent<CharacterController>();
            shaderController = GetComponent<ShaderController>();
            
            statesDictionary.Add(EnemyStates.Idle, GetComponent<IdleState>());
            statesDictionary.Add(EnemyStates.Combat, GetComponent<CombatState>());
            statesDictionary.Add(EnemyStates.Attack, GetComponent<AttackState>());
            statesDictionary.Add(EnemyStates.RetreatAfterAttack, GetComponent<RetreatAfterAttackState>());
            statesDictionary.Add(EnemyStates.Dead, GetComponent<DeadState>());
            
            ChangeState(EnemyStates.Idle);
        }

        public void ChangeState(EnemyStates requestedState)
        {
            stateMachine?.ChangeState(statesDictionary[requestedState]);
        }

        public bool IsInState(EnemyStates checkState)
        {
            return stateMachine.currentState ==  statesDictionary[checkState];
        }
        

        void Update()
        {
            stateMachine?.ExecuteState();

            var distance = animator.applyRootMotion ? new Vector3(0,0,0) : transform.position - prevPos;
            var velocity = distance / Time.deltaTime;
            
            var forwardSpeed  = Vector3.Dot( velocity, transform.forward);
            animator.SetFloat("ForwardSpeed", forwardSpeed / navMeshAgent.speed, 0.2f, Time.deltaTime);
            
            float Angle = Vector3.SignedAngle(transform.forward, velocity, Vector3.up);
            var sideSpeed = Mathf.Sin(Angle *  Mathf.Deg2Rad);
            
            animator.SetFloat("SideSpeed", sideSpeed, 0.2f, Time.deltaTime);
            

            prevPos = transform.position;
        }
        
    }
}