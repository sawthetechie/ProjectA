using System;
using UnityEngine;

public class Blockade : MonoBehaviour
{
    [SerializeField] private Transform endAlignObject;
    [SerializeField] private BlockEndTrigger endTrigger;
    void OnEnd()
    {
        transform.position = endAlignObject.position;
        transform.GetComponent<Shaker>().isStop =  true;
    }

    private void OnEnable()
    {
        endTrigger.OnBlockEnd += OnEnd;
    }

    private void OnDisable()
    {
        endTrigger.OnBlockEnd -= OnEnd;
    }
}
