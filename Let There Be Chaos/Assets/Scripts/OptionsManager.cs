using UnityEngine.UI;
using UnityEngine;

public class OptionsManager : MonoBehaviour {
	[SerializeField] private Slider musicSlider;
	[SerializeField] private Slider sfxSlider;

	public static readonly string musicKey = "OPTIONS_MUSIC";
	public static readonly string sfxKey = "OPTIONS_SFX";

	void Start() {
		musicSlider.value = PlayerPrefs.GetFloat(musicKey, 1);
		sfxSlider.value = PlayerPrefs.GetFloat(sfxKey, 1);
	}

	void Update() {
		SceneManager2.instance.musicPlayer.LoadVolume(musicSlider.value);
		SceneManager2.instance.sfxPlayer.LoadVolume(sfxSlider.value);
		Apply();
	}

	public void Apply() {
		PlayerPrefs.SetFloat(musicKey, musicSlider.value);
		PlayerPrefs.SetFloat(sfxKey, sfxSlider.value);
		PlayerPrefs.Save();
	}

    public void ResetProgress()
    {
		PlayerPrefs.DeleteAll();
		PlayerPrefs.Save();
	}
}
