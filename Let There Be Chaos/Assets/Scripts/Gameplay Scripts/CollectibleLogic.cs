using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleLogic : MonoBehaviour
{   
    public enum CType
    {
        COIN,
        MEMORY
    };

    [SerializeField] private CType CollectibleType;
    [SerializeField] private int Value;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            LevelManager.instance.player.Collects(CollectibleType, Value);

            Destroy(gameObject);
        }
    }
}
