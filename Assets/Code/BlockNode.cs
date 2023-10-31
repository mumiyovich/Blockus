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

    [SerializeField] private float height_bonus_line;


    public GameObject down_panel;
    public GameObject meshes;

    [SerializeField] private float took_velocity;
    [SerializeField] private float took_time;


    [SerializeField] public GameObject ParticlePriz;


    [SerializeField] private AudioSource toock;

    [SerializeField] private float priz_speed;
    private int cur_priz = 0;



    public Rigidbody rb;

    private float time_toock = 0;
    private float time_toock_k = 0.0f;

    private Material currentMat;

    private float scale = 0;


    private BlockRotateParams blockRotateParams;
    private float rotateSeed = 0;



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

        // rotate param

        rotateSeed = ((float)UnityEngine.Random.Range(0, 1001)) / 1001.0f * 6.28f;

        BlockProps blockProps = GetComponentInChildren<BlockProps>();

        if (blockProps != null)
        {
            blockRotateParams = blockProps.blockData.blockRotateParams;
            blockRotateParams.rotateAxis = blockRotateParams.rotateAxis.normalized;
        }
        else
        {
            blockRotateParams = new BlockRotateParams();
        }


        ////////////////////////
        transform.localScale = new Vector3(0, 0, 0);

        scale = 0;

        

        rb = GetComponent<Rigidbody>();


        //   currentMat = meshes.GetComponent<Renderer>().material;

        //ChangeAlpha(0.1f);
    }

    float priz_cur_time = 0;
    // Update is called once per frame
    void Update()
    {



        float dist_to_panel = transform.position.y - (down_panel.transform.position.y + _gm.down_panel_height);
        if (dist_to_panel < height_bonus_line * _gm.scale_block)
        {
            if (_gm.blocks_in_down.ContainsKey(gameObject) == false)
            {
                bool ok = true;
                foreach (KeyValuePair<GameObject, BlockNode> bl in _gm.blocks_in_down)
                {
                    if (bl.Key.transform.position.x == gameObject.transform.position.x)
                    {
                        ok = false; break;
                    }
                }
                if (ok)
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


        if (scale != 1)
        {
            scale += Time.deltaTime * 1.5f;// 3.0f;

            scale = math.min(scale, 1.0f);

            float s = (1.0f - (1.0f - scale) * (1.0f - scale) * (1.0f - scale)) * _gm.scale_block;
            transform.localScale = new Vector3(s, s, s);

        }


        time_toock += Time.deltaTime * time_toock_k;

        if (time_toock >= took_time)
        {
            time_toock = 0;
            time_toock_k = 0;
        }


        if (type == -1)
        {
            priz_cur_time += Time.deltaTime;
            if (priz_cur_time >= priz_speed)
            {
                priz_cur_time = 0;
                DrawPriz();
            }
        }





        float angle = math.sin(Time.time * blockRotateParams.rotateSpeed + rotateSeed) * blockRotateParams.rotateAngle;
        Quaternion q = Quaternion.AngleAxis(angle, blockRotateParams.rotateAxis);
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
        if (cur_priz == meshes.transform.childCount)
        {
            cur_priz = 0;
        }

    }

    private void OnCollisionEnter(Collision other)//collision)
    {
        // Debug.Log("zzzz "+other);

        if (collected)
            return;



        BlockNode other_b = other.gameObject.GetComponent<BlockNode>();

        if (other_b == null)
        {
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

        if ((type == -2 || other_b.type == -2) && (type != -1 && other_b.type != -1))
        {
            return;
        }


        if (type == other_b.type || type == -1 || other_b.type == -1)
        {
            _gm.BlocksOk(transform, other_b.transform, true);
        }
        else
        {
            _gm.BlocksOk(transform, other_b.transform, false);
        }

        collected = true;
        other_b.collected = true;

        Destroy(gameObject);
        Destroy(other_b.gameObject);

    }
}
