
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;



//https://docs.unity.com/ugs/en-us/manual/cloud-save/manual/tutorials/unity-sdk-sample
//https://docs.unity.com/ugs/en-us/manual/authentication/manual/platform-signin-username-password

public class CloudServices : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI text_tmp;


    [SerializeField]
    private GameObject itemTemplate;
    [SerializeField]
    private GameObject content;

    [SerializeField]
    private GameObject scroll;

    private Cloud cloud = new Cloud();


    private void Awake()
    {

    }

    // Start is called before the first frame update
    async void Start()
    {
        // await cloud.ClearData();
        await cloud.GetData(OnGetData);

        /*
        UserCloudItem item;
            item = new UserCloudItem();
            item.name = "N_"+ (i.ToString("D2"));
            item.time = Random.Range(0, 10000);
            item.score = Random.Range(0, 10000);
            await cloud.SaveData(item);
        

        await cloud.GetData(OnGetData);
        */


    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnGetData()
    {
        //    itemTemplate.GetComponent<RectTransform>().rect.height;
        bool sel;
        int n = 0;
        float ni = 0;
        foreach (UserCloudItem i in cloud.userCloudItems)
        {
            

            if (i.name == "N_59") //"N_01") //"zzzzz")//"N_59")
            {
                ni = n;

                sel=true;
            }
            else
            {
                sel = false; 
            }


            GameObject copy = AddItem(i,sel);
            n++;
        }

        float h = itemTemplate.GetComponent<RectTransform>().rect.height;
        float sp = content.GetComponent<VerticalLayoutGroup>().spacing;
        float sh = scroll.GetComponent<RectTransform>().rect.height;
        float y = h * ni + h / 2 + sp * ni - sh / 2;

        // content.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, y);

        // Debug.Log(y);

        StartCoroutine(ExampleCoroutine(y));
    }

    IEnumerator ExampleCoroutine(float y)
    {
        float t = 0;
        float ts = 0;
        RectTransform rt = content.GetComponent<RectTransform>();
        float time = 1;
        do
        {
            ts = StaticLib.Smoothed(t, SmoothType.Out);



            rt.anchoredPosition = new Vector2(0, y * ts);

            t += Time.deltaTime / time;
            yield return null;
        } while (t < 1);

        rt.anchoredPosition = new Vector2(0, y);


    }

    private GameObject AddItem(UserCloudItem item, bool selected = false)
    {
        GameObject copy = Instantiate(itemTemplate);

        TextMeshProUGUI tn = copy.transform.Find("Text Name").GetComponent<TextMeshProUGUI>();
        tn.text = item.name;

        TextMeshProUGUI tsc = copy.transform.Find("Text Score").GetComponent<TextMeshProUGUI>();
        tsc.text = item.score.ToString();

        // copy.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 200);
        copy.transform.SetParent(content.transform, false);
        //copy.transform.parent = content.transform;

        if (selected)
        {
            copy.transform.Find("Panel Select").gameObject.SetActive(true);
        }

        return copy;

    }

    public void Close()
    {
        cloud.SignOut();
        Destroy(gameObject);
    }



}

