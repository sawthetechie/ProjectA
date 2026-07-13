using UnityEngine;

public class Spinner : MonoBehaviour
{
    [SerializeField] private int spinSpeed;
    [SerializeField] private SPINAXIS spinaxis;

    // Update is called once per frame
    void Update()
    {
        if (spinaxis == SPINAXIS.Horizontal)
        {
            gameObject.transform.Rotate(0, spinSpeed * Time.deltaTime, 0);
        }else if (spinaxis == SPINAXIS.Vertical)
        {
            gameObject.transform.Rotate(0, 0,spinSpeed * Time.deltaTime);
        }
    }

    enum SPINAXIS
    {
        Horizontal,
        Vertical
    }
}
