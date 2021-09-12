using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurretLogic : GunLogic
{
    [SerializeField] private float SpawnDelay;


    private float lastspawn;

    private HealthBarScript HealthBar;
    private float healthPoints;
    [SerializeField] private float MaxHealthPoints;

    [Header("Turret Behaviour")]
    [SerializeField] private bool LookAtPlayer;

    // Used if LookAtPlayer==false
    [SerializeField] private float angleDisplacement;
    [SerializeField] private float RotateTimePeriod;
    private float baseAngle;
    private float rotateTimePassed;

    [SerializeField] private Vector2 displacement;
    [SerializeField] private float MoveTimePeriod;
    private Vector2 basePosition;
    private float moveTimePassed;


    protected override void OnAwake()
    {
        lastspawn = 2; // 2 second delay to start firing
        HealthBar = GetComponentInChildren<HealthBarScript>();
        healthPoints = MaxHealthPoints;

        baseAngle = GetAngle();
        basePosition = transform.position;
    }
   
    private void Update()
    {
        if (LevelManager.instance.gameIsOver) return;

        if (!LevelManager.instance.IsInLoadedChunks(transform.position)) return;

        if (LookAtPlayer)
            LookTowards(LevelManager.instance.player.transform.position);
        else if (angleDisplacement != 0)
        {
            rotateTimePassed += Time.deltaTime;
            BehaveRotate();
        }

        if (displacement!=Vector2.zero)
        {
            moveTimePassed += Time.deltaTime;
            BehaveMove();
        }


        if (lastspawn > SpawnDelay)
        {
            FireBullet();
            lastspawn = 0f;
        }
        else
        {
            lastspawn += Time.deltaTime;
        }
    }

    private void BehaveRotate()
    {
        float current = (rotateTimePassed % RotateTimePeriod) / RotateTimePeriod;
        float factor = 0;

        if      (current < .25f) factor = current * 4;
        else if (current < .5f)  factor = (.5f - current) * 4;
        else if (current < .75f) factor = (current - .5f) * -4;
        else                     factor = (1f - current) * -4;

        LookTowards(baseAngle + factor * angleDisplacement);
        Debug.Log(baseAngle + factor * angleDisplacement);

        // 0 -> .25 -> .5 -> .75 -> 1
        // 0 -> x   -> 0  -> -x  -> 0
    }

    private void BehaveMove()
    {
        float current = (moveTimePassed % MoveTimePeriod) / MoveTimePeriod;
        float factor = 0;

        if (current < .25f) factor = current * 4;
        else if (current < .5f) factor = (.5f - current) * 4;
        else if (current < .75f) factor = (current - .5f) * -4;
        else factor = (1f - current) * -4;

        transform.position = basePosition + factor * displacement;
    }

    public void Damage(float damage)
    {
        healthPoints -= damage;
        healthPoints = Mathf.Max(healthPoints, 0);
        HealthBar.UpdateValue(healthPoints / MaxHealthPoints);
        SceneManager2.instance.sfxPlayer.Play("turret_hurt");
        if (healthPoints == 0) Die();
    }
    private void Die()
    {
        // particle etc...
        SceneManager2.instance.sfxPlayer.Play("turret_die");
        Destroy(gameObject);
    }
}
