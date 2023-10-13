using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FxMoveRL : BasicEffect
{
    public SmoothType smooth_type_end = SmoothType.Linear;

    public float x_start;
    public float x_middle;
    public float x_end;

    private List<Transform> objects = new List<Transform>();

    FxMoveRL()
    {
        type = BackFX.Over;
    }

    // Start is called before the first frame update
    void Start()
    {
        FindAllChildrens<Transform>(objects, gameObject);
        StartCoroutine(routine: Move());
        processes_count++;

    }

    private IEnumerator Move()
    {
        float t = 0;

        do
        {
            foreach( Transform tr in objects)
            {
                tr.position = new Vector3(StaticLib.SmoothedLerp(x_start, x_middle, t, smooth_type), tr.position.y, tr.position.z);
            }

            t += Time.deltaTime / (time * 0.5f);
            yield return null;
        } while (t < 1);

        /*
        foreach (Transform tr in objects)
        {
            tr.position = new Vector3(x_middle, tr.position.y, tr.position.z);
        }
        */

        if (obj1 != null)
        {
            Destroy(obj1);
        }

        if (obj2 != null)
        {
            obj2.SetActive(true);
        }

        t = 0;

        do
        {
            foreach (Transform tr in objects)
            {
                tr.position = new Vector3(StaticLib.SmoothedLerp(x_middle, x_end, t, smooth_type_end), tr.position.y, tr.position.z);
            }
            t += Time.deltaTime / (time * 0.5f);

            yield return null;
        } while (t < 1);

        processes_count--;
        yield break;
    }


}
