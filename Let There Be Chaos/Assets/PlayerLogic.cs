using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    Rigidbody2D rb;

    public Transform cam;
    Vector3 cameraVelocity;
    public Vector3 cameraOffset;
    public float CameraFollowSmoothTime;

    public float SideForce;
    public float MaxSideVelocity;
    public float JumpImpulse;

    int bodyContacts;

    // Start is called before the first frame update
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        bodyContacts = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, JumpImpulse);
        }
    }

    private void FixedUpdate()
    {
        Vector3.SmoothDamp(cam.position, transform.position + cameraOffset, ref cameraVelocity, CameraFollowSmoothTime);
        cam.position = cam.position + cameraVelocity * Time.deltaTime;

        //if (IsGrounded())
        //    rb.velocity = new Vector2(MaxSideVelocity, rb.velocity.y);
        //else
        //    rb.velocity = new Vector2(MaxSideVelocity * 1f, rb.velocity.y);

        if (Input.GetKey(KeyCode.A))
        {
            //rb.AddForce(Vector2.left * SideForce * Time.deltaTime);
            if (IsGrounded())
                rb.velocity = new Vector2(-MaxSideVelocity, rb.velocity.y);
            else
                rb.velocity = new Vector2(-MaxSideVelocity * 1f, rb.velocity.y);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            //rb.AddForce(Vector2.right * SideForce * Time.deltaTime);
            if (IsGrounded())
                rb.velocity = new Vector2(MaxSideVelocity, rb.velocity.y);
            else
                rb.velocity = new Vector2(MaxSideVelocity * 1f, rb.velocity.y);
        }
        else
        {
            if (IsGrounded())
                rb.velocity = new Vector2(0, rb.velocity.y);
            else
                rb.velocity = new Vector2(rb.velocity.x - rb.velocity.x * Time.deltaTime * 2f, rb.velocity.y);
        }



        //rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -MaxSideVelocity, MaxSideVelocity), rb.velocity.y);
    }


    private bool IsGrounded()
    {
        return bodyContacts > 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Platform"))
        {
            bodyContacts++;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Platform"))
        {
            bodyContacts--;
        }
    }

}
