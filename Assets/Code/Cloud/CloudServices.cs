using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using Unity.Services.CloudSave.Models;
using Unity.Services.Core;
using UnityEngine;



//https://docs.unity.com/ugs/en-us/manual/cloud-save/manual/tutorials/unity-sdk-sample

public class CloudServices : MonoBehaviour
{
    bool _is_connect = false;

    [SerializeField] private TextMeshProUGUI text_tmp;

    private async void Awake()
    {

        await UnityServices.InitializeAsync();
        await SignInAnonymously();

    }

    private async Task SignInAnonymously()
    {
  
        AuthenticationService.Instance.SignedIn += () =>
        {
            string playerId = AuthenticationService.Instance.PlayerId;
            //Debug.Log("Signed in as: " + playerId);

            GetData();

            _is_connect = true;
        };
        AuthenticationService.Instance.SignInFailed += s =>
        {
            // Take some action here...
            //Debug.Log(s);
        };

      // await AuthenticationService.Instance.SignInAnonymouslyAsync();

      //  await AuthenticationService.Instance.AddUsernamePasswordAsync("test", "Test_001");

        await AuthenticationService.Instance.SignInWithUsernamePasswordAsync("test", "Test_001");



    }


    // Start is called before the first frame update
    void Start()
    {

       

    }

    // Update is called once per frame
    void Update()
    {  

    }

    private async void GetData()
    {
        

        var savedData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { "key" });

        var keys = await CloudSaveService.Instance.Data.Player.ListAllKeysAsync();

        var keys_v = await CloudSaveService.Instance.Data.Player.LoadAllAsync();


        text_tmp.text = savedData["key"].Value.GetAsString();


        /*
        var data = new Dictionary<string, object> { { "key", "someValue" } };
        await CloudSaveService.Instance.Data.Player.SaveAsync(data);


        await CloudSaveService.Instance.Data.Player.DeleteAsync("key2");

        */




    }
}
