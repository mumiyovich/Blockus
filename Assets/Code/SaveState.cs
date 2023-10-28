

using UnityEngine;

[System.Serializable]
public class State
{
    [SerializeField]
    private string _name;
    public string name
    {
        get
        {
            if(_name == null) return "";
            return _name; 
        }
        set
        { 
            _name = value; 
        }
    }

    [SerializeField]
    private int _time;
    public int time { get { return _time; } set { _time = value; } }

    [SerializeField]
    private int _score;
    public int score { get { return _score; } set { _score = value; } }

    [SerializeField]
    private int _score_diff;
    public int score_diff { get { return _score_diff; } set { _score_diff = value; } }

    [SerializeField]
    private int _leven_num;
    public int leven_num { get { return _leven_num; } set { _leven_num = value; } }

    [SerializeField]
    private float _score_add;
    public float score_add { get { return _score_add; } set { _score_add = value; } }

    [SerializeField]
    private int _count_x_block;
    public int count_x_block { get { return _count_x_block; } set { if (value > 0) _count_x_block = value; } }

}

/*
 






*/