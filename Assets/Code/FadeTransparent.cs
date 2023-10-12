using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR;

public enum SmoothType { Linear, InOut, In, Out }

public class FadeTransparent : MonoBehaviour
{

    [SerializeField] public float fade_time;
    [SerializeField] public SmoothType smooth_type;


    private List<RendererItem> renderers = new List<RendererItem>();

    public BackFX In = BackFX.Null;
    public BackFX Out = BackFX.Null;
    public Color color_fx = Color.white;

    public bool on_of = false;

    public void InitInOut()
    {
        if (transform.childCount == 1)
        {
            BackPanelProperty pp = transform.GetChild(0).GetComponent<BackPanelProperty>();
            if (pp != null)
            {
                In = In == BackFX.Null ? pp.In : In;
                Out = Out == BackFX.Null ? pp.Out : Out;
                color_fx = pp.color;
                smooth_type = pp.smooth_type;
                if(pp.fade_time!=0)
                    fade_time = pp.fade_time;

            }
        }

        In = In == BackFX.Null ? BackFX.Fade : In;
        Out = Out == BackFX.Null ? BackFX.Fade : Out;
    }


    // Start is called before the first frame update
    void Start()
    {

        InitInOut();


        if (In == BackFX.Fade || Out == BackFX.Fade)
        {
            FindRenderers(transform);
        }

        if (In == BackFX.Fade)
        {
            StartCoroutine(routine: FadeStart());
        }
                
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Off()
    {
        if (Out == BackFX.Fade)
        {
            StartCoroutine(routine: FadeEnd());
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    void FindRenderers(Transform tr)
    {

        Renderer r = tr.gameObject.GetComponent<Renderer>();
        if(r != null)
        {
            RendererItem ri = new RendererItem();
            ri.renderer = r;
            ri.material = r.material;
            ri.color = ri.material.color;

            renderers.Add(ri);
        }

        foreach (Transform child in tr)
        {
            FindRenderers(child);
        }


    }

    private IEnumerator FadeStart()
    {
        float t = 0;

        do
        {
            StaticLib.ChangeAlpha(renderers, t, smooth_type);
            t += Time.deltaTime / fade_time;

            yield return null;
        } while (t < 1);

        StaticLib.ChangeAlpha(renderers, 1, smooth_type);

        if (on_of)
            Off();

        yield break;
    }

    private IEnumerator FadeEnd()
    {
        float t = 0;

        do
        {
            StaticLib.ChangeAlpha(renderers, 1.0f-t, smooth_type);
            t += Time.deltaTime / fade_time;

            yield return null;
        } while (t < 1) ;
            //StaticLib.ChangeAlpha(renderers, 0);
            Destroy(gameObject);
        yield break;
    }


}

public struct RendererItem
{
    public struct RendererStares
    {
        public float _Surface;
        public float _Blend;
        public int _SrcBlend;
        public int _DstBlend;
        public int _ZWrite;
        public int renderQueue;
    }

    public RendererStares renderer_stares;

    public Renderer renderer;
    private Material _material;
    public Material material
    {  
        get { return _material; } 
        set
        { 
            _material = value;

            renderer_stares._Surface = _material.GetFloat("_Surface");
            renderer_stares._Blend = _material.GetFloat("_Blend");
            renderer_stares._SrcBlend = _material.GetInt("_SrcBlend");
            renderer_stares._DstBlend = _material.GetInt("_DstBlend");
            renderer_stares._ZWrite = _material.GetInt("_ZWrite");
            renderer_stares.renderQueue = _material.renderQueue;
        }
    }
    public Color color;
}
