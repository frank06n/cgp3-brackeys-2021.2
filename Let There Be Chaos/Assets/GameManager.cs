using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public TextMeshProUGUI scoreText;


    [HideInInspector]
    public PlayerLogic player;

    private int score;


    void Awake()
    {
        if (instance != null) Destroy(gameObject);

        instance = this;

        player = FindObjectOfType<PlayerLogic>();
        score = 0;

        StartCoroutine(GameStart());
    }

    IEnumerator GameStart()
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(2);
        Debug.Log("start");
        Time.timeScale = 1;
    }


    public void AddScore(int score)
    {
        this.score += score;
        scoreText.text = $"Score: {this.score:0000}";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
