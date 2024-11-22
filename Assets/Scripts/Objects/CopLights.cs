using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopLights : MonoBehaviour
{
    
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0f, Time.deltaTime * 200f, 0f));
    }
}
