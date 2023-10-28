
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using System;
using System.Collections.Generic;
using Unity.Services.CloudSave;





//https://docs.unity.com/ugs/en-us/manual/cloud-save/manual/tutorials/unity-sdk-sample
//https://docs.unity.com/ugs/en-us/manual/authentication/manual/platform-signin-username-password

public class Cloud
{
    private const string USERNAME = "BlockusHiScores";
    private const string PASSWORD = "Blockus_Hi_Scores_001";
    private const int COUNT_CLOUD_ITEMS = 300;

    //private event Action OnGetData;


    public bool is_connect { get; private set; }

    public string playerId { get; private set; }

    public List<UserCloudItem> userCloudItems { get; private set; } = new List<UserCloudItem>();

    public List<UserCloudItem> userItemsScore { get; private set; } = new List<UserCloudItem>();
    public List<UserCloudItem> userItemsTime { get; private set; } = new List<UserCloudItem>();
    public List<UserCloudItem> userItemsRating { get; private set; } = new List<UserCloudItem>();

    public Cloud()
    {

    }

    //****************************
    public void SignOut()
    {
        AuthenticationService.Instance.SignOut();
        is_connect = false;
    }

    public async Task SignIn()
    {


        if (is_connect)
            return;

        try
        {
            await UnityServices.InitializeAsync();
            AuthenticationService.Instance.SignInFailed += s => { Debug.Log("!!! " + s); };
            await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(USERNAME, PASSWORD);
            playerId = AuthenticationService.Instance.PlayerId;
            is_connect = true;
        }
        catch
        { }

    }

    //***********************************************************************
    public async Task GetData(Action OnGetData)
    {
        await SignIn();

        //await AuthenticationService.Instance.SignInAnonymouslyAsync();
        //await AuthenticationService.Instance.AddUsernamePasswordAsync(USERNAME, PASSWORD);
        //await AuthenticationService.Instance.UpdatePasswordAsync(currentPassword, newPassword);
        //await AuthenticationService.Instance.DeleteAccountAsync();
        //await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(USERNAME, PASSWORD);

        await GetDataToList();
        if (OnGetData != null) OnGetData();

    }

    async Task GetDataToList()
    {
        userCloudItems.Clear();
        userItemsScore.Clear();
        userItemsTime.Clear();
        userItemsRating.Clear();

        var results = await CloudSaveService.Instance.Data.Player.LoadAllAsync();
        foreach (var result in results)
        {
            userCloudItems.Add(result.Value.Value.GetAs<UserCloudItem>());
        }

        SortLists();
    }

    public void SortLists()
    {
        userItemsScore = new List<UserCloudItem>(userCloudItems);
        userItemsTime = new List<UserCloudItem>(userCloudItems);
        userItemsRating = new List<UserCloudItem>(userCloudItems);

        userItemsScore.Sort(delegate (UserCloudItem x, UserCloudItem y)
        {
            return y.score.CompareTo(x.score);
        });

        userItemsTime.Sort(delegate (UserCloudItem x, UserCloudItem y)
        {
            return y.time.CompareTo(x.time);
        });

        userItemsRating.Sort(delegate (UserCloudItem x, UserCloudItem y)
        {
            return y.GetRating().CompareTo(x.GetRating());
        });



    }

    //****************************************************************************************

    public async Task SaveData(UserCloudItem item, Action OnSaveData = null)
    {
        await SignIn();
        await SaveDataToCloud(item);
        if (OnSaveData != null) OnSaveData();
    }

    async Task SaveDataToCloud(UserCloudItem item)
    {
        string key = playerId + "_" + item.name.Replace(" ","_");
        item.id = playerId;
        var data = new Dictionary<string, object> { { key, item } };

        await CloudSaveService.Instance.Data.Player.SaveAsync(data);
  
        //await CloudSaveService.Instance.Data.Player.DeleteAsync("key2");

    }

    //**************************************************************************************

    public async Task ClearData(Action OnClearData = null)
    {
        await SignIn();
        await ClearDataToCloud();
        if (OnClearData != null) OnClearData();
    }

    async Task ClearDataToCloud()
    {
        userCloudItems.Clear();

        await CloudSaveService.Instance.Data.Player.DeleteAllAsync();

    }

}

public struct UserCloudItem
{
    public string name;
    public string id;
    public int time;
    public int score;

    public float GetRating()
    {
        return 60.0f * (float)score / (float)time;
    }
}
