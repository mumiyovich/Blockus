
using TMPro;
using UnityEngine;
using System;

public class EnterString : MonoBehaviour
{
    [SerializeField] private TMP_InputField text;
    //TextMeshProUGUI text;

    private string olt_txt = "";

    public Action OnEnter;


    // Start is called before the first frame update
    void Start()
    {
        olt_txt = SaveManager.state.name;
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
        if(olt_txt!="" && text.text!="" && olt_txt != text.text)
        {
            try
            {
                ChangeNameJnCloud();
            }catch{ }
            
        }


        SaveManager.state.name = text.text;

        SaveManager.state.name = SaveManager.state.name.Replace(((char)(8203)).ToString(), string.Empty);


        if (OnEnter!= null)
            OnEnter();
        Destroy(gameObject);
    }

    async void ChangeNameJnCloud()
    {

        Cloud cloud = new Cloud();
        await cloud.GetData(null);

        foreach(UserCloudItem i in cloud.userCloudItems)
        {
            if(i.id == StaticLib.playerId_dev && i.name == olt_txt)
            {
                await cloud.DeleteData(i);
                i.name = text.text;
                await cloud.SaveData(i);

                break;

            }
        }

    }
}
