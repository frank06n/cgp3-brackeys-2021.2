using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        StartCoroutine(GameStart());

    }

    IEnumerator GameStart()
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(2);
        Debug.Log("start");
        Time.timeScale = 1;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
