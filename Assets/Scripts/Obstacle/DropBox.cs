using System;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;


public class DropBox : MonoBehaviour
{
    public DropTrigger dropTrigger;
    private bool isDropped = false;
    private Rigidbody rigidBody;
    
    [SerializeField] private Transform objPosition;
    [SerializeField] private float flyForce;
    
    private void OnEnable()
    {
        DropTrigger.DropAction += DropForce;
    }

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }
    void Update()
    {
        if (!isDropped)
        {
            transform.position = objPosition.position;
        }
    }

    private void OnDisable()
    {
        DropTrigger.DropAction -= DropForce;
    }

    void DropForce()
    {
        isDropped = true;
        rigidBody.AddForce(Vector3.up * flyForce,ForceMode.Impulse);
        rigidBody.useGravity = true;
    }
}