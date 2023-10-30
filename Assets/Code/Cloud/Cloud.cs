
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using System;
using System.Collections.Generic;
using Unity.Services.CloudSave;
using NUnit.Framework;





//https://docs.unity.com/ugs/en-us/manual/cloud-save/manual/tutorials/unity-sdk-sample
//https://docs.unity.com/ugs/en-us/manual/authentication/manual/platform-signin-username-password

public class Cloud
{
    private const string USERNAME = "BlockusHiScores";
    private const string PASSWORD = "Blockus_Hi_Scores_001";
    private const int COUNT_CLOUD_ITEMS = 300;

    //private event Action OnGetData;






    public List<UserCloudItem> userCloudItems { get; private set; } = new List<UserCloudItem>();

    public List<UserCloudItem> userItemsScore = new List<UserCloudItem>();
    public List<UserCloudItem> userItemsTime = new List<UserCloudItem>();
    public List<UserCloudItem> userItemsRating = new List<UserCloudItem>();

    public Cloud()
    {

    }

    //****************************
    public void SignOut()
    {
        AuthenticationService.Instance.SignOut();
        StaticLib.is_connect = false;
    }

    public async Task<bool> SignIn()
    {


        if (StaticLib.is_connect)
            return true;

        try
        {
            await UnityServices.InitializeAsync();
            AuthenticationService.Instance.SignInFailed += s => { Debug.Log("!!! " + s); };
            await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(USERNAME, PASSWORD);
            StaticLib.playerId_dev = SystemInfo.deviceUniqueIdentifier;// AuthenticationService.Instance.PlayerId;
            StaticLib.is_connect = true;
        }
        catch
        {
            //Debug.Log("try");
            return false;
        }

        return true;

    }

    //***********************************************************************
    public async Task GetData(Action OnGetData)
    {
        if (!await SignIn())
            return;

        //await AuthenticationService.Instance.SignInAnonymouslyAsync();
        //await AuthenticationService.Instance.AddUsernamePasswordAsync(USERNAME, PASSWORD);
        //await AuthenticationService.Instance.UpdatePasswordAsync(currentPassword, newPassword);
        //await AuthenticationService.Instance.DeleteAccountAsync();
        //await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(USERNAME, PASSWORD);
        try
        {
            await GetDataToList();
            if (OnGetData != null) OnGetData();
        }
        catch { }

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

    public async void SortLists()
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

        List<UserCloudItem> del = new List<UserCloudItem>();
        CropList(userItemsScore, ref del);
        CropList(userItemsTime, ref del);
        CropList(userItemsRating, ref del);

        ClearDelList(userItemsScore, ref del);
        ClearDelList(userItemsTime, ref del);
        ClearDelList(userItemsRating, ref del);

        foreach (UserCloudItem item in del)
        {
            await DeleteData(item);
        }



    }

    void ClearDelList(List<UserCloudItem> list, ref List<UserCloudItem> del)
    {
        List<UserCloudItem> idl = new List<UserCloudItem>();
        string id, nam;
        for (int i = 0; i < del.Count; i++)
        {
            id = del[i].id;
            nam = del[i].name;
            UserCloudItem it = list.Find(x => x.id == id && x.name == nam);
            if (it != null)
            {
                idl.Add(del[i]);
            }
        }

        foreach (UserCloudItem i in idl)
        {
            del.Remove(i);
        }
    }

    void CropList(List<UserCloudItem> list, ref List<UserCloudItem> del)
    {
        while (list.Count > 99)
        {
            del.Add(list[99]);
            list.RemoveAt(99);
        }
    }

    //******************************************************************************************

    public async Task DeleteData(UserCloudItem item)
    {
        if (!await SignIn())
            return;

        string key = item.id + "_" + item.name.Replace(" ", "_");

        //string key = StaticLib.playerId_dev + "_" + item.name.Replace(" ", "_");
        
        try
        {
            await CloudSaveService.Instance.Data.Player.DeleteAsync(key);
        }
        catch { }

    }

    //****************************************************************************************

    public async Task SaveData(UserCloudItem item, Action OnSaveData = null)
    {
        if (!await SignIn())
            return;
        await SaveDataToCloud(item);
        if (OnSaveData != null) OnSaveData();
    }

    async Task SaveDataToCloud(UserCloudItem item)
    {
        string key = StaticLib.playerId_dev + "_" + item.name.Replace(" ", "_");
        item.id = StaticLib.playerId_dev;
        var data = new Dictionary<string, object> { { key, item } };
        try
        {
            await CloudSaveService.Instance.Data.Player.SaveAsync(data);
        }
        catch { }

        //await CloudSaveService.Instance.Data.Player.DeleteAsync("key2");

    }

    //**************************************************************************************

    public async Task ClearData(Action OnClearData = null)
    {
        if (!await SignIn())
            return;
        await ClearDataToCloud();
        if (OnClearData != null) OnClearData();
    }

    async Task ClearDataToCloud()
    {
        userCloudItems.Clear();
        try
        {
            await CloudSaveService.Instance.Data.Player.DeleteAllAsync();
        }
        catch { }

    }

}

public class UserCloudItem
{
    public string name = "";
    public string id = "";
    public int time;
    public int score;

    public float GetRating()
    {
        return 60.0f * (float)score / (float)time;
    }
}
