using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelMenuItemScript : MonoBehaviour
{
    [SerializeField] private int LevelNumber;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI statusText;

    private void Start()
    {
        string completed = PlayerPrefs.GetString($"level{LevelNumber}_completed", "false");
        int minTime = PlayerPrefs.GetInt($"level{LevelNumber}_minTime", -1);
        int maxScore = PlayerPrefs.GetInt($"level{LevelNumber}_maxScore", 0);
        int highestUnlockedLevel = PlayerPrefs.GetInt("highestUnlockedLevel", 1);

        levelText.text = $"Level {LevelNumber:00}";

        if (LevelNumber > highestUnlockedLevel)
        {
            GetComponent<Button>().interactable = false;
            statusText.text = "Locked";
        }
        else if (completed.Equals("true"))
        {
            int mins = minTime / 60, secs = minTime % 60;
            statusText.text = $"Min time: {mins:00}:{secs:00}\nMax score: {maxScore}";
        }
        else
        {
            statusText.text = $"Not completed";
        }
    }

    public void LoadLevel()
    {
        SceneManager2.instance.LoadScene(LevelNumber + 1);
    }
}
