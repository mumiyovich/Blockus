
using TMPro;
using UnityEngine;
using System;

public class EnterString : MonoBehaviour
{
    [SerializeField] private TMP_InputField text;
        //TextMeshProUGUI text;

    public Action OnEnter;


    // Start is called before the first frame update
    void Start()
    {

        text.text = SaveManager.state.name;


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Cancel();
        }

        if (Input.GetKeyUp(KeyCode.Return))
        {
            Ok();
        }

    }

    public void Cancel()
    {
        Destroy(gameObject);
    }
    public void Ok()
    {
        SaveManager.state.name = text.text;

        SaveManager.state.name = SaveManager.state.name.Replace(((char)(8203)).ToString(), string.Empty);


        if (OnEnter!= null)
            OnEnter();
        Destroy(gameObject);
    }
}
