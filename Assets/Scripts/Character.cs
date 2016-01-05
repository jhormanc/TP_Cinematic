using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    [SerializeField]
    private float DeltaTime;

    private Animator anim;
    private Rigidbody rb;

    // Use this for initialization
    void Awake ()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        anim.SetFloat("Speed", 0.5f);
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * DeltaTime);
    }
}
