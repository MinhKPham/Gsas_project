using UnityEngine;
using System.Collections;

public class sc : MonoBehaviour
{
    public int i = 0;
    void Start()
    {
        Color col1 = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1.0f);
        gameObject.GetComponent<Renderer>().material.color = col1;
    }
    
}
