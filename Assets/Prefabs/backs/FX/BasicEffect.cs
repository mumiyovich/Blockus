using System.Collections;
using System.Collections.Generic;
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

    /*
    // Start is called before the first frame update
    void Start()
    {
 
    }
    */



    // Update is called once per frame
    //void FixedUpdate()
    void Update()
    {
        if (processes_count == 0)
        {
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
