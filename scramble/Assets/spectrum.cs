using UnityEngine;
using System.Collections;

using System.Collections.Generic;
using UnityEngine.UI;

[RequireComponent(typeof(LineRenderer))]
public class spectrum : MonoBehaviour
{
    //framerate
    private float Ra=-1f;
    public float beatcooldown=0;
    public bool beat = false;
    public Text t;
    int m_frameCounter = 0;
    float m_timeCounter = 0.0f;
    float m_lastFramerate = 0.0f;
    public float m_refreshTime = 0.5f;
    public int limit = 0;
    private float beattime = 0;
    public GameObject parent;
    public float radius = 600f;
    private float inout = 0.004f;
    public Text text;
    public GameObject thing;
    public float size = 10.0f;
    public GameObject target;
    public float amplitude = 1.0f;
    public int cutoffSample = 128; //MUST BE LOWER THAN SAMPLE SIZE
    public FFTWindow fftWindow;
    public GameObject bar;
    private float scale = 0.3f;
    //public GameObject top;
    List<GameObject> bars;
    List<Vector3> oriscales;
    //List<GameObject> bars2;
    //List<GameObject> tops;
    private float[] samples = new float[1024]; //MUST BE A POWER OF TWO
    private float[] samples01 = new float[1024]; //MUST BE A POWER OF TWO
    private LineRenderer lineRenderer;
    private float stepSize;
    public float cool = 0.4f;
    float timer = 0;
    private float[] cooldown = new float[1024];
    void Start()
    {
        print(AudioSettings.outputSampleRate);

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetVertexCount(cutoffSample);
        stepSize = size / cutoffSample;
        bars = new List<GameObject>();
        oriscales = new List<Vector3>();
        //bars2 = new List<GameObject>();
        //tops = new List<GameObject>();
        for (int i = 0; i < cutoffSample; i++)
        {
            //bars.Add(Instantiate(bar, new Vector3(i * bar.transform.localScale.x * 1.2f - cutoffSample * bar.transform.localScale.x * 1.2f*0.5f, 5, 0), Quaternion.identity));
            //tops.Add(Instantiate(top, new Vector3(i * top.transform.localScale.x * 1.2f - size / 2, 10, 0), Quaternion.identity));
            //bars2.Add(Instantiate(bar, new Vector3(i * bar.transform.localScale.x * 1.2f - size / 2, 5, 40), Quaternion.identity));
            cooldown[i] = 0.5f;
            Vector3 pos;
            Vector3 center = new Vector3(0, 0,0);
            pos.x = center.x + radius * Mathf.Sin(360.0f/cutoffSample*i * Mathf.Deg2Rad);
            pos.y = center.y + radius * Mathf.Cos(360.0f/cutoffSample*i * Mathf.Deg2Rad);
            pos.z = 0;

            bars.Add(Instantiate(bar, new Vector3(pos.x,pos.y,pos.z), Quaternion.identity));
        }
        for (int i = 0; i < cutoffSample; i++)
        {
            Vector2 dir = target.transform.position - bars[i].transform.position;
            float angle= Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            bars[i].transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            oriscales.Add( bars[i].transform.localScale);
            bars[i].transform.parent = parent.transform;
        }
        AudioListener.GetSpectrumData(samples01, 0, fftWindow);
        AudioProcessor processor = FindObjectOfType<AudioProcessor>();
        processor.onBeat.AddListener(onOnbeatDetected);
        

    }
    void onOnbeatDetected()
    {
        //print("beat");
        beat = true;

    }

