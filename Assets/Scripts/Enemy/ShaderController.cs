using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Enemy
{
    public class ShaderController : MonoBehaviour
    {
        [SerializeField] Material originalMaterial;
        [SerializeField] private Material HighlightedMaterial;
        
        [SerializeField] List<SkinnedMeshRenderer> skmeshesRenderer =  new List<SkinnedMeshRenderer>();

        public void HighlightMaterial(bool reverse)
        {
            foreach (SkinnedMeshRenderer skmeshRenderer in skmeshesRenderer)
            {
                skmeshRenderer.material = (reverse) ? originalMaterial : HighlightedMaterial;
            }
        }
    }
}