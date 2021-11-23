using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleLogic : MonoBehaviour
{   
    public enum CType
    {
        COIN,
        MEMORY,
        HEALTHKIT,
        GUN
    };

    [SerializeField] private CType CollectibleType;
    [SerializeField] private int Value;
    [SerializeField] private float HoverYChange;
    [SerializeField] private float HoverSpeed;

    private float initLocalY;
    private float hoverDelta;


    private void Start()
    {
        if (CollectibleType==CType.MEMORY)
        {
            LevelManager.instance.memoryCount++;
        }
        initLocalY = transform.localPosition.y;
        hoverDelta = Random.Range(0f, 2 * Mathf.PI);
    }

    private void Update()
    {
        if (HoverYChange>0)
        {
            hoverDelta += HoverSpeed * Time.deltaTime;
            float change = Mathf.Sin(hoverDelta) * HoverYChange;
            transform.localPosition = new Vector3(transform.localPosition.x, initLocalY + change, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            LevelManager.instance.player.Collects(CollectibleType, Value);

            Destroy(gameObject);
        }
    }
}
