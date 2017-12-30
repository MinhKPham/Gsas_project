using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class follow : MonoBehaviour {
    public float rate = 0.0003f;
    static public int miss = 0;
    public float lifetime = 4f;
    public float fix = 0.4f;
    private float life = 0;
    float scale = 300;
    float oscale = 300;
    public bool slower=false;
    public bool larger = false;
    public bool inward = false;
    public bool fixedrate = true;
    Vector3 sc;
    SpriteRenderer spr;
    float a;
    public Transform targ;
	void Start () {
        sc= new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
        // Use this for initialization
        spr = GetComponent<SpriteRenderer>();
        a = spr.color.a;
        scale *= 1.1f;
    }

    // Update is called once per frame
    void Update () {
        if (fixedrate) rate = fix;
        float inw= 1;
        if (inward) inw = -1;
        life += Time.deltaTime;
        
        spr.color = new Vector4(spr.color.r, spr.color.g, spr.color.b, a - life * (a / 6f));
        transform.position += -transform.right * Time.deltaTime*scale*rate*inw;
        
        Vector3 diff = transform.position - new Vector3(0, 0, 0);
        if (larger) transform.localScale = sc * 4.3f*life*2f;
        if (life >= lifetime) { Destroy(gameObject); }
        if (slower)scale -=1;
        
        if (scale < 40) scale = 40;
        //if (diff.magnitude<=20f) { Destroy(gameObject); miss++;  }
            
    }
    
}
