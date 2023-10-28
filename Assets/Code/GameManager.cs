
using Pixeye.Unity;
using System;
using System.Collections;
//using System.Collections;
using System.Collections.Generic;
//using System.Linq;
//using System.Drawing;
using TMPro;
using Unity.Mathematics;
//using Unity.VisualScripting;
//using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.SceneManagement;
//using UnityEngine.SocialPlatforms.Impl;
//using UnityEngine.UIElements;
//using UnityEngine.XR;
//using static UnityEngine.GraphicsBuffer;


//To format a selection: Ctrl + K, Ctrl + F
//To format a document: Ctrl + K, Ctrl + D


public class GameManager : MonoBehaviour
{
    private bool start = true;
    private float _cam_sh = 0;
    private float _k_cam = 0.1f;
    //   private int score = 0;
    private int draw_score = 0;
    //    private int score_diff = 0;
    private float[] x_lines;
    [SerializeField] private float sec_scor_add_end = 1.0f;
    private bool is_paused = false;
    private bool first = true;

    [SerializeField] private State state = SaveManager.state;

    [HideInInspector] public float scale_block = 1;


    private BackPanel backPanel;


    [HideInInspector] public Dictionary<GameObject, BlockNode> blocks_in_down = new Dictionary<GameObject, BlockNode>();


    [Foldout("Particles:", true)]
    //[Header("Particles:")]
    [SerializeField] private GameObject ParticleSwop;
    [SerializeField] private GameObject ParticleBoom;
    [SerializeField] private GameObject ParticleOk;

    [Foldout("Objects:", true)]
    //[Header("Objects:")]
    [SerializeField] private GameObject PanelPause;
    [SerializeField] private GameObject down_panel;
    [SerializeField] private GameObject StartSceneX;
    [SerializeField] private GameObject block;
    [SerializeField] private GameObject score_point;
    [SerializeField] private GameObject AllObgects;
    [SerializeField] private GameObject SolidBlock;
    [SerializeField] private GameObject Back_Panel;


    [System.Serializable]
    public struct Blocks_level
    {
        public List<GameObject> blocks;
    }

    [Foldout("Objects blocks:", true)]
    // [Header("Objects blocks:")]
    [SerializeField] private List<Blocks_level> blocks_level;

    [Foldout("Texts:", true)]
    // [Header("Texts:")]
    [SerializeField] private TextMeshProUGUI text_tmp;
    [SerializeField] private TextMeshProUGUI text_score;
    [SerializeField] private TextMeshProUGUI text_time;



    [Foldout("Blocks params:", true)]
    // [Header("Blocks params:")]
    [SerializeField] private float start_y_block;
    [SerializeField] private float first_x_block;
    [SerializeField] private float last_x_block;
    [SerializeField] public float down_panel_height;
    [SerializeField] public float height_down_line;
    private float st_first_x_block;
    private float st_last_x_block;

    /*
    [SerializeField] private int _count_x_block;
    public int count_x_block
    {
        get { return _count_x_block; }
        set
        {
            if (_count_x_block != value)
            {
                AllOk();
                _count_x_block = value;
                InitXLines();
            }
        }
    }
    */
    public int count_x_block
    {
        get { return state.count_x_block; }
        set
        {
            if (state.count_x_block != value)
            {
                AllOk();
                state.count_x_block = value;
                InitXLines();
            }
        }
    }

    [Foldout("Difficult:", true)]
    //[Header("Difficult:")]
    [SerializeField] private GameObject diff_progres;
    //   [SerializeField] private int leven_num = 0;
    [SerializeField] private float min_gravity;
    [SerializeField] private float max_gravity;
    [SerializeField] private float _gravity;
    [SerializeField] private int priz_var;
    [SerializeField] private int solid_var;
    //   [SerializeField] private int lives;
    [SerializeField] private float min_time_new_block;
    [SerializeField] private float max_time_new_block;
    [SerializeField] private float time_new_block;
    [SerializeField] private int max_score;
    [SerializeField] private float score_speed;
    //   [SerializeField] private float score_add = 0;//!



