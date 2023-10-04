using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.VFX;

public class BackPanel : MonoBehaviour
{


    [SerializeField] private List<GameObject> panels;

    [SerializeField] private GameObject panel;

    private static int num_move = 0;

    private float tx = 0;
    private float vx = 0;
    private float sx = 0;

    private float ty = 0;
    private float vy = 0;
    private float sy = 0;

    // Start is called before the first frame update
    void Start()
    {
       // int move_count = 1;

        num_move = 1;

        if (num_move==0)
        {
            tx = 1.0f;
            vx = 1.0f;
            sx = 0.0f;

            ty = 1.0f;
            vy = 0.9f;
            sy = 0.0f;
        }else
        if (num_move == 1)
        {
            tx = 0.5f;
            vx = 1.0f;
            sx = 0.0f;

            ty = 1.0f;
            vy = 0.7f;
            sy = 3.14f / 2.0f;
        }


    }

    // Update is called once per frame
   // private void FixedUpdate()
    void Update()
    {
        Vector3 v = new Vector3(0,0,0);

        v.x = math.sin(Time.time * tx + sx) * vx;
        v.y = math.cos(Time.time * ty + sy) * vy;

        transform.position = v;

    }
}
