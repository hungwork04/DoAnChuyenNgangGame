using UnityEngine;

public class MapOneEffection : MonoBehaviour
{
    public Quaternion targetRotation;
    public bool canRotate=false;
    public bool canRotateLeft=false; 
    public float rotationSpeed = 2f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // void Start()
    // {
    //     targetRotation= Quaternion.Euler(0,0,3.28f);
    // }

    // Update is called once per frame
    void Update()
    {
        if(canRotate)
        {
            if(canRotateLeft){
                targetRotation= Quaternion.Euler(0,0,3.28f);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            }
                
            else if(!canRotateLeft){
                targetRotation= Quaternion.Euler(0,0,-3.28f);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            }
        }else {
            targetRotation= Quaternion.Euler(0,0,0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }
}