    [HideInInspector] public static GameManager _gm;


    private Coroutine coroutineProcessDrawScore;

    public GameManager()
    {
        _gm = this;
    }



    private void Awake()
    {
        Application.targetFrameRate = 500;// 60;
                                          //QualitySettings.vSyncCount = 0;

        st_first_x_block = first_x_block;
        st_last_x_block = last_x_block;

        backPanel = Back_Panel.GetComponent<BackPanel>();



        SaveManager.Load();//!!!!!!!!

    }


    // Start is called before the first frame update
    void Start()
    {
        //https://docs.unity3d.com/ScriptReference/Camera.ScreenToWorldPoint.html

        first = true;

        start = true;
        _cam_sh = 0;
        _k_cam = 0.25f;

        InitXLines();

        DrawScore();
        CaclDifficult();

        _float_game_time_sec = state.time;

    }

    private void OnDestroy()
    {
        SaseState();
    }
    private void OnApplicationFocus(bool focus)
    {
        SaseState();
    }
    private void OnApplicationPause(bool pause)
    {
        SaseState();
    }

    void SaseState()
    {
        SaveManager.Save();
    }

    void InitXLines()
    {

        scale_block = 5.0f / (float)count_x_block;
        float ofx = (1.0f - scale_block) * 0.5f;
        first_x_block = st_first_x_block - ofx;
        last_x_block = st_last_x_block + ofx;


        x_lines = new float[count_x_block];
        for (int i = 0; i < count_x_block; i++)
        {
            x_lines[i] = first_x_block + ((last_x_block - first_x_block) / (float)(count_x_block - 1)) * (float)i;
        }

    }

    private void CaclDifficult()
    {

        state.score_diff = math.min(math.max(state.score_diff, 0), max_score);

        float k = ((float)(state.score_diff)) / (float)max_score;

        // k = 0.4f;
        Color diff_co;
        Color diff_co1 = new Color(0, 0.8051351f, 0.8392157f);
        Color diff_co2 = new Color(0.8310356f, 0.8396226f, 0);
        Color diff_co3 = new Color(0.6792453f, 0.1765224f, 0);
        if (k <= 0.5f)
        {
            diff_co = Color.Lerp(diff_co1, diff_co2, k * 2.0f);
        }
        else
        {
            diff_co = Color.Lerp(diff_co2, diff_co3, (k - 0.5f) * 2.0f);
        }
        diff_progres.GetComponent<UnityEngine.UI.Image>().color = diff_co;

        diff_progres.transform.localScale = new Vector3(k, diff_progres.transform.localScale.y, diff_progres.transform.localScale.z);

        _gravity = (max_gravity - min_gravity) * k + min_gravity;


        time_new_block = (max_time_new_block - min_time_new_block) * (1.0f - k) + min_time_new_block;


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

        text_tmp.text = "FPS:" + fps.ToString() + " MIN:" + min_fps + " MAX:" + max_fps;

    }

    float time_level_change = 0;


    float _float_game_time_sec = 0;

    void Update()
    {


        if (is_paused)
            return;


        DrFPS();


        _float_game_time_sec += Time.deltaTime;

        if (((int)(_float_game_time_sec)) > state.time)
        {
            state.time = (int)(_float_game_time_sec);
            /*
            int h = (int)(state.time / 3600);
            int m = (int)(state.time / 60 - h * 60);
            int c = (int)(state.time - m * 60 - h * 60);
            text_time.text = (h < 100 ? h.ToString("D2") : h.ToString()) + ":" + m.ToString("D2") + ":" + c.ToString("D2");
            */

            text_time.text = StaticLib.TimeSecToStr(state.time);
        }



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

            //  backPanel.ChangeMesh();
        }

        if (blocks_in_down.Count == count_x_block)
        {
            CheckBonusBlockInDown();
        }


        if (Input.GetKeyUp(KeyCode.Q))
        {
            count_x_block--;
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            count_x_block++;
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            backPanel.NewBack();
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            SaseState();
            SceneManager.LoadScene(0);
        }


