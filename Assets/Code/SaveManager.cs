using System;
using UnityEngine;



public static class SaveManager
{
    private const string GAME_STATE_KEY = "save";

    public static State state = new State();

    public static void Clear()
    {

        PlayerPrefs.DeleteKey(GAME_STATE_KEY);
    }

    public static void Save()
    {

        PlayerPrefs.SetString(GAME_STATE_KEY, JsonUtility.ToJson(state));

        // PlayerPrefs.SetString(GAME_STATE_KEY, state.Serialize());

        SaveCloud();

    }


    public async static void SaveCloud()
    {
        if (state.name == "")
            return;

        try
        {

            Cloud cloud = new Cloud();
            UserCloudItem item;
            item = new UserCloudItem();
            item.name = state.name;
            item.time = state.time;
            item.score = state.score;
            await cloud.SaveData(item);
        }
        catch { }
    }





    public static void Load()
    {

        try
        {
            State load_state = JsonUtility.FromJson<State>(PlayerPrefs.GetString(GAME_STATE_KEY));
            //State load_state = PlayerPrefs.GetString(GAME_STATE_KEY).Deserialize<State>();
            Action<State, State> map = MapperFactory.CreateMapper<State, State>();
            map(load_state, state);
        }
        catch
        {
            Save();
        }

        /*
        if (PlayerPrefs.HasKey(GAME_STATE_KEY))
        {
            State load_state = PlayerPrefs.GetString(GAME_STATE_KEY).Deserialize<State>();
            Action<State, State> map = MapperFactory.CreateMapper<State, State>();
            map(load_state, state);
        }
        else
        {
            Save();
        }
        */

    }
}


/*
public class SaveManager : MonoBehaviour
{
    [HideInInspector] public SaveState state;
    public static SaveManager Instance { set; get; }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Instance = this;
        Load();
    }

    public void Save()
    {
        PlayerPrefs.SetString("save", state.Serialize());
    }

    public void Load()
    {
        if (PlayerPrefs.HasKey("save"))
        {
            state = PlayerPrefs.GetString("save").Deserialize<SaveState>();
        }
        else
        {
            state = new SaveState();
            Save();
        }

    }
}
*/
