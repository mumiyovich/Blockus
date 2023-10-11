using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonOnOff : MonoBehaviour
{

    [SerializeField]
    private Sprite sprite_on;

    [SerializeField]
    private Sprite sprite_off;

    [SerializeField]
    private bool on;

    public bool On
    {
        get { return on; }
        set
        {
            if (on != value)
            { 
                on = value;
                CahgeState();
            }
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        on = !on;
        Swap();
        
        
    }

    virtual public void Swap()
    {
        On = !On;
    }

    void CahgeState()
    {
        Image img = GetComponent<Image>();
        if(on)
            img.sprite = sprite_on;
        else
            img.sprite = sprite_off;

    }


}


