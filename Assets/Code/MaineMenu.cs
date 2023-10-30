using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;

public class MaineMenu : MonoBehaviour
{

    [SerializeField] private GameObject hiScores;

    [SerializeField] private GameObject EnterName;

    [SerializeField] private TextMeshProUGUI text_name;

    [SerializeField] private GameObject infoPage;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(SystemInfo.deviceUniqueIdentifier);
        DrawName();

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void StartNewGame()
    {
        SaveManager.Clear();
        SceneManager.LoadScene(1);
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene(1);
    }

    public void HiScores()
    {
        if (SaveManager.state.name != "")
        {
            Instantiate(hiScores);
            return;
        }

        GameObject estr = Instantiate(EnterName);
        EnterString escr = estr.GetComponent<EnterString>();
        escr.OnEnter = HiScoresEnterName;
    }

    public void HiScoresEnterName()
    {
        SaveManager.Save();
        DrawName();
        HiScores();
    }

    public void ChangeName()
    {
        GameObject estr = Instantiate(EnterName);
        EnterString escr = estr.GetComponent<EnterString>();
        escr.OnEnter = ChangeEnd;
    }

    public void ChangeEnd()
    {
        SaveManager.Save();
        DrawName();
    }

    void DrawName()
    {
        if (SaveManager.state.name != "")
            text_name.text = SaveManager.state.name;
        else text_name.text = "your name";
    }

    public void InfoPage()
    {

        GameObject new_back = (Instantiate(infoPage) as GameObject);

        Destroy(gameObject);

    }
}
