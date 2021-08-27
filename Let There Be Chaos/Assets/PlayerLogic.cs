using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    private Rigidbody2D rb;
    private float airtime;
    private float lastJumpPress;

    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Vector3 cameraOffset;
    [SerializeField] private float CameraFollowSmoothTime;
    private Vector3 cameraVelocity;

    [SerializeField] private float SideForce;
    [SerializeField] private float MaxSideVelocity;
    [SerializeField] private float JumpImpulse;
    [SerializeField] private float JumpBuffer;
    [SerializeField] private float CoyoteTime;

    private int bodyContacts;

    

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        bodyContacts = 0;
    }

    private bool IsGrounded()
    {
        return bodyContacts > 0;
    }

    private void CheckJump()
    {
        if (lastJumpPress <= JumpBuffer && airtime <= CoyoteTime)
            rb.velocity = new Vector2(rb.velocity.x, JumpImpulse);
    }

    private void Update()
    {
        if (IsGrounded())   airtime = 0;
        else                airtime += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space))    lastJumpPress = 0;
        else                                    lastJumpPress += Time.deltaTime;

        CheckJump();
    }

    private void SmoothCameraFollow()
    {
        Vector3.SmoothDamp(cameraTransform.position, transform.position + cameraOffset, ref cameraVelocity, CameraFollowSmoothTime);
        cameraTransform.position = cameraTransform.position + cameraVelocity * Time.deltaTime;
    }

    private void LookTowards(int direction)
    {
        float scaleX = direction * Mathf.Abs(transform.localScale.x);
        transform.localScale = new Vector3(scaleX, transform.localScale.y, 1);
    }

    private void MoveTowards(int direction)
    {
        LookTowards(direction);
        rb.velocity = new Vector2(direction * MaxSideVelocity, rb.velocity.y);
    }

    private void AddFrictionForce()
    {
        if (IsGrounded())
            rb.velocity = new Vector2(0, rb.velocity.y);
        else
            rb.velocity = new Vector2(rb.velocity.x - rb.velocity.x * Time.deltaTime * 2f, rb.velocity.y);
    }

    private void FixedUpdate()
    {
        SmoothCameraFollow();


        const int SIDE_LEFT  = -1;
        const int SIDE_RIGHT = +1;

        if (Input.GetKey(KeyCode.A))
        {
            MoveTowards(SIDE_LEFT);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            MoveTowards(SIDE_RIGHT);
        }
        else {
            AddFrictionForce();
        }
    }

    private bool IsPlatform(Collision2D collision)
    {
        return collision.collider.CompareTag("Platform");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsPlatform(collision)) bodyContacts++;
    }
    
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (IsPlatform(collision)) bodyContacts--;
    }




    public void Collects(CollectibleLogic.CType ctype, int value)
    {
        if (ctype==CollectibleLogic.CType.COIN)
            GameManager.instance.AddScore(value);
        Debug.Log("coin " + value);
    }
}
