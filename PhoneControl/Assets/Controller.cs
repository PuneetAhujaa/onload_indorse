using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;

public class Controller : MonoBehaviour
{
    string url = "http://192.168.43.189:8080/sensors.json";
    string json = "";
    Vector3 gvt, gyro;
    int x,y;
    float radius;
    Quaternion quat;
    public int max_x = 555,max_y = 382,max_r = 160,min_r = 10;
    public float m = 0.1f;
    //public multiplier = 10;
    // Start is called before the first frame update
    string path = "Assets/Text/posfile.txt";
    void Start()
    {
        //StartCoroutine(GetText());
         
        // GetPosition();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        StartCoroutine(GetText());
        RotateByQuat();
      //GetPosition();
    //  Debug.Log(x);
    //  Debug.Log(y);
    //  Debug.Log(radius);
    //  MoveByPos();
    }
    
    void GetPosition()
    {
        StreamReader reader = new StreamReader(path); 
        string pos = reader.ReadToEnd();
        reader.Close();
        if(pos[0]=='0')
            return;
        pos = pos.Substring(2,pos.Length-3);
        string[] subStrings = pos.Split(',');
        x = int.Parse(subStrings[0]);
        y = int.Parse(subStrings[1]);
        radius = float.Parse(subStrings[2]);
    }
    
    void MoveByPos()
    {
        transform.position = new Vector3(m*(x - max_x/2f),m*(y - max_y/2f),m*(radius - (max_r - min_r)/2f));
    }
    
    void RotateByGvt()
    {
        Quaternion target = Quaternion.Euler(-gvt.y * 9, transform.localRotation.y, -gvt.z * 9 * getSign(gvt.x));
        transform.localRotation = target;
    }

    void RotateByGyro()
    {
        transform.RotateAround(transform.position, Vector3.right, 180 * gyro.z * Time.deltaTime / 3.1415f);
        transform.RotateAround(transform.position, Vector3.back, 180 * gyro.y * Time.deltaTime / 3.1415f);
        transform.RotateAround(transform.position, Vector3.down, 180 * gyro.x * Time.deltaTime / 3.1415f);
    }

    void RotateByQuat()
    {
        transform.localRotation = quat;
    }

    IEnumerator GetText()
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            json = www.downloadHandler.text;
        }
        int n = json.Length - 1,i;
        quat = GetQuaternion(json, n);
        //n -= 3000;
        //gvt = getValues(json, n,true);
        //n -= 3000;
        //gyro = getValues(json, n, true);
    }

    Quaternion GetQuaternion(string json,int n)
    {
        int i;
        for (i = n - 5; i > 0; i--)
        {
            if (json[i] == '[')
                break;
        }
        string val = json.Substring(i + 1, n - 5 - i);
        string[] subStrings = val.Split(',');
        Quaternion q = new Quaternion(float.Parse(subStrings[3]), float.Parse(subStrings[2]),-float.Parse(subStrings[1]),-float.Parse(subStrings[0]));  //Json returns quat (x,y,z,w)  , Unity : (w,x,y,z)
        return q;
    }

    Vector3 getValues(string json,int n,bool findParanthesis)
    {
        int i;
        if(findParanthesis)
        {
            while(n-- >=0)
            {
                if (json[n] == '}')
                {
                    n++;
                    break;
                }
            }
        }
        for (i = n - 5; i > 0; i--)
        {
            if (json[i] == '[')
                break;
        }
        string val = json.Substring(i + 1, n - 5 - i);
        string[] subStrings = val.Split(',');       
        Vector3 values = new Vector3(float.Parse(subStrings[0]), float.Parse(subStrings[1]), float.Parse(subStrings[2]));
        return values;
    }

    int getSign(float f)
    {
        if (f < 0) return -1;
        return 1;
    }
}
