using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarScript : MonoBehaviour
{
    private SpriteRenderer BackRndr;
    private SpriteRenderer FillRndr;
    private Transform Fill;

    private float lastValueUpdate;
    
    private void Awake()
    {
        BackRndr = GetComponent<SpriteRenderer>();
        Fill = transform.GetChild(0);
        FillRndr = Fill.GetComponentInChildren<SpriteRenderer>();

        lastValueUpdate = 0;
    }
    
    private void Update()
    {
        lastValueUpdate += Time.deltaTime;
        float alpha = 1f;

        if (Fill.localScale.x>.2f) alpha = 1f - Mathf.Clamp01(lastValueUpdate-1);
        
        Color bcol = BackRndr.color;
        Color fcol = FillRndr.color;
        bcol.a = fcol.a = alpha;
        BackRndr.color = bcol;
        FillRndr.color = fcol;
    }

    public void UpdateValue(float value)
    {
        Fill.localScale = new Vector3(value, 1, 1);
        lastValueUpdate = 0;
    } 
}
