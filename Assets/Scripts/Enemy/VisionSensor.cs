
using System;
using Enemy;
using Player;
using UnityEngine;
using Utility;

namespace Enemy
{ 
    public class VisionSensor : MonoBehaviour
    {
        [SerializeField] private EnemyController enemy;

        private void Awake()
        {
            enemy.visionSensor = this;
        }

        void OnTriggerEnter(Collider other)
        {
            var fighter = other.GetComponent<MeleeFighter>();
            if (fighter != null)
            {
                enemy.targetsInRange.Add(fighter);
                EnemyManager.Instance.AddEnemy(enemy);
            }
        }
        
        void OnTriggerExit(Collider other)
        {
            var fighter = other.GetComponent<MeleeFighter>();
            if (fighter != null)
            {
                enemy.targetsInRange.Remove(fighter);
                EnemyManager.Instance.RemoveEnemy(enemy);
            }
        }
        
    }
}
