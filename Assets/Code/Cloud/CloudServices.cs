
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



//https://docs.unity.com/ugs/en-us/manual/cloud-save/manual/tutorials/unity-sdk-sample
//https://docs.unity.com/ugs/en-us/manual/authentication/manual/platform-signin-username-password

public class CloudServices : MonoBehaviour
{

    [SerializeField]
    private bool admin;

    [SerializeField] private TextMeshProUGUI text_tmp;


    [SerializeField]
    private GameObject butSortScore;
    [SerializeField]
    private GameObject butSortTime;
    [SerializeField]
    private GameObject butSortRating;


    [SerializeField]
    private GameObject itemTemplate;
    [SerializeField]
    private GameObject content;
    [SerializeField]
    private GameObject wait;

    [SerializeField]
    private GameObject scroll;

    private Cloud cloud = new Cloud();

    private float _pos_sel = 0;


    private void Awake()
    {

    }

    // Start is called before the first frame update
    async void Start()
    {
        //await cloud.ClearData();
        // await cloud.GetData(OnGetData);

        /*
         UserCloudItem item;
         for (int i = 0; i < 99; i++)
         {
             item = new UserCloudItem();
             item.name = "Name " + (i.ToString("D2"));
             item.time = (int)((float)(i*3+1)/100.0f * 7200.0f);
             item.score = (i+1)*10;
             await cloud.SaveData(item);
         }
         //*/

        await cloud.GetData(OnGetData);



    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Close();
        }
    }

    private void OnGetData()
    {
        GetData(cloud.userItemsRating);
    }

    private void GetData(List<UserCloudItem> list)
    {

        foreach (Transform t in content.transform)
        {
            Destroy(t.gameObject);
        }


        bool sel;
        int n = 0;
        float ni = -1;
        foreach (UserCloudItem i in list)
        {
            if (i.name == SaveManager.state.name && i.id == StaticLib.playerId_dev)
            {
                ni = n;
                sel = true;
            }
            else
            {
                sel = false;
            }
            GameObject copy = AddItem(i, n + 1, sel);
            n++;
        }

        if (ni == -1)
        {
            _pos_sel = -1;
        }
        else
        {
            float h = itemTemplate.GetComponent<RectTransform>().rect.height;
            float sp = content.GetComponent<VerticalLayoutGroup>().spacing;
            float sh = scroll.GetComponent<RectTransform>().rect.height;
            float y = h * ni + h / 2 + sp * ni - sh / 2;
            float m = h * (n - 1) + h + sp * (n - 1) - sh;

            y = Mathf.Min(y, m);
            y = Mathf.Max(y, 0);

            _pos_sel = y;
        }
        wait.SetActive(false);

        FindYouname();
    }

    IEnumerator MoveSel()
    {



        float t = 0;

        RectTransform rt = content.GetComponent<RectTransform>();
        float time = 1;

        float start = rt.anchoredPosition.y;

        do
        {
            rt.anchoredPosition = new Vector2(0, StaticLib.SmoothedLerp(start, _pos_sel, t, SmoothType.Out));
            t += Time.deltaTime / time;
            yield return null;
        } while (t < 1);

        rt.anchoredPosition = new Vector2(0, _pos_sel);


    }

    public void FindYouname()
    {
        if (_pos_sel == -1)
            return;
        StartCoroutine(MoveSel());
    }

    private GameObject AddItem(UserCloudItem item, int num, bool selected = false)
    {


       
        //StaticLib.TimeSecToStr(state.time)
        GameObject copy = Instantiate(itemTemplate);

        copy.GetComponentInChildren<ButtunDelCloudItem>().item = item;

        copy.transform.Find("ButtonDel").gameObject.SetActive(admin);


        TextMeshProUGUI tnum = copy.transform.Find("Text N").GetComponent<TextMeshProUGUI>();
        tnum.text = num.ToString("D2");

        TextMeshProUGUI tn = copy.transform.Find("Text Name").GetComponent<TextMeshProUGUI>();
        tn.text = item.name;

        TextMeshProUGUI tsc = copy.transform.Find("Text Score").GetComponent<TextMeshProUGUI>();

        int k = (int)item.GetRating();

        tsc.text = item.score.ToString() + "\n" + StaticLib.TimeSecToStr(item.time) + "\n" + k.ToString();

        copy.transform.SetParent(content.transform, false);

        if (selected)
        {
            copy.transform.Find("Panel Select").gameObject.SetActive(true);
        }

        return copy;

    }

    public void Close()
    {

        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        cloud.SignOut();
    }

    GameObject bSortOld = null;
    public void SortClick(GameObject b)
    {
        if (bSortOld == null)
            bSortOld = butSortRating;

        if (b == bSortOld)
        {
            FindYouname();
            return;
        }

        bSortOld = b;

        butSortScore.GetComponent<Image>().color = new Color(0.7f, 0.7f, 0.7f, 1);
        butSortTime.GetComponent<Image>().color = new Color(0.7f, 0.7f, 0.7f, 1);
        butSortRating.GetComponent<Image>().color = new Color(0.7f, 0.7f, 0.7f, 1);

        b.GetComponent<Image>().color = new Color(1, 1, 1, 1);

        if (b == butSortScore)
            GetData(cloud.userItemsScore);
        else
            if (b == butSortTime)
            GetData(cloud.userItemsTime);
        else
            if (b == butSortRating)
            GetData(cloud.userItemsRating);


    }



}

