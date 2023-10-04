using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Unity.Mathematics;

public class LightM : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {


        Vector3 axis = new Vector3(0, 1, 0).normalized;
        float angle = Time.time * 30.0f;
        angle = math.sin(Time.time * 0.6f ) * 30.0f;
        Quaternion q = Quaternion.AngleAxis(angle, axis);

        axis = new Vector3(1, 0, 0).normalized;
        Quaternion q2 = Quaternion.AngleAxis(30, axis);


         axis = new Vector3(1, 0, 0).normalized;
        angle = Time.time * 30.0f;
        angle = math.sin(Time.time * 1.6f) * 15.0f;
        Quaternion q3 = Quaternion.AngleAxis(angle, axis);

        q = q * q3 * q2;


        gameObject.transform.rotation = q;

    }
}