    // Update is called once per frame
    void Update()
    {
       
        beattime += Time.deltaTime;
        
        if (m_timeCounter < m_refreshTime)
        {
            m_timeCounter += Time.deltaTime;
            m_frameCounter++;
        }
        else
        {
            //This code will break if you set your m_refreshTime to 0, which makes no sense.
            m_lastFramerate = (float)m_frameCounter / m_timeCounter;
            m_frameCounter = 0;
            m_timeCounter = 0.0f;
        }
        
        if (scale >= 0.4f) { inout = -0.004f; }
        if (scale <= 0.04f) { inout = 0.004f; }
        scale += inout;
        parent.transform.localScale = new Vector3(scale, scale, 1);
        for (int x = 0; x < cutoffSample; x++)
        {
            //bars.Add(Instantiate(bar, new Vector3(i * bar.transform.localScale.x * 1.2f - cutoffSample * bar.transform.localScale.x * 1.2f*0.5f, 5, 0), Quaternion.identity));
            //tops.Add(Instantiate(top, new Vector3(i * top.transform.localScale.x * 1.2f - size / 2, 10, 0), Quaternion.identity));
            //bars2.Add(Instantiate(bar, new Vector3(i * bar.transform.localScale.x * 1.2f - size / 2, 5, 40), Quaternion.identity));
            cooldown[x] += Time.deltaTime;
            
            if (cooldown[x] >= cool)
            {
                cooldown[x] = cool;
            }
            
        }
        
        timer += Time.deltaTime;
        bool reset = false;
        int i = 0;
        float max = 0;
        AudioListener.GetSpectrumData(samples, 0, fftWindow);
        List<float> segs = new List<float>();
       
        float t = 0;
        
        int spawn = 0;
        for (i = 0; i < cutoffSample; i++)
        {
            
            int j = i;
            float tx = bars[i].transform.position.x;
            float ty = bars[i].transform.position.y;
            
            float tz = bars[i].transform.position.z;
            float resultor = samples[i] * amplitude;
            Vector3 position = new Vector3(i * stepSize - size / 2, resultor, 0.0f);
            //if (samples[i] * amplitude > max) max = samples[i] * amplitude;
            float result = (samples[i] - samples01[i]) * amplitude;
            result = samples[i] * amplitude;
            if (result >= 6) result = 6;
            //print(result);
            //if (result < 1) result = 1;
            if (i>0 && samples[i - 1] * amplitude <= result && samples[i + 1] * amplitude <= result &&
                result > 1f &&
                beat && beattime >= beatcooldown &&
                spawn < limit && cooldown[i] == cool)
            {

                GameObject shoot = Instantiate(thing, new Vector3(tx, ty, tz+150), Quaternion.identity);
                shoot.GetComponent<follow>().rate *= 1.5f * result;
                shoot.transform.rotation = bars[i].transform.rotation;
                cooldown[i] = 0;
                reset = true;
                i += 8;
                spawn++;
            }

            if (i==0 && samples[i + 1] * amplitude <= result &&
                result > 1f &&
                beat && beattime>=beatcooldown && 
                spawn<limit && cooldown[i]==cool) {
                
                GameObject shoot = Instantiate(thing, new Vector3(tx,ty,tz+150), Quaternion.identity);
                shoot.GetComponent<follow>().rate *= 1.5f*result;
                shoot.transform.rotation = bars[i].transform.rotation;
                cooldown[i] = 0;
                reset = true;
                i += 8;
                spawn++;
            }

            

            //bars2[i].transform.localScale = new Vector3(bars2[i].transform.localScale.x, result * 10, bars2[i].transform.localScale.z);
            //float final = oriscales[i].x * resultor;
            //if (final <= 5f) final = 5f;
            //bars[i].transform.localScale = new Vector3(final, bars[i].transform.localScale.y, bars[i].transform.localScale.z);
            //bars[i].GetComponent<addforce>().force = result * 200;
            
            
            
           
        }
        for (i = 0; i < cutoffSample; i++)
        {
            
            int j = i;
            float tx = bars[i].transform.position.x;
            float ty = bars[i].transform.position.y;

            float tz = bars[i].transform.position.z;
            float resultor = samples[i] * amplitude;
            Vector3 position = new Vector3(i * stepSize - size / 2, resultor, 0.0f);
            //if (samples[i] * amplitude > max) max = samples[i] * amplitude;
            float result = (samples[i] - samples01[i]) * amplitude;
            //result = samples[i] * amplitude;
            //print(result);
            //if (result < 1) result = 1;

            
            //bars2[i].transform.localScale = new Vector3(bars2[i].transform.localScale.x, result * 10, bars2[i].transform.localScale.z);
            float final = oriscales[i].x * resultor;
            if (final <= 5f) final = 5f;
            bars[i].transform.localScale = new Vector3(final, bars[i].transform.localScale.y, bars[i].transform.localScale.z);
            //bars[i].GetComponent<addforce>().force = result * 200;
            lineRenderer.SetPosition(i, position);
            i = j;

        }

        if (reset) { timer = 0; beattime = 0; }
        

        text.text= kill.prox + "___________" + m_lastFramerate;
        System.Array.Copy(samples,samples01,1024);
        beat = false;

    }
}
