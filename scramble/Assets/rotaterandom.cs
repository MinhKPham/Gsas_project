using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotaterandom : MonoBehaviour {

    // Use this for initialization
    float rotationsPerMinute = 10.0f;
    int direction = 1;
    float time = 0;
    public bool random = true;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;
        if (time >= 1f) { direction = Random.Range(0, 2);
            
            time = 0;
           
        }
        if (direction == 0) { direction = -1; }
        if (random==false)direction = 1;
        transform.Rotate(Vector3.forward * Time.deltaTime* 150 * direction);
    }
}