        ///////!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        time_level_change += Time.deltaTime;
        if (time_level_change >= 60 * 3)
        {
            time_level_change = 0;


            backPanel.NewBack();

            NextLevel();
            count_x_block++;
            if (count_x_block == 8)
                count_x_block = 3;



        }

    }
    //int old_count_x_block = 5;

    void NextLevel()
    {
        AllOk();
        state.leven_num++;
    }

    void AllOk(int p_scor = 0)
    {

        foreach (Transform child in AllObgects.transform)
        {
            BlocksOk(child, child, true, p_scor);
            Destroy(child.gameObject);

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
                BlocksOk(bl.Key.transform, bl.Key.transform, true, count_x_block * 10);
                bl.Value.collected = true;
                Destroy(bl.Key);
            }

        }
        else
        if (co.Count == 1)
        {
            foreach (KeyValuePair<GameObject, BlockNode> bl in blocks_in_down)
            {
                BlocksOk(bl.Key.transform, bl.Key.transform, true, count_x_block * 30);
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

    //static bool tmp=false;
    void ÑreateBlock()
    {
        /*
        if (tmp)
            return;
        tmp = true;
        //*/

        if (AllObgects.transform.childCount >= count_x_block * 10)
        {
            return;
        }

        int idbl = state.leven_num % blocks_level.Count;

        var m = blocks_level[idbl].blocks;

        int idb = UnityEngine.Random.Range(0, m.Count);


        /*
        int ipriz = UnityEngine.Random.Range(0, priz_var);
        int isolid = UnityEngine.Random.Range(0, solid_var);
        */
        bool ipriz = PiRND.GetProbability("priz", priz_var);
        bool isolid = PiRND.GetProbability("solid", solid_var);


        GameObject b = Instantiate(block) as GameObject;
        b.transform.parent = AllObgects.transform;

        BlockNode blnod = b.GetComponent<BlockNode>();

        blnod._gm = this;

        blnod.down_panel = down_panel;

        Transform mt = b.transform.Find("meshes");

        blnod.meshes = mt.gameObject;


        if (ipriz)
        {
            foreach (GameObject mesh in m)
            {
                (Instantiate(mesh) as GameObject).transform.parent = mt;
            }
            //(Instantiate(SolidBlock) as GameObject).transform.parent = mt;

            blnod.type = -1;

            blnod.ParticlePriz.SetActive(true);
        }
        else if (isolid)
        {
            (Instantiate(SolidBlock) as GameObject).transform.parent = mt;
            blnod.type = -2;
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

    }

    public void BlocksOk(Transform t1, Transform t2, bool ok, int bonus = -9999999)
    {

        Vector3 v = new Vector3((t1.position.x + t2.position.x) * 0.5f, (t1.position.y + t2.position.y) * 0.5f, (t1.position.z + t2.position.z) * 0.5f);


        int cur_cror_add = 0;

        if (ok)
        {
            state.score_add += score_speed;

            cur_cror_add = math.max(1, (int)state.score_add);

            if (bonus == -9999999)
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
            if (state.score_add > 0)
            {
                //score_add *= 0.5f;
                //state.score_add = 0;
                state.score_add = -state.score_add;
            }
            state.score_add -= score_speed;

            cur_cror_add = math.min(-1, (int)state.score_add);


            if (bonus == -9999999)
            {
                StartScorePoint(cur_cror_add, v, Color.red);
            }
            else
            {
                StartScorePoint(bonus, v, Color.red);
            }

            DrawBoom(t1, t2);
        }

        if (bonus == -9999999)
        {
            state.score += cur_cror_add;
        }
        else
        {
            state.score += bonus;
        }


        state.score_diff += cur_cror_add;

        CaclDifficult();


        //DrawScore();

    }

    private void StartScorePoint(int n, Vector3 pos, Color color)
    {

        if (n == 0)
            return;

        GameObject mesh = Instantiate(score_point) as GameObject;
        mesh.transform.position = pos;

        //*
        mesh.GetComponent<ScorePoint>().color = color;
        mesh.GetComponent<TextMeshPro>().text = (n > 0 ? "+" : "") + n.ToString();
        //*/

    }



    private void DrawScore()
    {
        if (draw_score == state.score)
            return;

        if (coroutineProcessDrawScore != null)
            StopCoroutine(coroutineProcessDrawScore);

        coroutineProcessDrawScore = StartCoroutine(routine: ProcessDrawScore());

    }

    private IEnumerator ProcessDrawScore()
    {
        float old_score = draw_score;
        draw_score = state.score;
        float new_score = 0;

        float t = 0;

        float sp = math.min((math.abs(state.score - old_score) / 5.0f), 1);
        sp = sp * sec_scor_add_end + 0.01f;

        do
        {
            t += Time.deltaTime / sp;
            t = math.min(t, 1);

            new_score = StaticLib.SmoothedLerp(old_score, state.score, t, SmoothType.In);
            text_score.text = ((int)math.abs(new_score)).ToString("D12");

            if (new_score > 0)
            {
                text_score.color = new Color(0.5330188f, 1.0f, 0.6825123f);
            }
            else if (new_score < 0)
            {
                text_score.color = new Color(1.0f, 0.5f, 0.5f); ;
            }
            else
            {
                text_score.color = Color.white;
            }

            yield return null;
        } while (t < 1);

        yield break;
    }

    /*
    float dts = 0;
    float dtk = 0;
    private void DrawScore()
    {
        if (draw_score == state.score)
        {
            dtk = 0;
            return;
        }

        dts += Time.deltaTime;

        if (dtk == 0)
        {
            dtk = sec_scor_add_end / (float)(state.score - draw_score);
        }
        else
        if (dts < dtk) //0.1f)
        {
            return;
        }
        dts = 0;

        if (draw_score < state.score)
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
    */
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

        Transform t1 = null;
        Transform t2 = null;
        float x, y_new, y_old, dist_new, dist_old;
        int ax = 0;


        bool click_in_up = (p.y - (down_panel.transform.position.y + down_panel_height)) >= height_down_line * scale_block;


        foreach (Transform child in AllObgects.transform)
        {
            if (math.abs(child.position.y - start_y_block) < scale_block * 0.5f)
                continue;

            x = child.position.x - p.x;

            if (t1 == null)
            {
                t1 = child;
                ax = x > 0 ? -1 : 1;
            }
            else
            {
                y_new = p.y - child.position.y;
                y_old = p.y - t1.position.y;
                dist_new = Vector2.Distance(new Vector2(child.position.x, child.position.y), new Vector2(p.x, p.y));
                dist_old = Vector2.Distance(new Vector2(t1.position.x, t1.position.y), new Vector2(p.x, p.y));

                if (y_new < 0 && y_old > 0 && dist_old > scale_block)
                {
                    t1 = child;
                    ax = x > 0 ? -1 : 1;
                }
                else if (y_new > 0 && y_old < 0 && dist_new > scale_block)
                {

                }
                else if (dist_new < dist_old)
                {
                    t1 = child;
                    ax = x > 0 ? -1 : 1;
                }
            }
        }

        if (t1 == null)
            return;

        int i1 = Array.IndexOf(x_lines, t1.position.x);
        int i2 = 0;
        float min_x = 999999;
        float n_x;
        for (int i = 0; i < count_x_block; i++)
        {
            n_x = x_lines[i] - p.x;
            n_x *= n_x;
            if (n_x < min_x)
            {
                i2 = i;
                min_x = n_x;
            }
        }

        if (i1 == i2)
        {
            i2 = math.max(math.min(i2 + ax, count_x_block - 1), 0);
        }

        float pos_x = x_lines[i2];

        foreach (Transform child in AllObgects.transform)
        {
            if (child == t1)
                continue;

            if (child.position.x == pos_x)
                if (math.abs(child.position.y - t1.position.y) <= scale_block)
                {
                    t2 = child;
                    break;
                }
        }

        float xt1 = t1.position.x;
        t1.position = new Vector3(pos_x, t1.position.y, t1.position.z);
        if (t2 != null)
        {
            t2.position = new Vector3(xt1, t2.position.y, t2.position.z);
            DrawSwap(t1, t2, p);// (x_lines[i1] + x_lines[i2]) * 0.5f);
        }
        else
        {
            DrawSwap(t1, t2, p);// x_lines[i1]);
        }



    }

    /*
        private void CalcOnTapObj(ref Transform t, Transform child, ref float t_d, Vector3 p, float x)
        {

            if (t == null)
            {
                t = child;
            }
            else
            {




                float y_new = p.y - child.position.y;
                float y_old = p.y - t.position.y;


                if (y_new < 0 && y_old > 0 && y_old * y_old > scale_block * scale_block)
                {
                    t = child;
                }
                else
                {
                    float dist_new = Vector2.Distance(new Vector2(child.position.x, child.position.y), new Vector2(p.x, p.y));
                    float dist_old = Vector2.Distance(new Vector2(t.position.x, t.position.y), new Vector2(p.x, p.y));
                    if (dist_new < dist_old)
                        t = child;
                }

            }
            t_d = math.min(t_d, math.abs(x));

        }

        private void _CheckTap()
        {
            Vector3 p = MouseToScren();

            if (start_y_block - p.y < 0.5f)
                return;

            Transform tl = null;
            Transform tr = null;
            float x;
            float tl_d = 999999;
            float tr_d = 999999;

            int i1 = 0;
            int i2 = 0;
            int ii = -1;

            // float dist1;
            //float dist2;

            bool click_in_up = (p.y - (down_panel.transform.position.y + down_panel_height)) >= height_down_line * scale_block;

            foreach (Transform child in AllObgects.transform)
            {
                //float start_dist = child.position.y - start_y_block;
                // if (start_dist * start_dist > scale_block * scale_block)
                if (math.abs(child.position.y - start_y_block) < scale_block)
                {
                    continue;
                }


                if (click_in_up)
                {
                    if (child.position.y - (down_panel.transform.position.y + down_panel_height) < height_down_line * scale_block)
                    {
                        continue;
                    }
                }


                x = child.position.x - p.x;

                //
                if (x > 0)
                {
                    CalcOnTapObj(ref tr, child, ref tr_d, p, x);
                }
                else
                if (x < 0)
                {
                    CalcOnTapObj(ref tl, child, ref tl_d, p, x);
                }
            }


            if (tl != null && tr != null)
            {
                float y = tl.position.y - tr.position.y;
                if (y * y > scale_block * scale_block) // 1.0f)//z
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
    */
    /*
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
    */

    private void DrawSwap(Transform t1, Transform t2, Vector3 p)
    {

        Instantiate(ParticleSwop, p, new Quaternion());

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
                    if (((t.position.y - child.position.y) * (t.position.y - child.position.y)) < scale_block * scale_block) // <1.0f
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


public static class PiRND
{
    private static Dictionary<object, int> _probability = new Dictionary<object, int>();

    public static void Clear()
    {
        _probability.Clear();
    }

    public static bool GetProbability(object Key, int Probability, int MinProbability = 0, int MaxProbability = 0)
    {
        if (Probability == 0)
            return false;

        if (MinProbability == 0)
            MinProbability = Probability / 2;
        if (MaxProbability == 0)
            MaxProbability = Probability + Probability / 2;

        if (_probability.ContainsKey(Key) == false)
            _probability.Add(Key, 0);

        int count = ++_probability[Key];

        if (count < MinProbability)
        {
            _probability[Key] = count;
            return false;
        }

        if (count > MaxProbability)
        {
            _probability[Key] = 0;
            return true;
        }

        int ok = UnityEngine.Random.Range(0, Probability);

        if (ok == 0)
        {
            _probability[Key] = 0;
            return true;
        }

        return false;
    }

}
