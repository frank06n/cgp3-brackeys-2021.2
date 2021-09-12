using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunLogic : MonoBehaviour
{
    [SerializeField] private float BulletDamage;
    [SerializeField] private float BulletSpeed;
    [SerializeField] private float BulletLife;
    [SerializeField] private GameObject BulletPrefab;

    [SerializeField] protected string BulletFireSfx;

    private Transform ArmPivot;
    private Transform BulletSpawnPoint;


    private void Awake()
    {
        ArmPivot = transform.GetChild(0);
        BulletSpawnPoint = ArmPivot.GetChild(1);
        OnAwake();
    }

    protected virtual void OnAwake() { }

    protected void SetActive(bool status)
    {
        ArmPivot.gameObject.SetActive(status);
    }

    protected void FireBullet()
    {
        Vector2 speed = (BulletSpawnPoint.position - transform.position).normalized * BulletSpeed;
        GameObject bulletObj = Instantiate(BulletPrefab, BulletSpawnPoint.position, ArmPivot.rotation, transform);
        bulletObj.transform.parent = LevelManager.instance.BulletsHolder;

        BulletLogic bullet = bulletObj.GetComponent<BulletLogic>();
        bullet.Initialise(BulletDamage, speed, BulletLife);

        SceneManager2.instance.sfxPlayer.Play(BulletFireSfx);
    }

    protected void LookTowards(Vector2 position, bool inverted=false)
    {
        float lookAngle = Vector2.SignedAngle(Vector2.right, position - (Vector2)transform.position);
        if (inverted) lookAngle += 180;

        LookTowards(lookAngle);
    }

    protected void LookTowards(float lookAngle)
    {
        if (lookAngle > 180) lookAngle -= 360;
        if (lookAngle < -180) lookAngle += 360;
        ArmPivot.rotation = Quaternion.Euler(0, 0, lookAngle);

        float scaleY = (Mathf.Abs(lookAngle) <= 90) ? +1 : -1;
        ArmPivot.localScale = new Vector3(1, scaleY, 1);
    }

    protected float GetAngle()
    {
        return ArmPivot.rotation.z;
    }
}
