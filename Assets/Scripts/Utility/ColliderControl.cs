using System;
using System.Collections.Generic;
using Player;
using UnityEngine;

namespace Utility
{
    public class ColliderControl : MonoBehaviour
    {
        [SerializeField] public List<Collider> RFAttackCollider;
        [SerializeField] public List<Collider>  LFAttackCollider;
        [SerializeField] public List<Collider>  RHAttackCollider;
        [SerializeField] public List<Collider>  LHAttackCollider;
        
        public void EnableColliders(HitboxType hitboxType)
        {
            switch (hitboxType)
            {
                case HitboxType.HitboxLeftFoot:
                    foreach (Collider col in LFAttackCollider)
                    {
                        col.enabled = true;
                    }
                    break;
                case HitboxType.HitboxRightFoot:
                    foreach (Collider col in RFAttackCollider)
                    {
                        col.enabled = true;
                    }
                    break;
                case HitboxType.HitboxLeftHand:
                    foreach (Collider col in LHAttackCollider)
                    {
                        col.enabled = true;
                    }
                    break;
                case HitboxType.HitboxRightHand:
                    foreach (Collider col in RHAttackCollider)
                    {
                        col.enabled = true;
                    }
                    break;
                 default:
                    DisableColliders();
                    break;
            }
            
        }

        public void DisableColliders()
        {
            foreach (Collider col in RFAttackCollider)
            {
                col.enabled = false;
            }
            foreach (Collider col in LFAttackCollider)
            {
                col.enabled = false;
            }
            foreach (Collider col in RHAttackCollider)
            {
                col.enabled = false;
            }
            foreach (Collider col in LHAttackCollider)
            {
                col.enabled = false;
            }
            
        }
    }

   
}