using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    
   
    // Start is called before the first frame update
 

    public void Rotation()
    {
        
        transform.Rotate(0f, 0f, 100f * Time.deltaTime, Space.Self);


    }
}
