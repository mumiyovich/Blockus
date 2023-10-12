using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[System.Serializable]
public enum BackFX {Null, None, Fade, ColorFlash, ColorInOut }

public class BackPanelProperty : MonoBehaviour
{
    public float fade_time;

    public BackFX In;
    public BackFX Out;

    public SmoothType smooth_type;

    public Color color;

}
