using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public enum BackFX { None, Fade, Over, Mask }

public class BasicEffect : MonoBehaviour
{
    protected BackFX type;
    public BackFX Type { get { return type; } }

    public float time = 1;

    public SmoothType smooth_type = SmoothType.Linear;

    protected int processes_count = 0;

    [HideInInspector] public GameObject obj1 = null;
    [HideInInspector] public GameObject obj2 = null;

    protected Material _material;

    public BasicEffect()
    {
        type = BackFX.Mask;
    }


    // Start is called before the first frame update
    void Start()
    {

        if(_material == null)
        {
            _material = GetComponent<Renderer>()?.material;
        }

        if (obj1 != null)
        {
            StaticLib.SetLayer(obj1, 12);
        }
        if (obj2 != null)
        {
            obj2.SetActive(true);
            StaticLib.SetLayer(obj2, 13);
        }

        StartProcesses();

    }

    virtual protected void StartProcesses()
    {
        if (_material == null)
            return;

        StartCoroutine(routine: Go_Mask());
        processes_count++;

    }

    private IEnumerator Go_Mask()
    {
        float t = 0;
        float ts = 0;
        float _min = 0;
        float _max = 0;

        do
        {
            ts = StaticLib.Smoothed(t, smooth_type);

            if (ts <= 0.5)
            {
                _min = 0;
                _max = Mathf.Lerp(0, 1, ts * 2);
            }
            else
            {
                _min = Mathf.Lerp(0, 1, ts * 2 - 1);
                _max = 1;
            }

            _material.SetFloat("_min", _min);
            _material.SetFloat("_max", _max);

            _material.SetFloat("_progress", ts);



            t += Time.deltaTime / time;
            yield return null;
        } while (t < 1);

        processes_count--;
        yield break;
    }




    // Update is called once per frame
    //void FixedUpdate()
    void Update()
    {
        if (processes_count == 0)
        {
            if (obj1 != null)
            {
                Destroy(obj1);
            }
            if (obj2 != null)
            {
                StaticLib.SetLayer(obj2, 0);
            }
            Destroy(gameObject);
        }

    }

    public static void FindRenderers(List<RendererItem> renderers, Transform tr)
    {

        Renderer r = tr.gameObject.GetComponent<Renderer>();
        if (r != null)
        {
            RendererItem ri = new RendererItem();
            ri.renderer = r;
            ri.material = r.material;
            ri.color = ri.material.color;

            renderers.Add(ri);
        }
        foreach (Transform child in tr)
        {
            FindRenderers(renderers, child);
        }
    }

    public static void FindAllChildrens<T>(List<T> list, GameObject go, GameObject exclude = null)
    {
        if (go != exclude)
        {
            T i = go.GetComponent<T>();
            if (i != null)
            {
                list.Add(i);
            }
        }
        foreach (Transform child in go.transform)
        {
            FindAllChildrens<T>(list, child.gameObject);
        }
    }

}
