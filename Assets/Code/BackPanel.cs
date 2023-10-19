using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.XR;



public class BackPanel : MonoBehaviour
{
 

    [SerializeField] private List<GameObject> Backs;
    private int num_back;

    [SerializeField] private int _num_fx_test = -1;
    [SerializeField] private List<GameObject> Effects;

    [SerializeField]
    private GameObject BackMesh;
    private GameObject current_back = null;


    [SerializeField] private List<PiSinMoove> move_list;
    private int num_move;

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        num_back = 0;
        num_move = 0;


        NewBack();
    }


    void Update()
    {




    }


    public void NewBack()
    {
        GameObject new_back = (Instantiate(BackMesh) as GameObject);
        new_back.transform.parent = transform;
        GameObject mesh = (Instantiate(Backs[num_back]) as GameObject);
        mesh.transform.parent = new_back.transform;

        SinusoidalMotion sm = mesh.GetComponent<SinusoidalMotion>();
        if (sm != null)
        {
            if (sm.ForExternalParameters)
            {
                sm.SinMoov.Init(move_list[num_move]);
                num_move = (num_move + 1) >= move_list.Count ? 0 : (num_move + 1);
            }
        }
        num_back = (num_back + 1) >= Backs.Count ? 0 : (num_back + 1);

        new_back.SetActive(false);



        StartFX(current_back, new_back);


        current_back = new_back;
    }


    int _num_fx = -1;
    void StartFX(GameObject obj1, GameObject obj2)
    {
        _num_fx++;
        if (_num_fx >= Effects.Count)
            _num_fx = 0;


        GameObject odj_fx = Effects[_num_fx];

        if (_num_fx_test>-1)
            odj_fx = Effects[_num_fx_test];//!!!!!

        odj_fx = (Instantiate(odj_fx) as GameObject);
        odj_fx.transform.parent = transform;
        BasicEffect fx = odj_fx.GetComponent<BasicEffect>();
        fx.obj1 = obj1;
        fx.obj2 = obj2;

       
    }

    private IEnumerator ObjectDestroy(GameObject go, float tim)
    {
        float t = 0;
        do
        {
            t += Time.deltaTime;

            yield return null;
        } while (t < tim);
        Destroy(go);
        yield break;
    }

    private IEnumerator ObjectAtivate(GameObject go, float tim)
    {
        float t = 0;
        do
        {
            t += Time.deltaTime;

            yield return null;
        } while (t < tim);
        go.SetActive(true);
        yield break;
    }


}

