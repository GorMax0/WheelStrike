using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      if(Input.GetMouseButton(0))
        {
            transform.Translate(Vector3.forward*Time.deltaTime);
            transform.Rotate(Vector3.left*Time.deltaTime);
        }
    }
}
