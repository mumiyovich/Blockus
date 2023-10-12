using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SinusoidalMotion : MonoBehaviour
{
    [SerializeField] public bool ForExternalParameters;
    [SerializeField] private PiSinMoove sin_moov = new PiSinMoove();
    public PiSinMoove SinMoov {  get { return sin_moov; } }

    [HideInInspector] public Vector3 bas_pos;

    // Start is called before the first frame update
    void Start()
    {
        bas_pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        SinMoov.Update(Time.time, bas_pos);
        transform.position = SinMoov.Pos;     
    }


}



[System.Serializable]
public struct PiSinParams
{
    public float speed;
    public float offset;
    public float scale;
}

[System.Serializable]
public class PiSinMoove
{
    public PiSinParams pos_x;
    public PiSinParams pos_y;
    public PiSinParams pos_z;

    private Vector3 v = new Vector3(0, 0, 0);

    public Vector3 Pos { get { return v; } }

    public void Init(PiSinMoove over)
    {
        pos_x = over.pos_x;
        pos_y = over.pos_y;
        pos_z = over.pos_z;
    }

    public Vector3 Update(float t)
    {
        return Update(t, new Vector3(0,0,0));
    }

    public Vector3 Update(float t, Vector3 bas_pos)
    {
        v.x = math.sin(t * pos_x.speed + pos_x.offset) * pos_x.scale + bas_pos.x;
        v.y = math.cos(t * pos_y.speed + pos_y.offset) * pos_y.scale + bas_pos.y;
        v.z = 0 + bas_pos.z;

        return v;
    }

}