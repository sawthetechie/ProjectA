using System;
using UnityEngine;

public class BlockEndTrigger : MonoBehaviour
{
    public event Action OnBlockEnd;

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            OnBlockEnd?.Invoke();
        }
        
        GameObject.Destroy(gameObject);
    }
}
