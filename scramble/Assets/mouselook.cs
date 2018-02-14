using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("Camera/Simple Smooth Mouse Look ")]
public class mouselook : MonoBehaviour
{

    int i = 0;
    public Text t;
    public AudioClip[] list;
    Vector2 _mouseAbsolute;
    Vector2 _smoothMouse;
    LineRenderer line;
    public Vector2 clampInDegrees = new Vector2(360, 180);
    public bool lockCursor;
    public Text sen;
    public Vector2 sensitivity = new Vector2(2, 2);
    public Vector2 smoothing = new Vector2(3, 3);
    public Vector2 targetDirection;
    public Vector2 targetCharacterDirection;
    AudioSource ac;
    // Assign this if there's a parent object controlling motion, such as a Character Controller.
    // Yaw rotation will affect this object instead of the camera if set.
    public GameObject characterBody;

    void Start()
    {
        

        line = GetComponent<LineRenderer>();
        ac = GetComponent<AudioSource>();
        ac.clip = list[0];
        ac.Play();
        
        t.text = ac.clip.name;
        // Set target direction to the camera's initial orientation.
        targetDirection = transform.localRotation.eulerAngles;

        // Set target direction for the character body to its inital state.
        if (characterBody)
            targetCharacterDirection = characterBody.transform.localRotation.eulerAngles;
    }

    void Update()
    {
        
        sensitivity.y = sensitivity.x;
        //print("DDDds");
        if (Input.GetKeyDown(KeyCode.Space))
        {
            print("DDD");
            i++;
            if (i >= list.Length) i = 0;
            ac.clip = list[i];
            ac.Play();
            t.text = ac.clip.name;
        }
            

        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        if (Input.GetKey("right"))
            sensitivity+=new Vector2(0.002f,0.002f);

        else if (Input.GetKey("left"))
            sensitivity -= new Vector2(0.002f, 0.002f);
        if (sensitivity.x <= 0f) sensitivity = new Vector2(0f, 0f);
        if (sensitivity.x >= 10f) sensitivity = new Vector2(10f, 10f);
        sen.text = "mouse sen " + sensitivity.x;
        RaycastHit hit;
        line.enabled = false;
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            print("Found an object - distance: " + hit.collider.gameObject.name);
            if (Input.GetMouseButton(0))
            {
                line.enabled = true;
                line.SetPosition(0, transform.position + transform.up * -5);
                line.SetPosition(1, hit.point);
                Destroy(hit.collider.gameObject);
                line.startWidth = 0.7f;
                line.endWidth = line.startWidth;
            }
            
        }

        // Allow the script to clamp based on a desired target value.
        var targetOrientation = Quaternion.Euler(targetDirection);
        var targetCharacterOrientation = Quaternion.Euler(targetCharacterDirection);

        // Get raw mouse input for a cleaner reading on more sensitive mice.
        var mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        // Scale input against the sensitivity setting and multiply that against the smoothing value.
        mouseDelta = Vector2.Scale(mouseDelta, new Vector2(sensitivity.x * smoothing.x, sensitivity.y * smoothing.y));

        // Interpolate mouse movement over time to apply smoothing delta.
        _smoothMouse.x = Mathf.Lerp(_smoothMouse.x, mouseDelta.x, 1f / smoothing.x);
        _smoothMouse.y = Mathf.Lerp(_smoothMouse.y, mouseDelta.y, 1f / smoothing.y);
        _smoothMouse.x = mouseDelta.x;
        _smoothMouse.y = mouseDelta.y;
        // Find the absolute mouse movement value from point zero.
        _mouseAbsolute += _smoothMouse;

        // Clamp and apply the local x value first, so as not to be affected by world transforms.
        if (clampInDegrees.x < 360)
            _mouseAbsolute.x = Mathf.Clamp(_mouseAbsolute.x, -clampInDegrees.x * 0.5f, clampInDegrees.x * 0.5f);

        // Then clamp and apply the global y value.
        if (clampInDegrees.y < 360)
            _mouseAbsolute.y = Mathf.Clamp(_mouseAbsolute.y, -clampInDegrees.y * 0.5f, clampInDegrees.y * 0.5f);

        transform.localRotation = Quaternion.AngleAxis(-_mouseAbsolute.y, targetOrientation * Vector3.right) * targetOrientation;

        // If there's a character body that acts as a parent to the camera
        if (characterBody)
        {
            var yRotation = Quaternion.AngleAxis(_mouseAbsolute.x, Vector3.up);
            characterBody.transform.localRotation = yRotation * targetCharacterOrientation;
        }
        else
        {
            var yRotation = Quaternion.AngleAxis(_mouseAbsolute.x, transform.InverseTransformDirection(Vector3.up));
            transform.localRotation *= yRotation;
        }
    }
}

