using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurretLogic : MonoBehaviour
{
    [SerializeField] private float BulletDamage;
    [SerializeField] private float BulletSpeed;
    [SerializeField] private float BulletLife;
    [SerializeField] private GameObject BulletPrefab;
    [SerializeField] private float SpawnDelay;

    private float lastspawn;

    private Transform ArmPivot;
    private Transform BulletSpawnPoint;


    void Awake()
    {
        ArmPivot = transform.GetChild(0);
        BulletSpawnPoint = ArmPivot.GetChild(1);

        lastspawn = 2; // 2 second delay to start firing
    }

    // Update is called once per frame
    void Update()
    {
        if (lastspawn>SpawnDelay)   FireBullet();
        else                        lastspawn += Time.deltaTime;
    }

    private void FireBullet()
    {
        Vector2 speed = (BulletSpawnPoint.position - transform.position).normalized * BulletSpeed;
        GameObject bulletObj = Instantiate(BulletPrefab, BulletSpawnPoint.position, ArmPivot.rotation, transform);

        BulletLogic bullet = bulletObj.GetComponent<BulletLogic>();
        bullet.Initialise(BulletDamage, speed, BulletLife);

        lastspawn = 0f;
        Debug.Log("Fired");
        SceneManager2.instance.sfxPlayer.Play("shoot");
    }
}
