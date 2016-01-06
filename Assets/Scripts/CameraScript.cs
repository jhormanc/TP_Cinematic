using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour
{
    public bool Over;
    public float DeltaTimeTraveling;
    public float DeltaTimeZoom;
    public float DeltaTimeRotate;
    public float DeltaTimeLook;

    // Traveling
    private bool traveling;
    private Vector3 target;
    private Transform lookAt;

    // Rotation
    private bool rotation;
    private Vector3 pivotRot;
    private Vector3 targetAngles;
    private Vector3 currentAngle;

    // Zoom
    private const float MinFov = 5f;
    private float maxFov;
    private float targetFov;
    private bool zooming;

    // Look at
    private Vector3 targetLookAt;
    private bool looking;

    // Wait
    private float waitTime;
    private float targetWaitTime;
    private bool waiting;

    private Camera cam;
    private CameraManager manager;

    // Use this for initialization
    void Awake ()
    {
        cam = GetComponent<Camera>();
        maxFov = cam.fieldOfView - MinFov;
        manager = CameraManager.Instance;
        looking = false;
        zooming = false;
        traveling = false;
        rotation = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
        bool oldOver = Over;

	    if(traveling)
        {
            transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * DeltaTimeTraveling);
            

            if (Vector3.Distance(transform.position, target) < 10f)
                traveling = false;
        }

        if(zooming)
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFov, Time.deltaTime * DeltaTimeZoom);
            if (Mathf.Abs(cam.fieldOfView - targetFov) < 2f)
                zooming = false;
        }

        if(rotation)
        {
            Vector3 angle = Vector3.Lerp(Vector3.zero, targetAngles, Time.deltaTime * DeltaTimeRotate);
            transform.position = RotatePtAround(transform.position, pivotRot, angle);
            currentAngle += angle;

            transform.LookAt(lookAt);
            if (Mathf.Abs((targetAngles - currentAngle).sqrMagnitude) < 10f)
                rotation = false;
        }

        if(looking)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetLookAt - transform.position);
            
            // Smoothly rotate towards the target point.
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * DeltaTimeLook);

            if (Quaternion.Angle(transform.rotation, targetRotation) < 1f)
                looking = false;
        }

        if(waiting)
        {
            waitTime += Time.deltaTime;
            if (waitTime > targetWaitTime)
                waiting = false;
        }

        Over = (!traveling && !zooming && !rotation && !looking && !waiting);

        if(!oldOver && Over)
            manager.EndCam();
    }

    public void Traveling(Vector3 from, Vector3 to, Transform look_at)
    {
        Traveling(from, to);
        lookAt = look_at;
    }

    public void Traveling(Vector3 to, Transform look_at)
    {
        Traveling(to);
        lookAt = look_at;
    }

    public void Traveling(Vector3 from, Vector3 to)
    {
        transform.position = from;
        Traveling(to);
    }

    public void Traveling(Vector3 to)
    {
        target = to;
        lookAt = null;
        traveling = true;
    }

    public void Zoom(float zoom)
    {
        targetFov = MinFov + maxFov * (1f - Mathf.Clamp01(zoom));
        zooming = true;
    }

    public void RotateCam(Transform pivot, Vector3 angles)
    {
        pivotRot = pivot.position;
        targetAngles = angles;
        currentAngle = Vector3.zero;
        lookAt = pivot;
        rotation = true;
    }

    public void LookAtTarget(Vector3 target)
    {
        targetLookAt = target;
        looking = true;
    }

    public void WaitCam(float timeToWait)
    {
        waitTime = 0f;
        targetWaitTime = timeToWait;
        waiting = true;
    }

    private Vector3 RotatePtAround(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        // Get point direction relative to pivot
        Vector3 dir = point - pivot;
        // Rotate it
        dir = Quaternion.Euler(angles) * dir;
        return dir + pivot;
    }
}
