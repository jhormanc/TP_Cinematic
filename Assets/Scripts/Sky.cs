using UnityEngine;
using System.Collections;

public class Sky : MonoBehaviour
{
    public float Speed;

    [SerializeField]
    private Transform center;

    private Animator anim;
    private Transform sun;
    private Material skyMaterial;
    private float offsetTextU;

    // Use this for initialization
    void Awake ()
    {
        sun = transform.Find("Sun");
        anim = GetComponent<Animator>();
        anim.speed = Speed;
        skyMaterial = GetComponent<Renderer>().material;
        offsetTextU = skyMaterial.mainTextureOffset.x;
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        sun.LookAt(center);
        float u = sun.transform.position.y / 22f;
        offsetTextU += Speed * 0.0007f;
        skyMaterial.mainTextureOffset = new Vector2(offsetTextU, 0);
    }
}
