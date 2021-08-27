using UnityEngine.UI;
using UnityEngine;

public class OptionsManager : MonoBehaviour {
	[SerializeField] private Slider musicSlider;
	[SerializeField] private Slider sfxSlider;

	private float defaultMusicValue;
	private float defaultSfxValue;

	public static readonly string musicKey = "OPTIONS_MUSIC";
	public static readonly string sfxKey = "OPTIONS_SFX";

	[SerializeField] private SceneManager2 sceneManager2;

	void Start() {
		defaultMusicValue = musicSlider.value;
		defaultSfxValue = sfxSlider.value;

		if (PlayerPrefs.HasKey(musicKey))
			musicSlider.value = PlayerPrefs.GetFloat(musicKey);
		if (PlayerPrefs.HasKey(sfxKey))
			sfxSlider.value = PlayerPrefs.GetFloat(sfxKey);
	}

	void Update() {
		sceneManager2.musicPlayer.LoadVolume(musicSlider.value);
		sceneManager2.sfxPlayer.LoadVolume(sfxSlider.value);
	}

	public void LoadDefaults() {
		musicSlider.value = defaultMusicValue;
		sfxSlider.value = defaultSfxValue;
	}

	public void Apply() {
		PlayerPrefs.SetFloat(musicKey, musicSlider.value);
		PlayerPrefs.SetFloat(sfxKey, sfxSlider.value);
		PlayerPrefs.Save();
	}

	public void Back() {
		sceneManager2.LoadScene("MainMenu");
	}
}
