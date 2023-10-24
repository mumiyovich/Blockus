using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Unity.Mathematics;
using UnityEngine.SceneManagement;

public class IntroText : MonoBehaviour
{

    private TMP_Text text;

   // [Multiline]
    [TextArea]
    [SerializeField]
    private List<string> lst_text = new List<string>();


    

    private int page = 0;
    private int nCar = 0;
    private bool stop = false;
    private float t = 0;


    [SerializeField] private float sinH;
    [SerializeField] private float sinW;
    [SerializeField] private float sinSpeedX;
    [SerializeField] private float sinSpeedY;
    private float offSetY;

    // Start is called before the first frame update
    void Start()
    {

        offSetY = transform.position.y;

        text = gameObject.GetComponent<TMP_Text>();

        text.text = "";

        

    }

    // Update is called once per frame
    void Update()
    {


        transform.position = new Vector3( 
            math.sin(Time.time * sinSpeedX) * sinW,
            math.sin(Time.time * sinSpeedY) * sinH + offSetY,
            transform.position.z);


        //////////////////////////////////////////////

        //Input.GetKeyUp(KeyCode.Space) || 
        if (Input.GetMouseButtonDown(0))
        {
            /*
            if(((float)(Input.mousePosition.y) / (float)(Screen.height)) <  0.24f )
            {
                StartGame();
            }
            else
            {
                stop = false;
            }
            */

            stop = false;

        }

        if (stop) return;

        t += Time.deltaTime;

        if (t < 0.025f)
            return;

        t = 0;

        if(nCar==0)
        {
            text.text = "";
        }

        string ctrp = lst_text[page];

        text.text += ctrp[nCar];

        nCar++;
        if(nCar >= ctrp.Length )
        {
            stop = true;
            nCar = 0;
            
            page++;
            if(page >= lst_text.Count)
            {
                page = 0;
            }
        }

        



    }


}
