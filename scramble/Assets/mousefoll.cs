using UnityEngine;
using System.Collections;

using System.Collections.Generic;
using UnityEngine.UI;

public class mousefoll : MonoBehaviour {
    List<GameObject> bars;
    public GameObject dot;
    float timer = 0;
    
    // Use this for initialization
    void Start () {
        bars = new List<GameObject>();
        
        
            //GameObject clone = Instantiate(dot, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
            //bars.Add(clone);

        
    }
	
	// Update is called once per frame
	void Update () {
        
        Vector3 temp = Input.mousePosition;
        temp.z = 10f; // Set this to be the distance you want the object to be placed in front of the camera.
        Vector3 diff = Camera.main.ScreenToWorldPoint(temp) - new Vector3(0,0,0);
        float distance = diff.magnitude;
        //if (distance > 50f)
        //{
            //transform.position = new Vector3(0, 0, 0) + (diff / distance) * 50f;
        //}
        //else
        //{
            transform.position = Camera.main.ScreenToWorldPoint(temp);
        //}
        
        
        for (int i = 0; i < bars.Count; i++)
        {
            //bars[i].transform.localScale = new Vector3(bars[i].transform.localScale.x*(i+1)/bars.Count, bars[i].transform.localScale.y*(i+1)/bars.Count, bars[i].transform.localScale.z);
        }
        
    }
}
