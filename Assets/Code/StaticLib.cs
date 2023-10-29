//using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
//using Unity.VisualScripting;
//using System.Xml.Serialization;
//using System.IO;
using System.Linq;
using System.Reflection;
using System;
using System.Linq.Expressions;

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

public static class StaticLib
{

    //public static bool is_connect { get;  set; }
    public static bool is_connect = false;
    //public string playerId { get; private set; }
    public static string playerId = "";

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
            //   v = 1.0f - (math.cos(t * math.PI) * 0.5f + 0.5f);
            v = t * t * (3 - 2 * t);
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

    public static string TimeSecToStr(int time)
    {
        int h = time / 3600;
        int m = time / 60 - h * 60;
        int c = time - m * 60 - h * 3600;
        return (h < 100 ? h.ToString("D2") : h.ToString()) + ":" + m.ToString("D2") + ":" + c.ToString("D2");
    }

    public static void SetLayer(GameObject go, int layer)
    {

        if (go == null)
            return;
        go.layer = layer;
        foreach (Transform child in go.transform)
        {
            SetLayer(child.gameObject, layer);
        }
    }


    public static void ChangeAlpha(List<RendererItem> renderers, float alphaVal, SmoothType type)
    {
        foreach (RendererItem item in renderers)
        {
            ChangeAlpha(item, alphaVal, type);
        }
    }

    public static void ChangeAlpha(RendererItem ri, float p_alphaVal, SmoothType type)
    {
        float alphaVal = Smoothed(p_alphaVal, type);

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

            if (ri.renderer_stares._Surface != 1.0f)
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


/*
    public static string Serialize<T>(this T toSerialize)
    {
        XmlSerializer xml = new XmlSerializer(typeof(T));
        StringWriter sw = new StringWriter();
        xml.Serialize(sw, toSerialize);
        return sw.ToString();
    }

    public static T Deserialize<T>(this string toDeserialize)
    {
        XmlSerializer xml = new XmlSerializer(typeof(T));
        StringReader sr = new StringReader(toDeserialize);
        return (T)xml.Deserialize(sr);
    }
    */


}

class MapperFactory
{
    public static Action<P, Q> CreateMapper<P, Q>()
    {
        var sourceType = typeof(P);
        var targetType = typeof(Q);

        var sourceProperties = GetVisibleProperties(sourceType);
        var targetProperties = GetVisibleProperties(targetType);

        var fromVar = Expression.Variable(sourceType, "from");
        var toVar = Expression.Variable(targetType, "to");

        Expression CreateCopyProperty(string name)
        {
            try
            {
                return Expression.Assign(
                    Expression.Property(toVar, targetProperties[name]),
                    Expression.Property(fromVar, sourceProperties[name]));
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Невозможно скопировать свойство {name}", ex);
            }
        }

        var commonProperties = sourceProperties.Keys.Intersect(targetProperties.Keys)
                                                    .OrderBy(n => n);
        var assignExpressions = commonProperties.Select(CreateCopyProperty);
        var assignment = Expression.Block(assignExpressions);
        var lambda = Expression.Lambda<Action<P, Q>>(assignment, fromVar, toVar);
        return lambda.Compile();
    }

    static Dictionary<string, PropertyInfo> GetVisibleProperties(Type type)
    {
        IEnumerable<Type> GetTypeChain(Type t) =>
            t == null ? Enumerable.Empty<Type>() : GetTypeChain(t.BaseType).Prepend(t);

        Dictionary<string, PropertyInfo> result = new Dictionary<string, PropertyInfo>();
        foreach (var t in GetTypeChain(type))
        {
            var properties = t.GetProperties(BindingFlags.Instance |
                                        BindingFlags.Public | BindingFlags.DeclaredOnly);
            foreach (var prop in properties)
            {
                if (!result.ContainsKey(prop.Name))
                    result.Add(prop.Name, prop);
            }
        }
        return result;
    }

}


