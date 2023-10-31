
using System.Collections;
using TMPro;
using Unity.Mathematics;
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

    [SerializeField] private GameObject button;
    [SerializeField] private TextMeshProUGUI text_v;

    /*
    [SerializeField]
    private string _url_inf;

    [SerializeField]
    private string _version;
    */


    private string _apk_url;

    //string _path;

    // Start is called before the first frame update
    void Start()
    {

        text_v.text = Version.inst._version;
        /*
        BlockusInfo bi = new BlockusInfo();
        bi.version = "1.02.3";
        bi.urls = new string[2];
        bi.urls[0] = "ssss";
        bi.urls[1] = "aaaa";
        string json = JsonUtility.ToJson(bi);
        */


        //  _path = Application.persistentDataPath;
        //  _path = "/storage/emulated/0/Download";

        StartCoroutine(LoadInfo());
    }




    IEnumerator LoadInfo()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(Version.inst._url_inf))
        {

            yield return webRequest.SendWebRequest();

            if(webRequest.result!= UnityWebRequest.Result.Success)
                yield break;

            //tmp_text.text = webRequest.downloadHandler.text;
            //UnityEngine.Debug.Log(webRequest.downloadHandler.text);

            BlockusInfo bi = JsonUtility.FromJson<BlockusInfo>(webRequest.downloadHandler.text);

            if(!VersinOk(bi.version))
                yield break;


            _apk_url = bi.urls[0].Trim();

            button.SetActive(true);

            text_v.gameObject.SetActive(false);
        }

        


    }

    bool VersinOk(string new_v)
    {

        string[] cv = Version.inst._version.Split('.');
        string[] nv = new_v.Split('.');

        int com = math.max(cv.Length, nv.Length);

        for(int i=0; i < com; i++)
        {

            int n1 = NumToStr(ref cv ,i);
            int n2 = NumToStr(ref nv, i);

            if(n2>n1)
                return true;
        }



        return false;
    }

    int NumToStr(ref string[] m, int i)
    {

        if (i >= m.Length)
            return 0;

        string s = m[i].Trim();

        try
        {
            return int.Parse(s);
        }
        catch { return 0; }

    }


    public void BlockusUpdate()
    {

        Application.OpenURL(_apk_url);

        //StartCoroutine(LoadUpdate());

    }


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

        tmp_text.text = "run ok";
        }
        catch (Exception ex)
        {
            tmp_text.text = ex.Message;
        }      

    }
    */





}

public class BlockusInfo
{
    public string version;
    public string[] urls;
}
