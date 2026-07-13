using UnityEngine;

public class TriggerProjectile : MonoBehaviour
{
    [SerializeField] GameObject projectile;
    [SerializeField] private Transform projectileSpawner;
    [SerializeField] private int spawnCount;
    [SerializeField] private Transform spawnProjectile;
    
    [SerializeField] private Transform wall;
    
    BoxCollider boxCollider;
    

    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            SpawnProjectile();
            boxCollider.enabled = false;
            wall.gameObject.SetActive(true);
            
            wall.position = new Vector3(wall.position.x, wall.position.y + 15f, wall.position.z);
        }
    }

    void SpawnProjectile()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            var obj = Instantiate(projectile, projectileSpawner.position, Quaternion.Euler(0,0,0));
        }
    }
    
}
