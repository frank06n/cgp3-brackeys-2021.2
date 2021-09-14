using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    private Rigidbody2D rb;
    private float airtime;
    private float lastJumpPress;
    private float lastJumpDone;

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

    private PlayerGunLogic PlayerGun;
    private HealthBarScript HealthBar;
    private float healthPoints;
    [SerializeField] private float MaxHealthPoints;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        PlayerGun = GetComponent<PlayerGunLogic>();
        HealthBar = GetComponentInChildren<HealthBarScript>();

        healthPoints = MaxHealthPoints;
        bodyContacts = 0;
        lastJumpPress = airtime = 100; // Random big junk value (to avoid spawn jump)

        StartCoroutine(WalkSound());
    }

    private bool IsGrounded()
    {
        return bodyContacts > 0;
    }

    private void CheckJump()
    {
        if (lastJumpPress <= JumpBuffer && airtime <= CoyoteTime)
        {
            rb.velocity = new Vector2(rb.velocity.x, JumpImpulse);
            SceneManager2.instance.sfxPlayer.Play("player_jump");
            lastJumpDone = 0;
        }
    }

    private void Update()
    {
        if (LevelManager.instance.gameIsOver) return;

        if (transform.position.y < -20f) Die();

        if (IsGrounded())   airtime = 0;
        else                airtime += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space))    lastJumpPress = 0;
        else                                    lastJumpPress += Time.deltaTime;

        if (lastJumpDone > 0.5f)    CheckJump();
        else                        lastJumpDone += Time.deltaTime;

        if (Input.GetMouseButtonDown(0)) PlayerGun.FirePress();
    }
    
    private IEnumerator WalkSound()
    {
        yield return new WaitForSecondsRealtime(0.5f);

        SfxPlayer sfx = SceneManager2.instance.sfxPlayer;
        string name = "player_walk";
        AudioSource source = sfx.GetSource(name);
        source.loop = true;
        source.Play();

        while (!LevelManager.instance.gameIsOver)
        {
            if (IsGrounded() && Mathf.Abs(rb.velocity.x) >= 1f)
                source.volume = sfx.ValidateVolume(1f, name);
            else
                source.volume = sfx.ValidateVolume(0f, name);
            yield return new WaitForSeconds(.1f);
        }
        source.Stop();
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

        float hb_scaleX = direction * Mathf.Abs(HealthBar.transform.localScale.x);
        HealthBar.transform.localScale = new Vector3(hb_scaleX, HealthBar.transform.localScale.y, 1);
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
        if (LevelManager.instance.gameIsOver || LevelManager.instance.gameIsPaused) return;
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

    private bool IsPlatformTopEdge(Collision2D collision)
    {
        return collision.collider.CompareTag("PlatformTopEdge");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsPlatformTopEdge(collision)) bodyContacts++;
    }
    
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (IsPlatformTopEdge(collision)) bodyContacts--;
    }

    public void Freeze()
    {
        rb.isKinematic = true;
        rb.velocity = Vector2.zero;
    }

    public void Collects(CollectibleLogic.CType ctype, int value)
    {
        switch (ctype)
        {
            case CollectibleLogic.CType.COIN:
                LevelManager.instance.AddScore(value);
                SceneManager2.instance.sfxPlayer.Play("player_collect_coin");
                break;

            case CollectibleLogic.CType.MEMORY:
                LevelManager.instance.AddMemory();
                SceneManager2.instance.sfxPlayer.Play("player_collect_memory");
                break;

            case CollectibleLogic.CType.HEALTHKIT:
                Regen(value);
                break;

            case CollectibleLogic.CType.GUN:
                PlayerGun.SetHolding(true);
                break;

            default:
                Debug.Log("Unknown collectible: " + ctype);
                break;
        }
    }

    private void Regen(float value)
    {
        healthPoints += value;
        healthPoints = Mathf.Min(healthPoints, MaxHealthPoints);
        HealthBar.UpdateValue(healthPoints / MaxHealthPoints);
        // play regen sound
    }

    public void Damage(float damage)
    {
        healthPoints -= damage;
        healthPoints = Mathf.Max(healthPoints, 0);
        HealthBar.UpdateValue(healthPoints / MaxHealthPoints);
        SceneManager2.instance.sfxPlayer.Play("player_hurt");
        if (healthPoints == 0) Die();
    }
    
    public void ResetHealth()
    {
        healthPoints = MaxHealthPoints;
        HealthBar.UpdateValue(1);
    }

    private void Die()
    {
        SceneManager2.instance.sfxPlayer.Play("player_die");
        LevelManager.instance.GameOver(false);
    }
}
