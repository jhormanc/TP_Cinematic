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
            if(camera == null)
            {
                string cam_name = string.Format("Cam{0}", camNumber);
                CameraScript cam;

                if (cams.TryGetValue(cam_name, out cam))
                {
                    camera = cam;
                    camera.gameObject.SetActive(true);
                    if (camNumber == 0)
                    {
                        camera.Traveling(traveling_pt1.position, traveling_pt2.position);
                    }

                    rendering = true;
                }
                else
                    Over = true;
            }
        }
	}

    public void NextCam(GameObject cam)
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
