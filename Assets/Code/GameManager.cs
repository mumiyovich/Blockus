using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
//using System.Drawing;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;


//To format a selection: Ctrl + K, Ctrl + F
//To format a document: Ctrl + K, Ctrl + D


public class GameManager : MonoBehaviour
{
    [HideInInspector] public Dictionary<GameObject, BlockNode> blocks_in_down = new Dictionary<GameObject, BlockNode>();


    [SerializeField] private GameObject diff_progres;

    [SerializeField] private GameObject ParticleSwop;
    [SerializeField] private GameObject ParticleBoom;
    [SerializeField] private GameObject ParticleOk;

    [SerializeField] private GameObject PanelPause;

    [SerializeField] private GameObject down_panel;
    [SerializeField] private GameObject StartSceneX;
    private bool start = true;

    private float _cam_sh = 0;
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
    [SerializeField] public int count_x_block;
    [SerializeField] private int leven_num = 0;

    [SerializeField]
    private float min_gravity;

    [SerializeField]
    private float max_gravity;

    [SerializeField] private float _gravity;



    [SerializeField] private int priz_var;

    [SerializeField] private int lives;
    private int score = 0;
    private int draw_score = 0;

    private int score_diff = 0;

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

        for (int i = 0; i < count_x_block; i++)
        {
            x_lines[i] = first_x_block + ((last_x_block - first_x_block) / (float)(count_x_block - 1)) * (float)i;
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

        score_diff = math.min(math.max(score_diff, 0), max_score);

        float k = ((float)(score_diff)) / (float)max_score;

        diff_progres.transform.localScale = new Vector3(k, diff_progres.transform.localScale.y, diff_progres.transform.localScale.z);

        _gravity = (max_gravity - min_gravity) * k + min_gravity;


        time_new_block = (max_time_new_block - min_time_new_block) * (1.0f - k) + min_time_new_block;

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

        tekst_sc.text = "FPS: " + fps.ToString() + " MIN: " + min_fps + " MAX: " + max_fps;

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

        //if(Input.GetMouseButtonUp(0))
        if (Input.GetMouseButtonDown(0))
        {
            CheckTap();
        }

        if (blocks_in_down.Count == count_x_block)
        {
            CheckBonusBlockInDown();
        }


    }

