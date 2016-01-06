using UnityEngine;
using System.Collections;

public class Sky : MonoBehaviour
{
    public float Speed;
    public bool Day { get; private set; }

    [SerializeField]
    private Transform center;

    private Animator anim;
    private Transform sun;
    private Transform clouds;
    private Transform moon;
    private Light sunLight;
    private Light moonLight;
    private Material skyMaterial;
    private float offsetTextU;
    private Light spot;
    private Light point;

    // Use this for initialization
    void Awake ()
    {
        sun = transform.Find("Sun");
        sunLight = sun.Find("Sun1").Find("Light").GetComponent<Light>();
        clouds = transform.Find("Clouds");
        moon = transform.Find("Moon").Find("Moon1").Find("Light");
        moonLight = moon.GetComponent<Light>();
        spot = GameObject.Find("Spotlight").GetComponent<Light>();
        point = GameObject.Find("Pointlight").GetComponent<Light>();
        anim = GetComponent<Animator>();
        anim.speed = Speed;
        skyMaterial = GetComponent<Renderer>().material;
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        float y_sun = sun.transform.position.y / 331.0067f;
        float u = 0.25f * y_sun;
        Day = u > 0f;
        offsetTextU = 1.25f - u;

        skyMaterial.mainTextureOffset = new Vector2(offsetTextU, 0);
        setMaterial(clouds, skyMaterial);

        if (Day)
        {
            if (moonLight.enabled)
            {
                moonLight.enabled = false;
                spot.enabled = false;
                point.enabled = false;
            }
            if (!sunLight.enabled)
                sunLight.enabled = true;

            sunLight.intensity = y_sun;
            sun.LookAt(center);
        }
        else
        {
            if (sunLight.enabled)
                sunLight.enabled = false;
            if (!moonLight.enabled)
            {
                moonLight.enabled = true;
                spot.enabled = true;
                point.enabled = true;
            }

            moonLight.intensity = Mathf.Abs(y_sun) * 0.2f;
            moon.LookAt(center);
        }

        if(!Mathf.Approximately(Speed, anim.speed))
            anim.speed = Speed;
    }

    void setMaterial(Transform p, Material mat)
    {
        foreach(Transform child in p)
        {
            child.GetComponent<Renderer>().material = mat;
        }
    }
}
