using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGunLogic : GunLogic
{
    [SerializeField] private float MaxFireRate;

    private bool isHeld;
    private float lastFire;
    private float lastPress;

    protected override void OnAwake()
    {
        SetActive(isHeld = false);

        lastFire = 0;
        lastPress = 100; // Random big junk value (to avoid spawn fire)
    }

    private void CheckFire()
    {
        float fireCooldown = 1 / MaxFireRate;
        if (lastFire>fireCooldown && lastPress<fireCooldown)
        {
            lastFire = 0;
            FireBullet();
        }
    }

    public void FirePress()
    {
        lastPress = 0;
        //if (isHeld) CheckFire();
    }

    public void SetHolding(bool status)
    {
        SetActive(isHeld = status);
    }

    private void Update()
    {
        if (LevelManager.instance.gameIsOver || LevelManager.instance.gameIsPaused) return;

        lastPress += Time.deltaTime;
        lastFire += Time.deltaTime;

        if (!isHeld) return;

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        bool inverted = LevelManager.instance.player.transform.localScale.x < 0;
        LookTowards(mousePosition, inverted);
        
        CheckFire();
    }
}

