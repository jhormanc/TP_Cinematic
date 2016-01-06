using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraManager : Singleton<CameraManager>
{
    public bool Over { get; private set; }

    [SerializeField]
    private Transform character;

    [SerializeField]
    private Transform traveling_pt1;

    [SerializeField]
    private Transform traveling_pt2;

    [SerializeField]
    private Transform center;

    private Dictionary<string, CameraScript> cams;
    private Sky sky;

    private int camCinematicPos;
    private int camNumber;
    private bool rendering;
    private CameraScript camera;

    // Use this for initialization
    void Awake ()
    {
        sky = FindObjectOfType<Sky>();
        CameraScript[] c = FindObjectsOfType<CameraScript>();
        cams = new Dictionary<string, CameraScript>(c.Length);
        foreach (CameraScript cam in c)
        {
            cam.gameObject.SetActive(false);
            cams.Add(cam.name, cam);
        }
        Over = false;
        camCinematicPos = 0;
        camNumber = 0;
        rendering = false;
        camera = null;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(!rendering && !Over)
        {
            if(camera != null)
            {
                if(camera.Over)
                {
                    if(camNumber == 0)
                    {
                        if (camCinematicPos == 1)
                        {
                            StartCoroutine(StartCam1_2());
                            rendering = true;
                        }
                        else if (camCinematicPos == 2)
                        {
                            StartCoroutine(StartCam1_3());
                            rendering = true;
                        }
                        else
                            NextCam(camera.gameObject);
                    }
                }
            }
            else
            {
                string cam_name = string.Format("Cam{0}", camNumber);
                CameraScript cam;

                if (cams.TryGetValue(cam_name, out cam))
                {
                    camera = cam;
                    camera.gameObject.SetActive(true);
                    if (camNumber == 0)
                    {
                        StartCoroutine(StartCam1_1());
                    }

                    rendering = true;
                }
                else
                    Over = true;
            }
        }
	}

    IEnumerator StartCam1_1()
    {
        camera.Traveling(traveling_pt1.position, traveling_pt2.position);
        float zoom = camera.DeltaTimeZoom;
        yield return new WaitForSeconds(1f);
        camera.DeltaTimeZoom = 2f;
        camera.Zoom(0.3f);
        yield return new WaitForSeconds(0.5f);
        camera.DeltaTimeZoom = 1.8f;
        camera.Zoom(0.1f);
        yield return new WaitForSeconds(1f);
        camera.DeltaTimeZoom = zoom;
        yield return new WaitForSeconds(2f);
        StartCoroutine(StartSunSpeed());
    }

    IEnumerator StartCam1_2()
    {
        camera.LookAtTarget(center);
        yield return new WaitForSeconds(1f);
        camera.RotateCam(center, new Vector3(0f, 360f, 0f));
    }
    IEnumerator StartCam1_3()
    {
        camera.DeltaTimeLook = 1.5f;
        camera.LookAtTarget(character);
        yield return new WaitForSeconds(0.5f);
        camera.Zoom(0.9f);
    }

    IEnumerator StartSunSpeed()
    {
        float speed = sky.Speed;

        for (int i = 0; i < 5; i++)
        {
            sky.Speed *= 2f;
            yield return new WaitForSeconds(i * 0.7f);
        }

        yield return new WaitForSeconds(0.2f);
        sky.Speed = speed;
    }

    private void NextCam(GameObject cam)
    {
        camNumber++;
        camCinematicPos = 0;
        cam.SetActive(false);
        camera = null;
        rendering = false;
    }

    public void EndCam()
    {
        camCinematicPos++;
        rendering = false;
    }
}
