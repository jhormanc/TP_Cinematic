using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour
{
    [SerializeField]
    private float DeltaTime;

    private bool traveling;

    private Vector3 target;
    private Transform lookAt;

	// Use this for initialization
	void Awake ()
    {
        traveling = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
	    if(traveling)
        {
            transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * DeltaTime);
            transform.LookAt(lookAt);

            if (transform.position == target)
                traveling = false;
        }
	}

    public void Traveling(Vector3 p1, Vector3 p2, Transform look_at)
    {
        traveling = true;

        transform.position = p1;
        target = p2;
        lookAt = look_at;
    }
}
