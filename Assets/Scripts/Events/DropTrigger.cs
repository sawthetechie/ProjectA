using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class DropTrigger: MonoBehaviour
{
    public static event Action DropAction;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            DropAction?.Invoke();
        } 
        
        GameObject.Destroy(gameObject);
    }
        
}
