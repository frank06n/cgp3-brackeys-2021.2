using System.Collections;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
	[SerializeField] private AudioClip[] clips;
	private int currentClip;

	[SerializeField] private float fadeDuration;
	[SerializeField] private AnimationCurve curve;

	private AudioSource source;

	private bool isFadingVolume;
	private bool halfVolume;
	private float maxVolume;

	private IEnumerator Start() {
		source = GetComponent<AudioSource>();
		LoadVolume(-1);

		StartCoroutine(Fade(true));

		while (true) {
			source.clip = GetAnotherClip();
			source.Play();
			yield return new WaitForSeconds(source.clip.length);
		}
	}

	public void LoadVolume(float fakeVolume) {
		string musicKey = OptionsManager.musicKey;

		if (fakeVolume >= 0f)
			maxVolume = fakeVolume;
		else if (PlayerPrefs.HasKey(musicKey))
			maxVolume = PlayerPrefs.GetFloat(musicKey);
		else
			maxVolume = 1;
		
		if (!isFadingVolume)
			source.volume = maxVolume * (halfVolume ? 0.5f : 1);
	}

	private void HalfVolume()
    {
        halfVolume = true;
        LoadVolume(-1);
    }

	public void FadeOut() {
		StartCoroutine(Fade(false));
	}

	private IEnumerator Fade(bool fadein) {
		float timePassed = 0;
		isFadingVolume = true;

		while (timePassed <= fadeDuration) {

			float vol = Mathf.Clamp01(timePassed / fadeDuration);
			vol = curve.Evaluate(vol);
			if (!fadein) vol = 1 - vol;

			source.volume = vol * maxVolume * (halfVolume ? 0.5f : 1);

			timePassed += Time.deltaTime;
			yield return null;
		}

		isFadingVolume = false;
	}

	private AudioClip GetAnotherClip() {
		int last = clips.Length -1;

		if (currentClip == 0) {
			currentClip = Random.Range(1, last);
		} else if (currentClip == last) {
			currentClip = Random.Range(0, last-1);
		} else {
			currentClip = (Random.value > 0.5f) ? Random.Range(0, currentClip-1) : Random.Range(currentClip+1, last);
		}

		return clips[currentClip];
	}

	private void Mute()
    {
        source.mute = true;
    }

	private void Unmute()
    {
        source.mute = false;
    }
}
