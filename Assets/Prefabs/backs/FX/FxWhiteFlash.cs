using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FxWhiteFlash : BasicEffect
{
    private List<RendererItem> renderers = new List<RendererItem>();

    FxWhiteFlash()
    {
        type = BackFX.Over;
    }

    // Start is called before the first frame update
    void Start()
    {
        FindRenderers(renderers, transform);

        if (obj1!=null)
        {
            Destroy(obj1);
        }

        if (obj2 != null)
        {
            obj2.SetActive(true);
        }

        StartCoroutine(routine: Fade_1_0());
        processes_count++;
        

    }

    private IEnumerator Fade_1_0()
    {
        float t = 0;

        do
        {
            StaticLib.ChangeAlpha(renderers, 1.0f - t, smooth_type);
            t += Time.deltaTime / time;

            yield return null;
        } while (t < 1);

        processes_count--;
        yield break;
    }

}
