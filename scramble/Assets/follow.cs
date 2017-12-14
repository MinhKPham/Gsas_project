using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class follow : MonoBehaviour {
    public float rate = 0.0003f;
    static public int miss = 0;
    public float lifetime = 4f;
    private float life = 0;
    float scale = 300;
    float oscale = 300;
    Vector3 sc;
    SpriteRenderer spr;
    float a;
    public Transform targ;
	void Start () {
        sc= new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
        // Use this for initialization
        spr = GetComponent<SpriteRenderer>();
        a = spr.color.a;
        scale *= Random.Range(1.1f, 1.2f);
    }

    // Update is called once per frame
    void Update () {
       
        
        life += Time.deltaTime;
        
        spr.color = new Vector4(spr.color.r, spr.color.g, spr.color.b, a - life * (a / 6f));
        transform.position += -transform.right * Time.deltaTime*scale*rate;
        
        Vector3 diff = transform.position - new Vector3(0, 0, 0);
        transform.localScale = sc * 4.3f*life;
        if (life >= lifetime) { Destroy(gameObject); }
        scale -=1;
        if (scale < 40) scale = 40;
        //if (diff.magnitude<=20f) { Destroy(gameObject); miss++;  }
            
    }
    
}
