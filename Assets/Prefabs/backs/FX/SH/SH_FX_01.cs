using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_FX_01 : BasicEffect
{

    public Material _material;

    SH_FX_01()
    {
        type = BackFX.Mask;
    }

    // Start is called before the first frame update
    void Start()
    {

        
        if (obj1 != null)
        {
            StaticLib.SetLayer(obj1, 12);
        }
        if (obj2 != null)
        {
            obj2.SetActive(true);
            StaticLib.SetLayer(obj2, 13);
        }

        StartCoroutine(routine: Go_SH_FX_01());
        processes_count++;

    }

    private IEnumerator Go_SH_FX_01()
    {
        float t = 0;
        float ts = 0;
        do
        {

            ts = StaticLib.Smoothed(t, smooth_type);

            _material.SetFloat("_time",ts* time*14);
            _material.SetFloat("_step", 1-ts);


            t += Time.deltaTime / time;

            yield return null;
        } while (t < 1);

        if (obj1 != null)
        {
            Destroy(obj1);
        }
        if (obj2 != null)
        {
            StaticLib.SetLayer(obj2, 0);
        }

        processes_count--;
        yield break;
    }

}
