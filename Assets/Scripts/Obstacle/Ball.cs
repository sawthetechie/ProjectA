using System.Collections;
using UnityEngine;

public class Ball : MonoBehaviour
{
    
    [SerializeField] Transform player;
    [SerializeField] float lifeTime;
    private Vector3 targetDirection;

    private float spawnedTime;
    private Rigidbody rb;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb =  GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // lifeTime -= Time.deltaTime;
        ExternalForce();
        DestroyGameObj();
    }

    void ExternalForce()
    {
        targetDirection = ( player.position - transform.position ).normalized;
        rb.AddForce(targetDirection, ForceMode.Acceleration);
    }
    
    
    void DestroyGameObj()
    {
        if (lifeTime < 0f)
        {
            Destroy(gameObject);
        }
    }
}
