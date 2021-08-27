using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

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

        StartCoroutine(GameTestStartDelay());
    }

    IEnumerator GameTestStartDelay()
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(1);
        Debug.Log("start");
        Time.timeScale = 1;
    }


    public void AddScore(int score)
    {
        this.score += score;
        scoreText.text = $"Score: {this.score:0000}";
    }

    void Update()
    {
        
    }
}
