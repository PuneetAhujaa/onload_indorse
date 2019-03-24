using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform t,target;
    public int n=4;
    public float speed=10;
    float timer =0;
    float waittime = 6;
    Vector3 offset = new Vector3(10,0,0);
    Vector3 tempoffset=new Vector3(10,0,0);
    void Start()    {
        SpawnObjects();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
           timer+=Time.deltaTime;
           if(timer > waittime)
           {
                timer=0;
                if(waittime > 3)
                {
                    waittime--;
                }
                
                offset = tempoffset;
                SpawnObjects();
                
           }
           MoveObjects();
    }
    void MoveObjects()
    {
        for(int i=0;i<transform.childCount;i++)
        {
            Transform tr = transform.GetChild(i);
            if (Vector3.Distance(tr.position, target.position) > 10f)
            {
                 
                 float step =  speed * Time.deltaTime; // calculate distance to move
                 tr.position = Vector3.MoveTowards(tr.position, target.position, step);
            }
           
        }
    }
    void SpawnObjects()
    {
        for(int i=0;i<n/2;i++)
        {
            Instantiate(t,transform.position+offset,transform.rotation,transform);
            Instantiate(t,transform.position-offset,transform.rotation,transform);
            offset.x+=20;
            
        }
    }
}
