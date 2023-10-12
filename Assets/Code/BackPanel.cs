using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.XR;



public class BackPanel : MonoBehaviour
{
    [SerializeField] private List<GameObject> Backs;
    private int num_back;

    [SerializeField]
    private GameObject BackMesh;
    private GameObject current_back = null;

    [SerializeField]
    private GameObject ColorQuad;


    [SerializeField] private List<PiSinMoove> move_list;
    private int num_move;

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        num_back = 0;
        num_move = 0;


        NewBack();
    }


    void Update()
    {




    }


    public void NewBack()
    {
        GameObject new_current_back = (Instantiate(BackMesh) as GameObject);
        new_current_back.transform.parent = transform;
        GameObject mesh = (Instantiate(Backs[num_back]) as GameObject);
        mesh.transform.parent = new_current_back.transform;

        FadeTransparent ft_new = new_current_back.GetComponent<FadeTransparent>();
        ft_new.InitInOut();

        bool off_cur = true;

        if (current_back != null)
        {
            FadeTransparent ft_cur = current_back.GetComponent<FadeTransparent>();

            if (ft_new.In == BackFX.ColorInOut)
            {
                ft_cur.Out = BackFX.None;
                StartFxColorInOut(current_back, new_current_back, ft_new.color_fx, ft_new.smooth_type, ft_new.fade_time);
                off_cur = false;
            }
            else
            if (ft_new.In == BackFX.ColorFlash)
            {
                ft_cur.Out = BackFX.None;
                StartFxColorFlash(ft_new.color_fx, ft_new.smooth_type, ft_new.fade_time);
            }


            if (ft_cur.Out == BackFX.ColorInOut)
            {
                ft_new.In = BackFX.None;
                StartFxColorInOut(current_back, new_current_back, ft_cur.color_fx, ft_cur.smooth_type, ft_cur.fade_time);
                off_cur = false;
            }
            else
            if (ft_cur.Out == BackFX.ColorFlash)
            {
                ft_new.In = BackFX.None;
                StartFxColorFlash(ft_cur.color_fx, ft_cur.smooth_type, ft_cur.fade_time);
            }




            if (off_cur)
                ft_cur.Off();


        }

        current_back = new_current_back;



        SinusoidalMotion sm = mesh.GetComponent<SinusoidalMotion>();
        if (sm != null)
        {
            if (sm.ForExternalParameters)
            {
                sm.SinMoov.Init(move_list[num_move]);

                num_move = (num_move + 1) >= move_list.Count ? 0 : (num_move + 1);
            }
        }

        num_back = (num_back + 1) >= Backs.Count ? 0 : (num_back + 1);


    }

    GameObject CreateFxColorQuad(Color p_color)
    {
        GameObject wq = (Instantiate(ColorQuad) as GameObject);
        wq.transform.parent = transform;
        wq.GetComponent<Renderer>().material.color = p_color;
        return wq;
    }

    void StartFxColorFlash(Color p_color, SmoothType p_smoov, float p_time)
    {
        GameObject wq = CreateFxColorQuad(p_color);
        FadeTransparent ft = wq.GetComponent<FadeTransparent>();
        ft.In = BackFX.None;
        ft.Out = BackFX.Fade;
        ft.on_of = false;
        ft.smooth_type = p_smoov;
        if (p_time != 0) ft.fade_time = p_time;
        ft.Off();
    }

    void StartFxColorInOut(GameObject go1, GameObject go2, Color p_color, SmoothType p_smoov, float p_time)
    {
        GameObject wq = CreateFxColorQuad(p_color);
        FadeTransparent ft = wq.GetComponent<FadeTransparent>();
        ft.In = BackFX.Fade;
        ft.Out = BackFX.Fade;
        ft.on_of = true;
        if(p_time!=0) ft.fade_time = p_time;
        float f_tim = ft.fade_time;
        ft.smooth_type = p_smoov;

        go2?.SetActive(false);

        if (go1 != null)
        {
            StartCoroutine(routine: ObjectDestroy(go1, f_tim));
        }
        if (go2 != null)
        {
            StartCoroutine(routine: ObjectAtivate(go2, f_tim));
        }

    }

    private IEnumerator ObjectDestroy(GameObject go, float tim)
    {
        float t = 0;
        do
        {
            t += Time.deltaTime;

            yield return null;
        } while (t < tim);
        Destroy(go);
        yield break;
    }

    private IEnumerator ObjectAtivate(GameObject go, float tim)
    {
        float t = 0;
        do
        {
            t += Time.deltaTime;

            yield return null;
        } while (t < tim);
        go.SetActive(true);
        yield break;
    }


}

