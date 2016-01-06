using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour
{
    public bool Over;

    [SerializeField]
    private float DeltaTimeTraveling;

    [SerializeField]
    private float DeltaTimeZoom;

    [SerializeField]
    private float DeltaTimeRotate;

    // Traveling
    private bool traveling;
    private Vector3 target;
    private Transform lookAt;

    // Rotation
    private bool rotation;
    private Vector3 pivotRot;
    private Vector3 targetAngles;

    // Zoom
    private const float MinFov = 5f;
    private float maxFov;
    private float targetFov;
    private bool zooming;

    private Camera cam;
    private CameraManager manager;

    // Use this for initialization
    void Awake ()
    {
        traveling = false;
        cam = GetComponent<Camera>();
        maxFov = cam.fieldOfView - MinFov;
        manager = CameraManager.Instance;
    }
	
	// Update is called once per frame
	void Update ()
    {
	    if(traveling)
        {
            transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * DeltaTimeTraveling);
            transform.LookAt(lookAt);

            if (transform.position == target)
            {
                traveling = false;
                //manager.NextCam(gameObject);
            }
        }

        if(zooming)
        {
            float fov;
            cam.fieldOfView = 1f;
        }

        if(rotation)
        {
            Vector3 angles = Vector3.Lerp(transform.rotation.eulerAngles, targetAngles, Time.deltaTime * DeltaTimeRotate);
            transform.position = RotatePtAround(transform.position, pivotRot, angles);

            if(transform.rotation.eulerAngles == targetAngles)
            {
                rotation = false;
            }
        }

        Over = (!traveling && !zooming && !rotation);
    }

    public void Traveling(Vector3 p1, Vector3 p2, Transform look_at)
    {
        Traveling(p1, p2);
        lookAt = look_at;
    }

    public void Traveling(Vector3 p1, Vector3 p2)
    {
        transform.position = p1;
        target = p2;
        traveling = true;
    }

    public void Zoom(float zoom)
    {
        targetFov = MinFov + maxFov * (1f - Mathf.Clamp01(zoom));
        zooming = true;
    }

    public void RotateCam(Vector3 pivot, Vector3 angles)
    {
        pivotRot = pivot;
        targetAngles = angles;
        rotation = true;
    }

    private Vector3 RotatePtAround(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        // Get point direction relative to pivot
        Vector3 dir = point - pivot;
        // Rotate it
        dir = Quaternion.Euler(angles) * dir;
        // Calculate rotated point
        return dir + pivot;
    }
}