    void CheckBonusBlockInDown()
    {
        Dictionary<int, int> co = new Dictionary<int, int>();
        foreach (KeyValuePair<GameObject, BlockNode> bl in blocks_in_down)
        {
            if (co.ContainsKey(bl.Value.type) == true)
            {
                co[bl.Value.type]++;
            }
            else
            {
                co.Add(bl.Value.type, 1);
            }
        }

        if (co.Count == count_x_block)
        {
            foreach (KeyValuePair<GameObject, BlockNode> bl in blocks_in_down)
            {
                BlocksOk(bl.Key.transform, bl.Key.transform, true, 50);
                bl.Value.collected = true;
                Destroy(bl.Key);
            }

        }
        else
        if (co.Count == 1)
        {
            foreach (KeyValuePair<GameObject, BlockNode> bl in blocks_in_down)
            {
                BlocksOk(bl.Key.transform, bl.Key.transform, true, 200);
                bl.Value.collected = true;
                Destroy(bl.Key);
            }
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

        blnod._gm = this;

        blnod.down_panel = down_panel;

        Transform mt = b.transform.Find("meshes");

        blnod.meshes = mt.gameObject;


        if (ipriz == 0)
        {
            foreach (GameObject mesh in m)
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

        if (first)
        {
            first = false;
            if (ix == (count_x_block / 2))
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

    public void BlocksOk(Transform t1, Transform t2, bool ok, int bonus = 0)
    {

        Vector3 v = new Vector3((t1.position.x + t2.position.x) * 0.5f, (t1.position.y + t2.position.y) * 0.5f, (t1.position.z + t2.position.z) * 0.5f);


        int cur_cror_add = 0;

        if (ok)
        {
            score_add += score_speed;

            cur_cror_add = math.max(1, (int)score_add);

            if (bonus == 0.0f)
            {
                StartScorePoint(cur_cror_add, v, Color.green);
            }
            else
            {
                StartScorePoint(bonus, v, Color.green);
            }

            DrawOk(t1, t2);
        }
        else
        {
            score_add -= score_speed;

            cur_cror_add = math.min(-1, (int)score_add);

            if (bonus == 0.0f)
            {
                StartScorePoint(cur_cror_add, v, Color.red);
            }
            else
            {
                StartScorePoint(bonus, v, Color.red);
            }

            DrawBoom(t1, t2);
        }

        if (bonus == 0.0f)
        {
            score += cur_cror_add;
        }
        else
        {
            score += bonus;
        }


        score_diff += cur_cror_add;

        CaclDifficult();


        //DrawScore();

    }

    private void StartScorePoint(int n, Vector3 pos, Color color)
    {

        GameObject mesh = Instantiate(score_point) as GameObject;
        mesh.transform.position = pos;

        //*
        mesh.GetComponent<ScorePoint>().color = color;
        mesh.GetComponent<TextMeshPro>().text = (n > 0 ? "+" : "") + n.ToString();
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
        }
        else
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

    private Vector3 MouseToScren()
    {
        Camera cam = Camera.main;
        Vector3 p = new Vector3();
        Vector3 mousePos = new Vector3();
        mousePos.x = Input.mousePosition.x;
        mousePos.y = Input.mousePosition.y;
        mousePos.z = -cam.transform.position.z;
        p = cam.ScreenToWorldPoint(mousePos);
        return p;
    }

    private void CheckTap()
    {
        Vector3 p = MouseToScren();

        Transform tl = null;
        Transform tr = null;
        float y, x;
        float tl_d = 999999;
        float tr_d = 999999;

        int i1 = 0;
        int i2 = 0;
        int ii = -1;

        float dist1;
        float dist2;

        foreach (Transform child in AllObgects.transform)
        {

            /*
            y = child.position.y - p.y;

            
            if (y * y > 0.5f * 0.5f)
                continue;
            */

            x = child.position.x - p.x;

            dist1 = Vector2.Distance(new Vector2(child.position.x, child.position.y), new Vector2(p.x, p.y));

            if (x > 0)
            {
                if (tr == null)
                {
                    tr = child;
                }
                else
                {
                    dist2 = Vector2.Distance(new Vector2(tr.position.x, tr.position.y), new Vector2(p.x, p.y));
                    if(dist1< dist2)
                        tr = child;

                    /*
                    if (child.position.x < tr.position.x)                
                        tr = child;
                    */
                }
                tr_d = math.min(tr_d, math.abs(x));
            }
            else
            if (x < 0)
            {
                if (tl == null)
                {
                    tl = child;
                }
                else
                {
                    dist2 = Vector2.Distance(new Vector2(tl.position.x, tl.position.y), new Vector2(p.x, p.y));
                    if (dist1 < dist2)
                        tl = child;

                    /*
                    if (child.position.x > tl.position.x)
                        tl = child;
                    */
                }
                tl_d = math.min(tl_d, math.abs(x));
            }
        }

        if (tl != null && tr != null)
        {
            y = tl.position.y - tr.position.y;
            if (y * y > 1.0f)
            {
                if (math.abs(tl.position.y - p.y) < math.abs(tr.position.y - p.y))
                {
                    tr = null;
                }
                else
                {
                    tl = null;
                }
            }
        }

        if (tl == null && tr == null)
        {
            return;
        }
        else if (tl != null && tr != null)
        {


            i1 = Array.IndexOf(x_lines, tl.position.x);
            i2 = Array.IndexOf(x_lines, tr.position.x);

            if (math.abs(i2 - i1) == 1)
            {
                x = tl.position.x;
                tl.position = new Vector3(tr.position.x, tl.position.y, tl.position.z);
                tr.position = new Vector3(x, tr.position.y, tr.position.z);

                DrawSwap(tl, tr, (x_lines[i1] + x_lines[i2]) * 0.5f);
                return;
            }

            if (tl_d < tr_d)
                tr = null;
            else
                tl = null;

        }

        tl = (tl == null) ? tr : tl;

        ii = Array.IndexOf(x_lines, tl.position.x);

        int nx = -1;
        float min_x = 999999;
        float n_x;

        for (int i = 0; i < count_x_block; i++)
        {
            n_x = x_lines[i] - p.x;
            n_x *= n_x;
            if (n_x < min_x)
            {
                nx = i;
                min_x = n_x;
            }
        }

        if (ii == nx)
        {
            x = tl.position.x - p.x;

            if (x < 0)
            {
                nx++;
            }
            else
            {
                nx--;
            }

            nx = math.min(math.max(nx, 0), count_x_block - 1);

        }

        tl.position = new Vector3(x_lines[nx], tl.position.y, tl.position.z);

        DrawSwap(tl, tr, x_lines[nx]);

    }

    private void _CheckTap()
    {

        /*
        Camera cam = Camera.main;
        Vector3 p = new Vector3();
        Vector3 mousePos = new Vector3();
        mousePos.x = Input.mousePosition.x;
        mousePos.y = Input.mousePosition.y;
        mousePos.z = -cam.transform.position.z;

        p = cam.ScreenToWorldPoint(mousePos);
        */

        Vector3 p = MouseToScren();


        int nx = -1;

        for (int i = 0; i < (count_x_block - 1); i++)
        {
            if (p.x >= x_lines[i] && p.x < x_lines[i + 1])
            {
                nx = i;
                break;
            }
        }

        if (nx == -1)
        {
            return;
        }


        Transform t1 = null;
        Transform t2 = null;

        foreach (Transform child in AllObgects.transform)
        {
            float x = child.position.x - x_lines[nx];
            float y = child.position.y - p.y;

            if (x * x + y * y <= 0.5f * 0.5f)
            {
                t1 = child;
            }


            x = child.position.x - x_lines[nx + 1];
            y = child.position.y - p.y;

            if (x * x + y * y <= 0.5f * 0.5f)
            {
                t2 = child;
            }

            if (t1 != null && t2 != null)
            {
                break;
            }

        }

        if (t1 != null)
        {
            t1.position = new Vector3(x_lines[nx + 1], t1.position.y, t1.position.z);

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
            DrawSwap(t1, t2, (x_lines[nx + 1] + x_lines[nx]) * 0.5f);
        }

    }

    private void DrawSwap(Transform t1, Transform t2, float x)
    {
        float y;
        if (t1 == null)
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

        Instantiate(ParticleSwop, new Vector3(x, y, 0), new Quaternion());

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
                    if (((t.position.y - child.position.y) * (t.position.y - child.position.y)) < 1.0f)
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
