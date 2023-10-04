using System.Collections;
using System.Collections.Generic;
//using Unity.Mathematics;
//using UnityEditor.PackageManager.UI;
//using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class blokus_logo : MonoBehaviour
{

    [SerializeField] private float sinX;
    [SerializeField] private float sinSpeedX;
    [SerializeField] private float sinOffSetX;

    [SerializeField] private float sinY;
    [SerializeField] private float sinSpeedY;
    [SerializeField] private float sinOffSetY;

    [SerializeField] private float sinZ;
    [SerializeField] private float sinSpeedZ;
    [SerializeField] private float sinOffSetZ;

    [SerializeField] private AudioSource sound;

    private int samples_count = 1024;

    private float[] samples;


    [SerializeField] private GameObject visAudio1;
    [SerializeField] private GameObject visAudio2;
    [SerializeField] private GameObject visAudio3;


    private float max1 = 0;
    private float max2 = 0;
    private float max3 = 0;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        //QualitySettings.vSyncCount = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        samples = new float[samples_count];

    }
    


    // Update is called once per frame
    void Update()
    {

        Vector3 v = transform.eulerAngles;

        v.z = Mathf.Sin(Time.time* sinSpeedZ) * sinZ + sinOffSetZ;
        v.x = Mathf.Sin(Time.time * sinSpeedX) * sinX + sinOffSetX;
        v.y = Mathf.Sin(Time.time * sinSpeedY) * sinY + sinOffSetY;

        transform.eulerAngles = v;


        ///////////////
        ///
        float speedAu = 2;


        float vol = 0;

        vol = GetVolume(2);
        Vector3 zs = visAudio1.transform.localScale;
        max1 = Mathf.Max(max1, vol);
        vol = vol* (1.0f/max1);
        if (vol > zs.y)
        {
            zs.y = vol;
        }
        else
        {
            zs.y = Mathf.Max(vol, zs.y - Time.deltaTime * speedAu);
        }
        visAudio1.transform.localScale = zs;

        vol = GetVolume(1);
        zs = visAudio2.transform.localScale;
        max2 = Mathf.Max(max2, vol);
        vol = vol * (1.0f / max2);
        if (vol > zs.y)
        {
            zs.y = vol;
        }
        else
        {
            zs.y = Mathf.Max(vol, zs.y - Time.deltaTime * speedAu);
        }
        visAudio2.transform.localScale = zs;

        vol = GetVolume(0);
        zs = visAudio3.transform.localScale;
        max3 = Mathf.Max(max3, vol);
        vol = vol * (1.0f / max3);
        if (vol > zs.y)
        {
            zs.y = vol;
        }
        else
        {
            zs.y = Mathf.Max(vol, zs.y - Time.deltaTime * speedAu);
        }
        visAudio3.transform.localScale = zs;

        //////////

        /*
        float[] spectrum = new float[256];
        AudioListener.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);
        for (int i = 1; i < spectrum.Length - 1; i++)
        {
            Debug.DrawLine(new Vector3(i - 1, spectrum[i] + 10, 0), new Vector3(i, spectrum[i + 1] + 10, 0), Color.red);
            Debug.DrawLine(new Vector3(i - 1, Mathf.Log(spectrum[i - 1]) + 10, 2), new Vector3(i, Mathf.Log(spectrum[i]) + 10, 2), Color.cyan);
            Debug.DrawLine(new Vector3(Mathf.Log(i - 1), spectrum[i - 1] - 10, 1), new Vector3(Mathf.Log(i), spectrum[i] - 10, 1), Color.green);
            Debug.DrawLine(new Vector3(Mathf.Log(i - 1), Mathf.Log(spectrum[i - 1]), 3), new Vector3(Mathf.Log(i), Mathf.Log(spectrum[i]), 3), Color.blue);
        }
        */


    }

    float GetVolume(int chanel) //https://itnan.ru/post.php?c=1&p=338334
    {
        // sound.GetOutputData(samples, chanel);

        //sound.GetSpectrumData(samples, 0, FFTWindow.Blackman);

        //sound.GetSpectrumData(samples, 0, FFTWindow.Rectangular);

        sound.GetSpectrumData(samples, 0, FFTWindow.Rectangular);

        float a = 0;
       // foreach (float s in samples)

        for (int i = (int)((((float)samples_count) / 3.0f)* (float)chanel); i < (int)((((float)samples_count) / 3.0f) * (float)(1+chanel)); i++)
        {
            a += Mathf.Sqrt(Mathf.Abs(samples[i]));
                //Mathf.Abs(samples[i]) * Mathf.Abs(samples[i]);
        }
        return a / samples_count;
    }
}
