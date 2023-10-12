using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;


public class TransparentObjects : MonoBehaviour
{

}

public class StaticLib
{
    public static void ChangeAlpha(List<RendererItem> renderers, float alphaVal, SmoothType type)
    {
        foreach(RendererItem item in renderers)
        {
            ChangeAlpha(item, alphaVal, type);
        }
    }

    public static void ChangeAlpha(RendererItem ri, float p_alphaVal, SmoothType type)
    {
        float alphaVal=0;
        if (p_alphaVal == 0 || p_alphaVal == 1 || type == SmoothType.Linear)
        {
            alphaVal = p_alphaVal;
        }
        else if (type == SmoothType.InOut)
        {
            alphaVal = 1.0f - (math.cos(p_alphaVal * math.PI) * 0.5f + 0.5f);
        }
        else if (type == SmoothType.In)
        {
            alphaVal = p_alphaVal * p_alphaVal;
        }
        else if (type == SmoothType.Out)
        {
            alphaVal = 1.0f - (1.0f - p_alphaVal) * (1.0f - p_alphaVal);
        }


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

            ri.material.SetOverrideTag("RenderType", "Transparent"); //--------------
            ri.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");//--------

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
