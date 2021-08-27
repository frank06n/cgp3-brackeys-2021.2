using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLogic : MonoBehaviour
{
    private float damage;
    private float life;

    public void Initialise(float damage, Vector2 speed, float life)
    {
        this.damage = damage;
        GetComponent<Rigidbody2D>().velocity = speed;
        this.life = life;
    }

    // Update is called once per frame
    void Update()
    {
        if (life <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            life -= Time.deltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        Debug.Log("Bullet Hit: " + collision.collider.name);
        if (collision.collider.CompareTag("Player"))
        {
            LevelManager.instance.player.Damage(damage);
        }
        else 
        {
            // if turret, damage turret
        }
        SceneManager2.instance.sfxPlayer.Play("powerup"); // change to 'hit'
        Destroy(gameObject);
    }
}
