using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kinematic : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void OnCollisionEnter (Collision collision)
    {
        if(collision.collider.name == "Cylinder_001")
        {
            this.GetComponent<Rigidbody>().isKinematic = false;
        }
    }
}
