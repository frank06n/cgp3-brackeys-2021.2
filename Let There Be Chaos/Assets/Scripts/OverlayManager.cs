using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class OverlayManager : MonoBehaviour {

	[SerializeField] private Image image;

	[SerializeField] private float fadeDuration;
	[SerializeField] private AnimationCurve curve;

	private IEnumerator Start() {
		float timePassed = 0;
		bool raycastTargetUnchecked = false;

		while (timePassed <= fadeDuration) {
			Color col = image.color;
			float fraction = Mathf.Clamp01(timePassed / fadeDuration);
			col.a = 1 - curve.Evaluate(fraction);
			image.color = col;

			if (fraction >= 0.7f && !raycastTargetUnchecked) {
				image.raycastTarget = false;
				raycastTargetUnchecked = true;
			}

			timePassed += Time.deltaTime;
			yield return null;
		}
	}

	public IEnumerator FadeIn() {
		float timePassed = 0;
		image.raycastTarget = true;

		while (timePassed <= fadeDuration) {
			Color col = image.color;
			col.a = curve.Evaluate(Mathf.Clamp01(timePassed / fadeDuration));
			image.color = col;

			timePassed += Time.deltaTime;
			yield return null;
		}
	}
}
