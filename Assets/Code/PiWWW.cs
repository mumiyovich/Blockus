
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;







//https://habr.com/ru/articles/433366/

//google
//inf
//https://drive.google.com/file/d/10Vk8Mzeiv1cqlR582KFwrb28YI2XgkNq/view?usp=sharing
//https://drive.google.com/uc?export=download&id=10Vk8Mzeiv1cqlR582KFwrb28YI2XgkNq

//apk
//https://drive.google.com/file/d/10Y4I4THbpGZsCgEMTFkCjCJowuBTVdyY/view?usp=sharing
//https://drive.google.com/uc?export=download&id=10Y4I4THbpGZsCgEMTFkCjCJowuBTVdyY



//DB


//apk
//https://www.dropbox.com/scl/fi/pueaf6lkus6nw9hmz6f7m/Blockus.apk?rlkey=n3h3bqs2ip86ewb2j382k9dvx&dl=1

//inf
//https://www.dropbox.com/scl/fi/5euxz4tzc75adcvlx3932/info.txt?rlkey=axz882cxd9bsoei72w6trhl5g&dl=1

public class PiWWW : MonoBehaviour
{


    [SerializeField]
    private string _url_inf;

    [SerializeField]
    private string _version;

    [SerializeField]
    private string _apk_url;

    //string _path;

