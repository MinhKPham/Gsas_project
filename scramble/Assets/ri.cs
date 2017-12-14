using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ri : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerStay2D(Collider2D collision)
    {
        transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        if (collision.tag == "mousething")
        {
            Destroy(collision.gameObject);
        }
    }



}
