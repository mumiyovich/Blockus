using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR;

//https://discussions.unity.com/t/change-surface-type-with-lwrp/221034/3

public class BlockNode : MonoBehaviour
{
    public int type = 0;

    public bool collected = false;

    [HideInInspector] public GameManager _gm;


    public GameObject down_panel;
    public GameObject meshes;

    [SerializeField] private float took_velocity;
    [SerializeField] private float took_time;


    [SerializeField] public GameObject ParticlePriz;


    [SerializeField] private AudioSource toock;

    [SerializeField] private float priz_speed;
    private int cur_priz = 0;

    private float rotate_seed=0;

    public Rigidbody rb;

    private float time_toock = 0;
    private float time_toock_k = 0.0f;

    private Material currentMat;

    private float scale = 0;

    private void OnDestroy()
    {

        if (_gm.blocks_in_down.ContainsKey(gameObject) == true)
        {
            _gm.blocks_in_down.Remove(gameObject);
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3(0, 0, 0);

        scale = 0;

        rotate_seed = ((float)UnityEngine.Random.Range(0, 1001))/1001.0f * 6.28f;

        rb = GetComponent<Rigidbody>();


     //   currentMat = meshes.GetComponent<Renderer>().material;

        //ChangeAlpha(0.1f);
    }

   /*
    void ChangeAlpha(float alphaVal)
    {

        Renderer renderer = meshes.transform.GetChild(0).gameObject.GetComponent<Renderer>();

        
        

       // Renderer renderer = GameObject.Find("back_02").GetComponent<Renderer>();


        Color oldColor = renderer.material.color; //currentMat.color;
        Color newColor = new Color(oldColor.r, oldColor.g, oldColor.b, alphaVal);
        //currentMat.SetColor("_Color", newColor);

        //meshes.GetComponent<Renderer>().material.shader = Shader.Find("Transparent/Diffuse");

        renderer.material.color = newColor;

        renderer.material.SetFloat("_Surface", (float)1);
        renderer.material.SetFloat("_Blend", (float)0);
        renderer.material.SetOverrideTag("RenderType", "Transparent");
        renderer.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        renderer.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        renderer.material.SetInt("_ZWrite", 0);
        renderer.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        renderer.material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
    //    renderer.material.SetShaderPassEnabled("ShadowCaster", false);
        



    }
   */


    float priz_cur_time = 0;
    // Update is called once per frame
    void Update()
    {



        float dist_to_panel = transform.position.y - down_panel.transform.position.y;
        if(dist_to_panel < 1.3f)
        {
            if (_gm.blocks_in_down.ContainsKey(gameObject) == false)
            {
                _gm.blocks_in_down.Add(gameObject, this);
            }
        }
        else
        {
            if (_gm.blocks_in_down.ContainsKey(gameObject) == true)
            {
                _gm.blocks_in_down.Remove(gameObject);
            }
        }


        if (scale!=1)
        {
            scale += Time.deltaTime * 1.5f;// 3.0f;

            scale = math.min(scale, 1.0f);

            float s = 1.0f - (1.0f - scale) * (1.0f - scale) * (1.0f - scale);
            transform.localScale = new Vector3(s, s, s);

        }


        time_toock += Time.deltaTime * time_toock_k;

        if(time_toock >= took_time)
        {
            time_toock = 0;
            time_toock_k = 0;
        }


        if (type == -1)
        {
            priz_cur_time+=Time.deltaTime;
            if (priz_cur_time >= priz_speed)
            {
                priz_cur_time = 0;
                DrawPriz();
            }
        }



        Vector3 axis = new Vector3(0.5f, 1, 0).normalized;
        float angle = Time.time * 30.0f;

        angle = math.sin(Time.time * 0.6f + rotate_seed) * 90.0f;

        Quaternion q = Quaternion.AngleAxis(angle, axis);

        meshes.transform.rotation = q;

    }

    private void DrawPriz()
    {
        foreach (Transform child in meshes.transform)
        {
            child.gameObject.SetActive(false);
        }
        meshes.transform.GetChild(cur_priz).gameObject.SetActive(true);
        cur_priz++;
        if(cur_priz == meshes.transform.childCount)
        {
            cur_priz = 0;
        }

    }

    private void OnCollisionEnter(Collision other)//collision)

    //private void OnTriggerEnter(Collider other)
    {
        // Debug.Log("zzzz "+other);

        if (collected)
            return;



        BlockNode other_b = other.gameObject.GetComponent<BlockNode>();

        if (other_b == null)
        {
            //Debug.Log(rb.velocity.y);
            if (rb.velocity.y < took_velocity && time_toock == 0.0f)
            {
                toock.Play();
                time_toock_k = 1.0f;
            }

            return;
        }

        if (transform.position.x != other_b.transform.position.x)
        {
            return;
        }

        //Debug.Log(gameObject.name);
        //Debug.Log(other.gameObject.name);

        GameManager gm = FindFirstObjectByType<GameManager>();

        if (type == other_b.type || type == -1 || other_b.type==-1)
        {
            gm.BlocksOk(transform, other_b.transform, true);
        }
        else
        {
            gm.BlocksOk(transform, other_b.transform, false);
        }

        collected = true;
        other_b.collected = true;

        Destroy(gameObject);
        Destroy(other_b.gameObject);

    }
}
