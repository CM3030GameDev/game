using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSceneTransition : MonoBehaviour
{
	[Header("Fade Overlay")]
	[SerializeField] private CanvasGroup fadeOverlay;

	[Header("Transition")]
	[SerializeField] private float fadeToBlackDuration = 1f;

	private bool transitionInProgress;

	private void Awake()
	{
		if (fadeOverlay == null)
		{
			Debug.LogError("Fade Overlay has not been assigned.");
			return;
		}

		fadeOverlay.alpha = 1f;
		fadeOverlay.blocksRaycasts = true;
		fadeOverlay.interactable = false;
	}

	private void Start()
	{
		if (fadeOverlay != null)
		{
			StartCoroutine(FadeScreenFromBlack());
		}
	}

	public void LoadSceneWithFade(string sceneName)
	{
		if (transitionInProgress)
		{
			return;
		}

		if (string.IsNullOrWhiteSpace(sceneName))
		{
			Debug.LogError("No scene name was provided.");
			return;
		}

		StartCoroutine(TransitionRoutine(sceneName));
	}

	private IEnumerator TransitionRoutine(string sceneName)
	{
		transitionInProgress = true;

		if (fadeOverlay != null)
		{
			fadeOverlay.blocksRaycasts = true;
			fadeOverlay.interactable = true;
		}

		Coroutine screenFade = null;

		if (fadeOverlay != null)
		{
			screenFade = StartCoroutine(FadeScreenToBlack());
		}

		if (UIAudioManager.Instance != null)
		{
			yield return UIAudioManager.Instance.FadeMusicOutAndWait();
		}
		else
		{
			yield return new WaitForSecondsRealtime(fadeToBlackDuration);
		}

		if (screenFade != null)
		{
			yield return screenFade;
		}

		SceneManager.LoadScene(sceneName);
	}

	private IEnumerator FadeScreenToBlack()
	{
		float startAlpha = fadeOverlay.alpha;
		float elapsed = 0f;

		if (fadeToBlackDuration <= 0f)
		{
			fadeOverlay.alpha = 1f;
			yield break;
		}

		while (elapsed < fadeToBlackDuration)
		{
			elapsed += Time.unscaledDeltaTime;

			fadeOverlay.alpha = Mathf.Lerp(
				startAlpha,
				1f,
				elapsed / fadeToBlackDuration
			);

			yield return null;
		}

		fadeOverlay.alpha = 1f;
	}

	private IEnumerator FadeScreenFromBlack()
	{
		float elapsed = 0f;

		while (elapsed < fadeToBlackDuration)
		{
			elapsed += Time.unscaledDeltaTime;

			fadeOverlay.alpha = Mathf.Lerp(
				1f,
				0f,
				elapsed / fadeToBlackDuration
			);

			yield return null;
		}

		fadeOverlay.alpha = 0f;
		fadeOverlay.blocksRaycasts = false;
	}
}