using UnityEngine;
using System.Collections;

public class CameraManager : Singleton<CameraManager>
{
    [SerializeField]
    private Transform character;

    [SerializeField]
    private Transform traveling_pt1;

    [SerializeField]
    private Transform traveling_pt2;

    private CameraScript[] cams;

    // Use this for initialization
    void Awake ()
    {
        cams = FindObjectsOfType<CameraScript>();
        cams[0].Traveling(traveling_pt1.position, traveling_pt2.position, character);
	}
	
	// Update is called once per frame
	void Update ()
    {
        
	}
}
