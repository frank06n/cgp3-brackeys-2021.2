using System;
using UnityEngine;

public class SfxPlayer : MonoBehaviour {

	[System.Serializable]
	public class Sfx {
		public string name;

		public AudioClip clip;
		[Range(0,1)]
		public float selfVolume;

		[HideInInspector]
		public AudioSource source;
	}

	[SerializeField] private GameObject sourceHolder;
	[SerializeField] private Sfx[] sfxs;

	private float setVolume;

	private void Awake() {
		foreach (Sfx sfx in sfxs) {
			sfx.source = sourceHolder.AddComponent<AudioSource>();
			sfx.source.clip = sfx.clip;
		}

		LoadVolume(-1);
	}

	public void LoadVolume(float fakeVolume) {
		string sfxKey = OptionsManager.sfxKey;

		float toSetVolume;

		if (fakeVolume >= 0f)
			toSetVolume = fakeVolume;
		else if (PlayerPrefs.HasKey(sfxKey))
			toSetVolume = PlayerPrefs.GetFloat(sfxKey);
		else
			toSetVolume = 1;

		if (toSetVolume != setVolume)
		{
			setVolume = toSetVolume;

			foreach (Sfx sfx in sfxs)
				sfx.source.volume = sfx.selfVolume * setVolume;
		}
	}

    private Sfx GetSfx(string name)
    {
        return Array.Find(sfxs, _sfx => _sfx.name == name);
    }

	private AudioSource GetSource(string name)
    {
        Sfx sfx = Array.Find(sfxs, _sfx => _sfx.name == name);
        return (sfx == null) ? null : sfx.source;
    }

	private float ValidateVolume(float sfxVolume, string name)
    {
        return GetSfx(name).selfVolume * setVolume * sfxVolume;
    }

	public void Play(string name)
	{
		Sfx sfx = Array.Find(sfxs, _sfx => _sfx.name == name);
		if (sfx == null)
			Debug.LogWarning("No loaded Sfx found: '" + name + "'");
		else
			sfx.source.Play();
	}

	public void Pause()
    {
        foreach (Sfx sfx in sfxs)
            sfx.source.Pause();
    }

	public void Resume()
    {
        foreach (Sfx sfx in sfxs)
            sfx.source.UnPause();
    }
}