    // Start is called before the first frame update
    void Start()
    {

      //  _path = Application.persistentDataPath;
      //  _path = "/storage/emulated/0/Download";

        StartCoroutine(LoadInfo());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /*
    void GetInfo()
    {
        WebClient web = new WebClient();
        web.DownloadFileCompleted += EndGetInfo;
        web.DownloadFileAsync(new Uri(_url_inf),Application.dataPath+"/blocus_info.txt");

    }
   
    public void EndGetInfo(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
    {
        if(e.Error!= null)
            return;


    }
    */

    IEnumerator LoadInfo()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(_url_inf))
        {

            yield return webRequest.SendWebRequest();

            if(webRequest.result!= UnityWebRequest.Result.Success)
                yield break;

            //tmp_text.text = webRequest.downloadHandler.text;
            //UnityEngine.Debug.Log(webRequest.downloadHandler.text);

        }


    }
    //*
    public void BlockusUpdate()
    {

        Application.OpenURL(_apk_url);

        //StartCoroutine(LoadUpdate());

    }
    //*/
    /*
    IEnumerator LoadUpdate()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(_apk_url))
        {
            tmp_text.text = "11";
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
                yield break;
            ;
            tmp_text.text = "22";
            try
            {
                File.WriteAllBytes(Application.persistentDataPath + "/Blocus.apk", webRequest.downloadHandler.data);

                tmp_text.text = "zxc";
                System.Diagnostics.Process.Start(Application.persistentDataPath + "/Blocus.apk");
                tmp_text.text = "xxx";

            }
            catch(Exception e) 
            { 
                tmp_text.text = e.Message;
            }

            


          //  Debug.Log(webRequest.downloadHandler.text);

        }


    }*/


    /*
    public void BlockusUpdate()
    {

        tmp_text.text = "up";

        WebClient web = new WebClient();
        web.DownloadFileCompleted += EndDownloadVers;
        web.DownloadProgressChanged+= ProcDownloadVers;
        web.DownloadFileAsync(new Uri(_apk_url), _path + "/Blocus.apk");

    }
    */
    /*
    public void ProcDownloadVers(object sender, DownloadProgressChangedEventArgs e)
    {
        tmp_text.text = e.ProgressPercentage.ToString();

    }
    */
    /*
    public void EndDownloadVers(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
    {
        tmp_text.text = "end";
        if (e.Error != null)
        {
            tmp_text.text = e.Error.Message;
            return;
        }

        tmp_text.text = "end ok";


        //System.Diagnostics.Process.Start(Application.persistentDataPath + "/Blocus.apk");

        try
        {


            string apkPath = _path + "/Blocus.apk";


            using AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            using AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            using AndroidJavaObject unityContext = currentActivity.Call<AndroidJavaObject>("getApplicationContext");

            string packageName = unityContext.Call<string>("getPackageName");
            string authority = packageName + ".provider";

            AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
            string ACTION_VIEW = intentClass.GetStatic<string>("ACTION_VIEW");
            AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent", ACTION_VIEW);

            int FLAG_ACTIVITY_NEW_TASK = intentClass.GetStatic<int>("FLAG_ACTIVITY_NEW_TASK");
            int FLAG_GRANT_READ_URI_PERMISSION = intentClass.GetStatic<int>("FLAG_GRANT_READ_URI_PERMISSION");
            int FLAG_GRANT_WRITE_URI_PERMISSION = intentClass.GetStatic<int>("FLAG_GRANT_WRITE_URI_PERMISSION");

            using AndroidJavaObject fileObj = new AndroidJavaObject("java.io.File", apkPath);
            using AndroidJavaClass fileProvider = new AndroidJavaClass("androidx.core.content.FileProvider");

            object[] providerParams = new object[3];
            providerParams[0] = unityContext;
            providerParams[1] = authority;
            providerParams[2] = fileObj;

            using AndroidJavaObject uri = fileProvider.CallStatic<AndroidJavaObject>("getUriForFile", providerParams);

            intentObject.Call<AndroidJavaObject>("setDataAndType", uri, "application/vnd.android.package-archive");
            intentObject.Call<AndroidJavaObject>("setFlags", FLAG_ACTIVITY_NEW_TASK);
            currentActivity.Call("grantUriPermission", packageName, uri, FLAG_GRANT_READ_URI_PERMISSION);
            currentActivity.Call("grantUriPermission", packageName, uri, FLAG_GRANT_WRITE_URI_PERMISSION);
            intentObject.Call<AndroidJavaObject>("addFlags", FLAG_GRANT_READ_URI_PERMISSION | FLAG_GRANT_WRITE_URI_PERMISSION);
            currentActivity.Call("startActivity", intentObject);
          
            */



            /*
            string uri = _path + "/Blocus.apk";


            using (var intentClass = new AndroidJavaClass("android.content.Intent"))
            {
                using (var intentObject = new AndroidJavaObject("android.content.Intent", intentClass.GetStatic<string>("ACTION_VIEW")))
                {
                    using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                    {
                        using (AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                        {
                            AndroidJavaObject unityContext = currentActivity.Call<AndroidJavaObject>("getApplicationContext");

                            string packageName = unityContext.Call<string>("getPackageName");
                            string authority = packageName + ".fileprovider";

                            int FLAG_ACTIVITY_NEW_TASK = intentObject.GetStatic<int>("FLAG_ACTIVITY_NEW_TASK");
                            int FLAG_GRANT_READ_URI_PERMISSION = intentObject.GetStatic<int>("FLAG_GRANT_READ_URI_PERMISSION");

                            AndroidJavaObject fileObj = new AndroidJavaObject("java.io.File", uri);
                            AndroidJavaClass fileProvider = new AndroidJavaClass("android.support.v4.content.FileProvider");
                            AndroidJavaObject javaUri = fileProvider.CallStatic<AndroidJavaObject>("getUriForFile", unityContext, authority, fileObj);

                            intentObject.Call<AndroidJavaObject>("setDataAndType", javaUri, "application/vnd.android.package-archive");
                            intentObject.Call<AndroidJavaObject>("addFlags", FLAG_ACTIVITY_NEW_TASK);
                            intentObject.Call<AndroidJavaObject>("addFlags", FLAG_GRANT_READ_URI_PERMISSION);


                                currentActivity.Call("startActivity", intentObject);
   
                        }
                    }

                }
            }

*/

            //Application.OpenURL("file:"+_path + "/Blocus.apk");
            //Process.Start(_path + "/Blocus.apk");
            //Application.OpenURL(_path + "/Blocus.apk");


            /*
            string url = "robloxmobile://placeID=370731277";


            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject packageManager = currentActivity.Call<AndroidJavaObject>("getPackageManager");

            AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
            AndroidJavaObject uriData = uriClass.CallStatic<AndroidJavaObject>("parse", url);

            AndroidJavaObject i = packageManager.Call<AndroidJavaObject>("getLaunchIntentForPackage", "com.roblox.client");

            i.Call<AndroidJavaObject>("setClassName", "com.roblox.client", "com.roblox.client.ActivityProtocolLaunch");
            i.Call<AndroidJavaObject>("setAction", "android.intent.action.VIEW");
            i.Call<AndroidJavaObject>("setData", uriData);

            currentActivity.Call("startActivity", i);
            */
/*
            tmp_text.text = "run ok";
        }
        catch (Exception ex)
        {
            tmp_text.text = ex.Message;
        }

        

    }
    */





}
