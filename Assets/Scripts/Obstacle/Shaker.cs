using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Shaker : MonoBehaviour
{
    [SerializeField] private float shakeSpeed;
    [SerializeField] private float horizontalShakeOffset;
    [SerializeField] private float verticalShakeOffset;
    [SerializeField] private SHAKEDIRECTION shakeDirection;
    [SerializeField] private bool isReversed;
    
    private float initialHorizontalPosition;
    private float  initialVerticalPosition;
    private bool isMaxed;
    
    public bool isStop;

    public enum SHAKEDIRECTION
    {
        Horizontal,
        Vertical,
        HorizontalReverse,
        VerticalReverse,
    }
    
    void Start()
    {
        initialHorizontalPosition = transform.position.y;
        initialVerticalPosition = transform.position.z ;
       
    }
    
    void Update()
    {
        if (isStop) return;
        switch (shakeDirection)
        {
            case SHAKEDIRECTION.Horizontal:
                Shake(transform.position.y, initialHorizontalPosition , horizontalShakeOffset, Vector3.up);
                break;
            case SHAKEDIRECTION.Vertical:
                Shake(transform.position.z, initialVerticalPosition , verticalShakeOffset, Vector3.forward);
                break;
            case SHAKEDIRECTION.HorizontalReverse:
                Shake(transform.position.z, initialVerticalPosition , verticalShakeOffset, Vector3.down);
                break;
            case SHAKEDIRECTION.VerticalReverse:
                Shake(transform.position.z, initialVerticalPosition ,verticalShakeOffset, Vector3.back);
                break;
        }
        
    }

    void Shake(float value, float initialValue, float offset, Vector3 direction)
    {
        var  maxSwing = initialValue + offset;
        var minSwing = initialValue - offset;

        SetIsMaxed(value, maxSwing, minSwing);
        
        
        if (isMaxed)
        {
            transform.Translate(- direction * shakeSpeed * Time.deltaTime);
            
        }
        else if (!isMaxed)
        {
            transform.Translate(direction * shakeSpeed * Time.deltaTime);
        }

        
    }

    void SetIsMaxed(float value, float maxSwing, float minSwing )
    {
        if (value > maxSwing)
        {
            isMaxed = true;
            if(isReversed) isMaxed = false;
        }
        else if (value < minSwing)
        {
            isMaxed = false;
            if(isReversed) isMaxed = true;
        }
        
    }
    
}
