using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class ScorePoint : MonoBehaviour
{
    [SerializeField] private float live;
   private float live_total =0;
    [SerializeField] private float speed;
    [SerializeField] private float a;
    private float s;
    private TextMeshPro tmp;
    public Color color;


    // Start is called before the first frame update
    void Start()
    {
        s = speed;
        tmp = GetComponent<TextMeshPro>();
        tmp.color = color;
    }

    // Update is called once per frame
    void Update()
    {
        

        Vector3 v = transform.position;
        Vector3 sc = transform.localScale;

        sc.x += s * 0.2f * Time.deltaTime;
        sc.y += s * 0.2f * Time.deltaTime;

        v.y += s * Time.deltaTime;
        //v.z -= s*0.5f * Time.deltaTime;

        s += a;

        transform.position = v;
        transform.localScale = sc;

        float alpha = 1.0f - math.min((live_total / live), 1.0f);

        tmp.color = new Color(tmp.color.r, tmp.color.g, tmp.color.b, alpha);

        live_total+= Time.deltaTime;

        if(alpha==0.0f)
        {
            Destroy(gameObject);
        }

    }
}
