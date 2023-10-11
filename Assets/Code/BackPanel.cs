using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.XR;
using static GameManager;

public class BackPanel : MonoBehaviour
{
    [SerializeField] private List<GameObject> Backs;


    private Transform meshes = null;

    private GameObject panel1 = null;
    private GameObject panel2 = null;


    private int num_move = 0;

    [SerializeField] private List<PiSinMoove> move_list;// = new List<PiSinMoove>();

    private Dictionary<GameObject, Vector3> pos_list = new Dictionary<GameObject, Vector3>();

    private void Awake()
    {
        //  _gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        //_gm = GameManager._gm;
        meshes = transform.Find("meshes");
    }

    // Start is called before the first frame update
    void Start()
    {

  
        num_move = 0;
        InitMoveParams();

        foreach (Transform child in meshes.transform)
        {
            child.gameObject.SetActive(false);
            pos_list.Add(child.gameObject ,  child.position);
        }

         ChangeMesh();


    }

    public void InitMoveParams()
    {

        move_list.Add(new PiSinMoove(1.0f,1.0f,0.0f,    1.0f,0.9f,0.0f));
        move_list.Add(new PiSinMoove(0.5f, 1.0f, 0.0f,  0.0f, 0.0f, 3.0f));
        move_list.Add(new PiSinMoove(0.5f, 1.0f, 0.0f,  1.0f, 0.7f, 3.14f));
        move_list.Add(new PiSinMoove(0.0f, 0.0f, 0.0f,  1.0f, 0.7f, 3.14f / 2.0f));
    }

    public void ChangeMesh()
    {
        panel1 = null;
        panel2 = meshes.transform.GetChild(0).gameObject;

        foreach (Transform child in meshes.transform)
        {
            if (panel1!=null)
            {
                panel2 = child.gameObject;
                break;
            }
            if (child.gameObject.activeSelf)
            {
                panel1 = child.gameObject;
            }
        }
        panel1?.SetActive(true);
        panel2?.SetActive(true);

        num_move++;
        if(num_move == move_list.Count)
            num_move = 0;

        StartCoroutine(routine: ChangeMeshProcess());
    }

    private IEnumerator ChangeMeshProcess()
    {

        float t = 0;

        panel1?.SetActive(true);
        panel2?.SetActive(true);

        while (t<=1)
        {
            if(panel1!=null)
            {
                GameManager.ChangeAlpha(panel1, 1.0f - t);
            }

            if (panel2 != null)
            {
                GameManager.ChangeAlpha(panel2, t);
            }
            t += Time.deltaTime;

            yield return null;
        }

        panel1?.SetActive(false);
        panel1 = null;
        GameManager.ChangeAlpha(panel2, 1.0f);

        yield break;
    }

    void Update()
    {
        

        if (panel1 != null)
        {
            int _num_move = num_move - 1;
            if (_num_move == -1)
            {
                _num_move = move_list.Count - 1;
            }
            panel1.transform.position = move_list[_num_move].Update(pos_list[panel1]);
        }

        if (panel2!=null)
        {
            panel2.transform.position = move_list[num_move].Update(pos_list[panel2]);
        }

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

    public Vector3 Pos {  get { return v; } }


    public PiSinMoove(float tx, float vx, float sx, float ty, float vy, float sy)
    {
        pos_x.speed = tx; pos_x.offset = sx; pos_x.scale = vx;
        pos_y.speed = ty; pos_y.offset = sy; pos_y.scale = vy;
    }

    public Vector3 Update(Vector3 offset)
    {
        v.x = math.sin(Time.time * pos_x.speed + pos_x.offset) * pos_x.scale + offset.x;
        v.y = math.cos(Time.time * pos_y.speed + pos_y.offset) * pos_y.scale + offset.y;
        v.z = offset.z;

        return v;
    }

}
