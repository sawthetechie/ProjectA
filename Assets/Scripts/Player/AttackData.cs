using UnityEngine;

namespace Player
{
    [CreateAssetMenu(menuName = "Combat System/ Create New Attack")]
    public class AttackData : ScriptableObject
    {
        [field: SerializeField] public string animName { get; private set; }
        [field: SerializeField] public float impactStartTime { get; private set; }
        [field: SerializeField] public float impactEndTime { get; private set; }
        
        [field: SerializeField] public HitboxType hitbox { get; private set; }
        
    }
    
    public enum HitboxType {HitboxRightHand, HitboxLeftHand, HitboxRightFoot, HitboxLeftFoot }
}