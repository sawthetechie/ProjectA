using UnityEngine;

public class ScoreHandler : MonoBehaviour
{
    
    int score;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            score++;
            Debug.Log($"OUCH!");
        }
    }
}
