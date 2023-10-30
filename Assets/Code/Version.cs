using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class Version : MonoBehaviour
{

    [SerializeField]
    public string _url_inf;

    [SerializeField]
    public string _version;

    public static Version inst;

    public Version()
    {
        inst = this;
    }


    /*
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    */
}
