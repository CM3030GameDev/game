using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSceneTransition : MonoBehaviour
{
	[Header("Fade Overlay")]
	[SerializeField] private CanvasGroup fadeOverlay;

	[Header("Transition")]
	[SerializeField] private float fadeToBlackDuration = 1f;

	[Header("Act Title Card")]
	[Tooltip("Shown on the black screen between scenes. Leave unassigned to skip the card.")]
	[SerializeField] private TMP_Text titleText;
	[SerializeField] private float titleFadeDuration = 0.5f;
	[SerializeField] private float titleHoldDuration = 1.5f;

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

		ResetTitle();
	}

	private void Start()
	{
		if (fadeOverlay != null)
		{
			StartCoroutine(FadeScreenFromBlack());
		}
	}

	private void ResetTitle()
	{
		// Title starts hidden and only appears once the screen is black.
		if (titleText != null)
		{
			titleText.text = string.Empty;
			titleText.alpha = 0f;
		}
	}

	public void LoadSceneWithFade(string sceneName)
	{
		LoadSceneWithFade(sceneName, null);
	}

	/// <summary>Fades out, shows a title card on the black screen, then loads the scene.</summary>
	public void LoadSceneWithFade(string sceneName, string title)
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

		StartCoroutine(TransitionRoutine(sceneName, title));
	}

	private IEnumerator TransitionRoutine(string sceneName, string title)
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

		// Card goes up only after the screen is fully black, so it never overlaps gameplay.
		yield return ShowTitleCard(title);

		SceneManager.LoadScene(sceneName);
	}

	private IEnumerator ShowTitleCard(string title)
	{
		if (titleText == null || string.IsNullOrWhiteSpace(title))
		{
			yield break;
		}

		titleText.text = title;

		yield return FadeTitle(0f, 1f);
		yield return new WaitForSecondsRealtime(titleHoldDuration);
		yield return FadeTitle(1f, 0f);
	}

	private IEnumerator FadeTitle(float from, float to)
	{
		if (titleFadeDuration <= 0f)
		{
			titleText.alpha = to;
			yield break;
		}

		float elapsed = 0f;

		while (elapsed < titleFadeDuration)
		{
			elapsed += Time.unscaledDeltaTime;
			titleText.alpha = Mathf.Lerp(from, to, elapsed / titleFadeDuration);
			yield return null;
		}

		titleText.alpha = to;
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