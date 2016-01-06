using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    [SerializeField]
    private float DeltaTime;

    private Animator anim;
    private Animator moveTargetAnim;
    private Rigidbody rb;

    // Use this for initialization
    void Awake ()
    {
        anim = GetComponent<Animator>();
        moveTargetAnim = GameObject.Find("TargetMove").GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        moveTargetAnim.Stop();
        anim.SetFloat("Speed", 0f);
        anim.SetInteger("Pose", 1);
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * DeltaTime);
    }

    public void StartCharacter()
    {
        moveTargetAnim.Play("TargetMoveAnimation");
        anim.SetInteger("Pose", -1);
        Invoke("SetWalkSpeed", 2f);
        Invoke("SetRunSpeed", 9f);
    }

    private void SetWalkSpeed()
    {
        SetSpeed(0.5f);
    }

    private void SetRunSpeed()
    {
        SetSpeed(2f);
    }

    public void SetSpeed(float speed)
    {
        anim.SetFloat("Speed", speed);
    }
}
