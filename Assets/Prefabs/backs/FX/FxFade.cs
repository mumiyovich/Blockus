using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class FxFade : BasicEffect
{

    private List<RendererItem> renderers1 = new List<RendererItem>();
    private List<RendererItem> renderers2 = new List<RendererItem>();

    FxFade()
    {
        type = BackFX.Fade;
    }


    // Start is called before the first frame update
    void Start()
    {

        if(obj1 != null)
        {
            FindRenderers(renderers1, obj1.transform);
            StartCoroutine(routine: Fade_1_0());
            processes_count++;
        }
        if (obj2 != null)
        {
            FindRenderers(renderers2, obj2.transform);
            obj2.SetActive(true);
            StartCoroutine(routine: Fade_0_1());
            processes_count++;
        }

    }

    private IEnumerator Fade_0_1()
    {
        float t = 0;

        do
        {
            StaticLib.ChangeAlpha(renderers2, t, smooth_type);
            t += Time.deltaTime / time;

            yield return null;
        } while (t < 1);

        StaticLib.ChangeAlpha(renderers2, 1, smooth_type);
        processes_count--;
        yield break;
    }

    private IEnumerator Fade_1_0()
    {
        float t = 0;

        do
        {
            StaticLib.ChangeAlpha(renderers1, 1.0f - t, smooth_type);
            t += Time.deltaTime / time;

            yield return null;
        } while (t < 1);
        
        Destroy(obj1);
        processes_count--;
        yield break;
    }



}
