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
    private CameraScript currentCamera;
    private Camera oldCam;
    private Character characterScript;

    // Use this for initialization
    void Awake ()
    {
        characterScript = character.GetComponent<Character>();
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
        currentCamera = null;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(!rendering && !Over)
        {
            if(currentCamera != null)
            {
                if(currentCamera.Over)
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
                            NextCam();
                    }
                }
            }
            else
            {
                string cam_name = string.Format("Cam{0}", camNumber);
                CameraScript cam;

                if (cams.TryGetValue(cam_name, out cam))
                {
                    if(oldCam != null)
                    {
                        oldCam.enabled = false;
                        oldCam.gameObject.SetActive(false);
                    }
                    currentCamera = cam;
                    currentCamera.gameObject.SetActive(true);
                    currentCamera.GetComponent<Camera>().enabled = true;
                    if (camNumber == 0)
                    {
                        StartCoroutine(StartCam1_1());
                    }
                    else if(camNumber == 1)
                    {

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
        currentCamera.Traveling(traveling_pt1.position, traveling_pt2.position);
        float zoom = currentCamera.DeltaTimeZoom;
        yield return new WaitForSeconds(1f);
        currentCamera.DeltaTimeZoom = 2f;
        currentCamera.Zoom(0.3f);
        yield return new WaitForSeconds(0.5f);
        currentCamera.DeltaTimeZoom = 1.8f;
        currentCamera.Zoom(0.1f);
        yield return new WaitForSeconds(1f);
        currentCamera.DeltaTimeZoom = zoom;
        yield return new WaitForSeconds(2f);
        StartCoroutine(StartSunSpeed());
    }

    IEnumerator StartCam1_2()
    {
        currentCamera.LookAtTarget(center.position);
        yield return new WaitForSeconds(1f);
        currentCamera.RotateCam(center, new Vector3(0f, 360f, 0f));
    }
    IEnumerator StartCam1_3()
    {
        currentCamera.DeltaTimeLook = 1.5f;
        currentCamera.LookAtTarget(character.position);
        yield return new WaitForSeconds(0.5f);
        currentCamera.Zoom(0.9f);
        characterScript.StartCharacter();
        currentCamera.WaitCam(10f);
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

    IEnumerator StartCam2_1()
    {
        currentCamera.DeltaTimeLook = 1.5f;
        currentCamera.LookAtTarget(character.position + character.forward * 2f);
        currentCamera.WaitCam(30f);
        yield return new WaitForSeconds(0.5f);
    }

    private void NextCam()
    {
        camNumber++;
        camCinematicPos = 0;
        oldCam = currentCamera.GetComponent<Camera>();
        currentCamera = null;
        rendering = false;
    }

    public void EndCam()
    {
        camCinematicPos++;
        rendering = false;
    }
}
