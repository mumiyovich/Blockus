using System.Collections;
using System.Collections.Generic;
//using System.Drawing;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UIElements;


public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject ParticleSwop;
    [SerializeField] private GameObject ParticleBoom;
    [SerializeField] private GameObject ParticleOk;

    [SerializeField] private GameObject PanelPause;

    [SerializeField] private GameObject down_panel;
    [SerializeField] private GameObject StartSceneX;
    private bool start = true;

    private float _cam_sh=0;
    private float _k_cam = 0.1f;


    [System.Serializable]
    public struct Blocks_level
    {
        public List<GameObject> blocks;
    }
    [SerializeField] private List<Blocks_level> blocks_level;

    [SerializeField] private GameObject block;
    



    [SerializeField] private TextMeshProUGUI tekst_sc;

    [SerializeField] private TextMeshProUGUI tekst_score;

    [SerializeField] private GameObject score_point;




    [SerializeField] private GameObject AllObgects;

    [SerializeField] private float start_y_block;
    [SerializeField] private float first_x_block;
    [SerializeField] private float last_x_block;
    [SerializeField] private int count_x_block;
    [SerializeField] private int leven_num = 0;

    [SerializeField]
    private float min_gravity;

    [SerializeField]
    private float max_gravity;

    [SerializeField] private float _gravity;



    [SerializeField] private int priz_var;

    [SerializeField] private int lives;
    private int score=0;
    private int draw_score = 0;

    private float[] x_lines;

    [SerializeField]
    private float min_time_new_block;
    [SerializeField]
    private float max_time_new_block;
    [SerializeField] private float time_new_block;

    [SerializeField]
    private int max_score;


    [SerializeField] private float score_speed;
    private float score_add = 0;//!

    private float sec_scor_add_end = 1.0f;

    private bool is_paused = false;

    private bool first = true;


    private void Awake()
    {
        Application.targetFrameRate = 500;// 60;
       //QualitySettings.vSyncCount = 0;
    }


    // Start is called before the first frame update
    void Start()
    {
        //https://docs.unity3d.com/ScriptReference/Camera.ScreenToWorldPoint.html

        first = true;

        start = true;
        _cam_sh = 0;
        _k_cam = 0.25f;

        x_lines = new float[count_x_block];

        for(int i = 0; i < count_x_block; i++)
        {
            x_lines[i] = first_x_block + ((last_x_block - first_x_block) / (float)(count_x_block-1)) * (float)i;
        }

        //Physics.gravity = new Vector3(0, -2.0F, 0);
        


        DrawScore();

        /*
        GameObject apl_p = Resources.Load<GameObject>("aple_02");// GameObject.Find("aple_02");
        Debug.Log(apl_p);
        GameObject apl = Instantiate(apl_p) as GameObject;
        apl.transform.parent = apl_p.transform;
        */

        //ÑreateBlock();

        CaclDifficult();

    }

    private void CaclDifficult()
    {

        float k = ((float)(math.min(math.max(score, 0), max_score))) / (float)max_score;

        _gravity = (max_gravity - min_gravity) * k + min_gravity;


        time_new_block = (max_time_new_block - min_time_new_block) * (1.0f-k) + min_time_new_block;

            /*
        max_score

        score

        min_gravity;
        max_gravity;
        gravity

        min_time_new_block;
        max_time_new_block;
        time_new_block;
        */


    Physics.gravity = new Vector3(0, -_gravity, 0);

    }

    // Update is called once per frame
    private float last_time = 0;


    float fps_tim_avr = 0;
    int min_fps = 999999;
    int max_fps = 0;
    void DrFPS()
    {

        int fps = (int)(1.0f / Time.deltaTime + 0.5f);

        if (fps > 800)
            return;

        min_fps = math.min(min_fps, fps);
        max_fps = math.max(max_fps, fps);



        fps_tim_avr += Time.deltaTime;

        if (fps_tim_avr >= 5.0f)
        {
            fps_tim_avr = 0;

            min_fps = 999999;
            max_fps = 0;

        }

        tekst_sc.text = "FPS: "+fps.ToString()+ " MIN: "+ min_fps + " MAX: " + max_fps;

    }

    void Update()
    {
        

        if (is_paused)
            return;


        DrFPS();






        if (start)
        {
            CameraColebrate();
            PosDownPanel();
        }

        DrawScore();


        last_time += Time.deltaTime;

        if (last_time >= time_new_block)
        {
            last_time = 0;

            ÑreateBlock();

        }

        if(Input.GetMouseButtonDown(0))
        {
            CheckTap();
        }

        


    }

    void PosDownPanel()
    {
       
        StartSceneX.transform.position = new Vector3(StartSceneX.transform.position.x, Camera.main.transform.position.y, StartSceneX.transform.position.z);
        Vector3 v = new Vector3(0, 0, 0);
        v.z = -Camera.main.transform.position.z;
        v.x = (float)(Camera.main.pixelWidth) * 0.5f;//(float)(Screen.height) * 0.5f;
        v = Camera.main.ScreenToWorldPoint(v);
        down_panel.transform.position = v;

    }
    void CameraColebrate()
    {
        Vector3 vstast = Camera.main.WorldToScreenPoint(StartSceneX.transform.position);
        
        float sh = 0;
        if (vstast.x > 0)
        {
            sh = _k_cam;
        }
        else
        {
            sh = -_k_cam;
        }
        if (_cam_sh != 0 && _cam_sh != sh)
        {
            _k_cam *= 0.9f;
            if (_k_cam < 0.01f)
            { 
                start = false;
                return;
            }
        }
        Vector3 vcam = Camera.main.transform.position;
        vcam.z += sh;
        Camera.main.transform.position = vcam;
        _cam_sh = sh;

    }

    void ÑreateBlock()
    {

        int idbl = leven_num % blocks_level.Count;

        var m = blocks_level[idbl].blocks;

        int idb = UnityEngine.Random.Range(0, m.Count);


        int ipriz = UnityEngine.Random.Range(0, priz_var);


        GameObject b = Instantiate(block) as GameObject;
        b.transform.parent = AllObgects.transform;

        BlockNode blnod = b.GetComponent<BlockNode>();

        blnod.down_panel = down_panel;

        Transform mt = b.transform.Find("meshes");

        blnod.meshes = mt.gameObject;


        if (ipriz == 0)
        {
            foreach(GameObject mesh in m)
            {
                (Instantiate(mesh) as GameObject).transform.parent = mt;
            }

            blnod.type = -1;

            blnod.ParticlePriz.SetActive(true);
        }
        else
        {


            GameObject mesh = m[idb];
            mesh = Instantiate(mesh) as GameObject;
            mesh.transform.parent = mt;

            blnod.type = (idbl + 1) * 1000 + idb;
            

        }





        int ix = UnityEngine.Random.Range(0, count_x_block);
       
        if(first)
        {
            first=false;
            if(ix == (count_x_block/2))
            {
                ix--;
            }
        }

        Vector3 v = b.transform.position;
        v.x = x_lines[ix];
        v.y = start_y_block;
        b.transform.position = v;









   //     b.GetComponent<Rigidbody>().velocity = new Vector3(0, -1.0F, 0);

        //Debug.Log(ix);


        /*
        GameObject apl_p = Resources.Load<GameObject>("aple_02");// GameObject.Find("aple_02");
        Debug.Log(apl_p);
        GameObject apl = Instantiate(apl_p) as GameObject;
        apl.transform.parent = apl_p.transform;
        */

    }

    public void BlocksOk(Transform t1, Transform t2, bool ok)
    {

        Vector3 v = new Vector3((t1.position.x + t2.position.x) * 0.5f, (t1.position.y + t2.position.y) * 0.5f, (t1.position.z + t2.position.z) * 0.5f);


        int cur_cror_add = 0;

        if(ok)
        {
            score_add += score_speed;

            cur_cror_add = math.max(1, (int)score_add);

            StartScorePoint(cur_cror_add, v,Color.green);

            DrawOk(t1, t2);
        }
        else
        {
            score_add -= score_speed;

            cur_cror_add = math.min(-1, (int)score_add);

            StartScorePoint(cur_cror_add, v, Color.red);

            DrawBoom(t1,t2);
        }

        score += cur_cror_add;

        CaclDifficult();


        //DrawScore();

    }

    private void StartScorePoint(int n, Vector3 pos, Color color)
    {

        GameObject mesh  = Instantiate(score_point) as GameObject;
        mesh.transform.position = pos;

        //*
        mesh.GetComponent<ScorePoint>().color = color;
        mesh.GetComponent<TextMeshPro>().text = (n > 0 ? "+": "") + n.ToString();
        //*/

    }



    float dts = 0;
    float dtk = 0;
    private void DrawScore()
    {
        if (draw_score == score)
        {
            dtk = 0;
            return;
        }

        dts += Time.deltaTime;

        if (dtk == 0)
        {
            dtk = sec_scor_add_end / (float)(score - draw_score);
        }
        else
        if (dts < dtk) //0.1f)
        {
            return;
        }
        dts = 0;

        if (draw_score < score)
        {
            draw_score++;
        }
        else
        {
            draw_score--;
        }

        tekst_score.text = (math.abs(draw_score)).ToString("D12");

        if (draw_score > 0)
        {
            tekst_score.color = new Color(0.5330188f, 1.0f, 0.6825123f);
        }else
        if (draw_score < 0)
        {
            tekst_score.color = new Color(1.0f, 0.5f, 0.5f); ;
        }
        else
        {
            tekst_score.color = Color.white;
        }

            dtk *= 0.75f;
    }

    private void CheckTap()
    {

        Camera cam = Camera.main;

        Vector3 p = new Vector3();
    //    Event currentEvent = Event.current;
        Vector3 mousePos = new Vector3();

        /*
        mousePos.x = currentEvent.mousePosition.x;
        mousePos.y = cam.pixelHeight - currentEvent.mousePosition.y;
        */

        mousePos.x = Input.mousePosition.x;
        mousePos.y = Input.mousePosition.y;
        mousePos.z = -cam.transform.position.z;

        p = cam.ScreenToWorldPoint(mousePos);

        int nx = -1;

        for (int i = 0; i < (count_x_block-1); i++)
        {
            if(p.x >= x_lines[i] && p.x < x_lines[i+1])
            {
                nx=i;
                break;
            }
        }

        if(nx == -1)
        {
            return;
        }


        Transform t1 = null;
        Transform t2 = null;

        foreach (Transform child in AllObgects.transform)
        {
            float x = child.position.x - x_lines[nx];
            float y = child.position.y - p.y;

            if(x*x+y*y <= 0.5f*0.5f)
            {
                t1 = child;
            }


            x = child.position.x - x_lines[nx+1];
            y = child.position.y - p.y;

            if (x * x + y * y <= 0.5f * 0.5f)
            {
                t2 = child;
            }

            if(t1!=null && t2!=null)
            {
                break;
            }

        }

        if (t1 != null)
        {
            t1.position = new Vector3(x_lines[nx+1], t1.position.y, t1.position.z);

            if (t2 == null)
            {
                t2 = Collided(t1);
                if (t2 != null)
                {
                    t2.position = new Vector3(x_lines[nx], t2.position.y, t2.position.z);
                    t2 = null;
                }
            }
        }





        if (t2 != null)
        {
            t2.position = new Vector3(x_lines[nx], t2.position.y, t2.position.z);

            if (t1 == null)
            {
                t1 = Collided(t2);
                if (t1 != null)
                {
                    t1.position = new Vector3(x_lines[nx + 1], t1.position.y, t1.position.z);
                    t1 = null;
                }
            }
        }



        if (t1 != null || t2 != null)
        {
            DrawSwap(t1, t2, (x_lines[nx + 1] + x_lines[nx])*0.5f );
        }


        /*
        int cc = AllObgects.transform.childCount;

        Transform ob;

        for(int i = 0; i < cc; i++)
        {
            ob = AllObgects.transform.GetChild(i);

            Debug.Log(ob.position);


        }
        */



            /*
            Debug.Log("Screen pixels: " + cam.pixelWidth + ":" + cam.pixelHeight);
            Debug.Log("Mouse position: " + mousePos);
            Debug.Log("World position: " + point);
            */


    }

    private void DrawSwap(Transform t1, Transform t2,float x)
    {
        float y;
        if(t1 == null)
        {
            y = t2.position.y;
        }
        else if (t2 == null)
        {
            y = t1.position.y;
        }
        else
        {
            y = (t1.position.y + t2.position.y) * 0.5f;
        }

        Instantiate(ParticleSwop, new Vector3(x, y, 0),new Quaternion());

    }

    private void DrawBoom(Transform t1, Transform t2)
    {
        float y = (t1.position.y + t2.position.y) * 0.5f;
        float x = (t1.position.x + t2.position.x) * 0.5f;


        Instantiate(ParticleBoom, new Vector3(x, y, 0), new Quaternion());

    }

    private void DrawOk(Transform t1, Transform t2)
    {
        float y = (t1.position.y + t2.position.y) * 0.5f;
        float x = (t1.position.x + t2.position.x) * 0.5f;


        Instantiate(ParticleOk, new Vector3(x, y, 0), new Quaternion());

    }

    private Transform Collided(Transform t)
    {
        Transform ret = null;
        foreach (Transform child in AllObgects.transform)
        {
            if (t != child)
            {
                if (t.position.x == child.position.x)
                {
                    if(((t.position.y - child.position.y)* (t.position.y - child.position.y)) < 1.0f  )
                    {
                        ret = child;
                        break;
                    }
                }
            }
        }
        return ret;
    }




    public void Pause()
    {
        is_paused = true;
        Time.timeScale = 0.0f;
        PanelPause.SetActive(true);
    }

    public void PauseOff()
    {
        is_paused = false;
        Time.timeScale = 1.0f;
        PanelPause.SetActive(false);
    }


}
