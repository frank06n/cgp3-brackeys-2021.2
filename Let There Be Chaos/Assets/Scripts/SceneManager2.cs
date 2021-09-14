using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneManager2 : MonoBehaviour {
    [SerializeField] private OverlayManager overlay;
	public MusicPlayer musicPlayer;
    public SfxPlayer sfxPlayer;


    public static SceneManager2 instance;
    public int buildIndex { get; private set; }

    private void Awake()
    {
        if (instance != null) Destroy(gameObject);
        instance = this;
        buildIndex = SceneManager.GetActiveScene().buildIndex;
    }

    public string GetActiveSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }

    public void LoadScene(string name)
    {
        StartCoroutine(PrepareLoadScene(name));
    }

    public void LoadScene(int index)
    {
        StartCoroutine(PrepareLoadScene(index));
    }

    public void Quit() {
		StartCoroutine(PrepareQuit());
	}

    private IEnumerator PrepareLoadScene(string name)
    {
        musicPlayer.FadeOut();
        yield return overlay.FadeIn();
        SceneManager.LoadSceneAsync(name);
    }

    private IEnumerator PrepareLoadScene(int index)
    {
        musicPlayer.FadeOut();
        yield return overlay.FadeIn();
        SceneManager.LoadSceneAsync(index);
    }

    private IEnumerator PrepareQuit() {
		musicPlayer.FadeOut();
		yield return overlay.FadeIn();
		yield return new WaitForSeconds(0.5f);
		Application.Quit();
	}
}
