using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    private int LevelNumber;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI memoryText;
    [SerializeField] private TextMeshProUGUI timeText;

    [SerializeField] private GameObject uiPanel;
    [SerializeField] private GameObject textEsc;

    [SerializeField] private GameObject changePanel;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI statusText;

    [SerializeField] private Button resumeLevel;
    [SerializeField] private Button nextLevel;

    [Header("Other Elements")]
    public Transform BulletsHolder;
    [SerializeField] private Vector2 LoadedChunkDistance;

    [HideInInspector] public PlayerLogic player;
    private int score;
    private int memoryCollected;
    private float time;

    [HideInInspector] public int memoryCount;


    [HideInInspector] public bool gameIsOver;
    [HideInInspector] public bool gameWon;
    [HideInInspector] public bool gameIsPaused;

    public HealthBarScript hbs;
    void Awake()
    {
        if (instance != null) Destroy(gameObject);

        instance = this;

        player = FindObjectOfType<PlayerLogic>();
        score = 0;
        memoryCollected = 0;
        memoryCount = 0;
        time = 0;

        gameIsOver = false;
        gameWon = false;
        gameIsPaused = false;

        string sceneName = SceneManager2.instance.GetActiveSceneName();
        if (sceneName.StartsWith("Level ")) {
            LevelNumber = int.Parse(sceneName.Substring(6));
        }
        else
        {
            LevelNumber = 1;
            Debug.LogWarning("Scene name for this Level is not correctly formatted");
        }
    }


    private void Start()
    {
        if (SceneManager2.instance.buildIndex == 0)
            StartCoroutine(UpdateSceneForMainMenu());
        else
            UpdateUI();
    }

    public void AddScore(int score)
    {
        this.score += score;
        UpdateUI();
    }

    public void AddMemory()
    {
        memoryCollected++;
        UpdateUI();

        if (memoryCollected==memoryCount) GameOver(true);
    }

    public void GameOver(bool won) {
        gameIsOver = true;
        gameWon = won;

        player.Freeze();

        SceneManager2.instance.musicPlayer.FadeOut();
        SaveProgress();
        if (won)
        {
            SceneManager2.instance.sfxPlayer.Play("player_victory");
            DisplayPanel(true, "You won the level! :)");
        }
        else
        {
            SceneManager2.instance.sfxPlayer.Play("player_lose");
            DisplayPanel(true, "You lost the level! :(");
        }
    }

    private void DisplayPanel(bool show, string status="")
    {
        if (show)
        {
            levelText.text = $"Level {LevelNumber:00}";
            statusText.text = status;

            if (!status.Equals("Game is Paused!"))
            {
                resumeLevel.interactable = false;
            }

            nextLevel.interactable = IsLevelComplete();
        }
        changePanel.SetActive(show);
        uiPanel.SetActive(!show);
        textEsc.SetActive(!show);
    }

    private void UpdateUI()
    {
        if (uiPanel == null) return;
        scoreText.text = $"Score: {score:0000}";
        memoryText.text = $"Memories: {memoryCollected}/{memoryCount}";

        int secs = (int)time % 60;
        int mins = (int)time / 60;

        timeText.text = $"Time: {mins:00}:{secs:00}";
    }

    private void SaveProgress()
    {
        string completed = PlayerPrefs.GetString($"level{LevelNumber}_completed", "false");
        int minTime = PlayerPrefs.GetInt($"level{LevelNumber}_minTime", -1);
        int maxScore = PlayerPrefs.GetInt($"level{LevelNumber}_maxScore", 0);
        int highestUnlockedLevel = PlayerPrefs.GetInt("highestUnlockedLevel", 1);

        if (gameWon)
        {
            completed = "true";

            if (minTime == -1) minTime = (int)time;
            else minTime = Mathf.Min(minTime, (int)time);

            maxScore = Mathf.Max(maxScore, score);

            highestUnlockedLevel = Mathf.Max(highestUnlockedLevel, LevelNumber+1);
        }

        PlayerPrefs.SetString($"level{LevelNumber}_completed", completed);
        PlayerPrefs.SetInt($"level{LevelNumber}_minTime", minTime);
        PlayerPrefs.SetInt($"level{LevelNumber}_maxScore", maxScore);
        PlayerPrefs.SetInt("highestUnlockedLevel", highestUnlockedLevel);
        PlayerPrefs.Save();
    }

    public bool IsInLoadedChunks(Vector2 point)
    {
        Vector2 playerpos = player.transform.position;
        float x1 = playerpos.x - LoadedChunkDistance.x;
        float x2 = playerpos.x + LoadedChunkDistance.x;
        float y1 = playerpos.y - LoadedChunkDistance.y;
        float y2 = playerpos.y + LoadedChunkDistance.y;

        return (x1 <= point.x && point.x <= x2) && (y1 <= point.y && point.y <= y2);
    }


    private bool IsLevelComplete()
    {
        return PlayerPrefs.GetString($"level{LevelNumber}_completed", "false").Equals("true");
    }


    public void TogglePauseState()
    {
        if (gameIsPaused)
        {
            DisplayPanel(false);
            SceneManager2.instance.sfxPlayer.Resume();
            Time.timeScale = 1;
        }
        else
        {
            DisplayPanel(true, "Game is Paused!");
            SceneManager2.instance.sfxPlayer.Pause();
            Time.timeScale = 0;
        }
        gameIsPaused = !gameIsPaused;
    }

    public void RetryLevel()
    {
        Time.timeScale = 1;
        player.Freeze();
        SceneManager2.instance.LoadScene(SceneManager2.instance.buildIndex);
    }

    public void GoToLevels()
    {
        Time.timeScale = 1;
        player.Freeze();
        SceneManager2.instance.LoadScene(1);
    }

    public void NextLevel()
    {
        Time.timeScale = 1;
        player.Freeze();
        SceneManager2.instance.LoadScene(SceneManager2.instance.buildIndex+1);
    }

    private void Update()
    {
        if (SceneManager2.instance.buildIndex==0)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                SceneManager2.instance.LoadScene(1);
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                SceneManager2.instance.Quit();
            }
            return;
        }

        if (gameIsOver) return;

        time += Time.deltaTime;
        UpdateUI();

        if (Input.GetKeyDown(KeyCode.Escape)) TogglePauseState();
    }

    private IEnumerator UpdateSceneForMainMenu()
    {
        while (true)
        {
            yield return new WaitForSeconds(10f);
            player.ResetHealth();
        }
    }
}
