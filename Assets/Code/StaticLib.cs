using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.VisualScripting;

public enum SmoothType { Linear, InOut, In, Out }

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

public class StaticLib
{
    public static float SmoothedLerp(float i1, float i2, float t, SmoothType type)
    {
        float k = Smoothed(t, type);
        return math.lerp(i1, i2, k);
    }

    public static float Smoothed(float t, SmoothType type)
    {
        float v = 0;
        if (t == 0 || t == 1 || type == SmoothType.Linear)
        {
            v = t;
        }
        else if (type == SmoothType.InOut)
        {
            v = 1.0f - (math.cos(t * math.PI) * 0.5f + 0.5f);
        }
        else if (type == SmoothType.In)
        {
            v = t * t;
        }
        else if (type == SmoothType.Out)
        {
            v = 1.0f - (1.0f - t) * (1.0f - t);
        }

        return v;
    }

    public static void ChangeAlpha(List<RendererItem> renderers, float alphaVal, SmoothType type)
    {
        foreach(RendererItem item in renderers)
        {
            ChangeAlpha(item, alphaVal, type);
        }
    }

    public static void ChangeAlpha(RendererItem ri, float p_alphaVal, SmoothType type)
    {
        float alphaVal= Smoothed(p_alphaVal, type);

        if (alphaVal == 0)
        {
            ri.renderer.enabled = false;
        }
        if (alphaVal == 1)
        {
            ri.renderer.enabled = true;
            ri.material.color = ri.color;
            ri.material.SetFloat("_Surface", ri.renderer_stares._Surface);
            ri.material.SetFloat("_Blend", ri.renderer_stares._Blend);      
            ri.material.SetInt("_SrcBlend", ri.renderer_stares._SrcBlend);
            ri.material.SetInt("_DstBlend", ri.renderer_stares._DstBlend);
            ri.material.SetInt("_ZWrite", ri.renderer_stares._ZWrite);       
            ri.material.renderQueue = ri.renderer_stares.renderQueue;

            //   ri.material.SetOverrideTag("RenderType", "Transparent"); //--------------
            //   ri.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");//--------
        }
        else
        {
            ri.renderer.enabled = true;
            ri.material.color = new Color(ri.color.r, ri.color.g, ri.color.b, alphaVal);

            if(ri.renderer_stares._Surface!=1.0f)
                ri.material.SetFloat("_Surface", 1.0f);
            if (ri.renderer_stares._Blend != 0.0f)
                ri.material.SetFloat("_Blend", 0.0f);
            if (ri.renderer_stares._SrcBlend != (int)UnityEngine.Rendering.BlendMode.SrcAlpha)
                ri.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            if (ri.renderer_stares._DstBlend != (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha)
                ri.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            if (ri.renderer_stares._ZWrite != 0)
                ri.material.SetInt("_ZWrite", 0);
            if (ri.renderer_stares.renderQueue != (int)UnityEngine.Rendering.RenderQueue.Transparent)
                ri.material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;

         //   ri.material.SetOverrideTag("RenderType", "Transparent"); //--------------
         //   ri.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");//--------

        }


    }

    public static void ChangeAlpha(GameObject go, float alphaVal)
    {
        Renderer renderer = go.GetComponent<Renderer>();
        Color oldColor = renderer.material.color;
        Color newColor = new Color(oldColor.r, oldColor.g, oldColor.b, alphaVal);
        renderer.material.color = newColor;
        renderer.material.SetFloat("_Surface", (float)1);
        renderer.material.SetFloat("_Blend", (float)0);
        renderer.material.SetOverrideTag("RenderType", "Transparent");
        renderer.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        renderer.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        renderer.material.SetInt("_ZWrite", 0);
        renderer.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        renderer.material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
        //renderer.material.SetShaderPassEnabled("ShadowCaster", false);
    }

}
