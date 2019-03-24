using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplaceBroken : MonoBehaviour
{
    public float max_v=20;
    public Transform prefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
   void OnCollisionEnter(Collision collision)
   {
        if(collision.relativeVelocity.magnitude > max_v)
        {
            Instantiate(prefab,transform.position - new Vector3(0,2.2f,0),Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(90,0,0)));
            Destroy(gameObject);
        }
   }
}
