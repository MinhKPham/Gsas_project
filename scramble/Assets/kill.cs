using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kill : MonoBehaviour {
    static public int destroy = 0;
    static public float prox = 0;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "enemy")
        {
            destroy++;
            
            Destroy(collision.gameObject);
        }
    }
}
