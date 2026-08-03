using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuButtonHover : MonoBehaviour,
	IPointerEnterHandler,
	IPointerExitHandler,
	ISelectHandler,
	IDeselectHandler
{
	[Header("References")]
	[SerializeField] private RectTransform textTransform;
	[SerializeField] private RectTransform accentLine;
	[SerializeField] private TMP_Text buttonText;

	[Header("Movement")]
	[SerializeField] private float textMoveDistance = 18f;
	[SerializeField] private float animationSpeed = 12f;

	[Header("Accent Line")]
	[SerializeField] private float normalLineWidth = 6f;
	[SerializeField] private float highlightedLineWidth = 18f;

	[Header("Text Colours")]
	[SerializeField]
	private Color normalTextColor =
		new Color32(235, 235, 235, 255);

	[SerializeField]
	private Color highlightedTextColor =
		new Color32(255, 255, 255, 255);

	private Button button;
	private Vector2 originalTextPosition;
	private Vector2 targetTextPosition;
	private float targetLineWidth;
	private Color targetTextColor;
	private bool pointerInside;

	private void Awake()
	{
		button = GetComponent<Button>();

		if (textTransform != null)
		{
			originalTextPosition = textTransform.anchoredPosition;
			targetTextPosition = originalTextPosition;
		}

		targetLineWidth = normalLineWidth;
		targetTextColor = normalTextColor;

		if (button != null)
		{
			button.onClick.AddListener(PlayClickSound);
		}
	}

	private void OnDestroy()
	{
		if (button != null)
		{
			button.onClick.RemoveListener(PlayClickSound);
		}
	}

	private void Update()
	{
		float t = animationSpeed * Time.unscaledDeltaTime;

		if (textTransform != null)
		{
			textTransform.anchoredPosition = Vector2.Lerp(
				textTransform.anchoredPosition,
				targetTextPosition,
				t
			);
		}

		if (accentLine != null)
		{
			Vector2 size = accentLine.sizeDelta;
			size.x = Mathf.Lerp(size.x, targetLineWidth, t);
			accentLine.sizeDelta = size;
		}

		if (buttonText != null)
		{
			buttonText.color = Color.Lerp(
				buttonText.color,
				targetTextColor,
				t
			);
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		pointerInside = true;
		SetHighlighted(true);

		if (UIAudioManager.Instance != null)
		{
			UIAudioManager.Instance.PlayHover();
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		pointerInside = false;
		SetHighlighted(false);
	}

	public void OnSelect(BaseEventData eventData)
	{
		SetHighlighted(true);

		// Avoid playing twice when the mouse hover also selects the button.
		if (!pointerInside && UIAudioManager.Instance != null)
		{
			UIAudioManager.Instance.PlayHover();
		}
	}

	public void OnDeselect(BaseEventData eventData)
	{
		if (!pointerInside)
		{
			SetHighlighted(false);
		}
	}

	private void PlayClickSound()
	{
		if (UIAudioManager.Instance != null)
		{
			UIAudioManager.Instance.PlayClick();
		}
	}

	private void SetHighlighted(bool highlighted)
	{
		if (textTransform != null)
		{
			targetTextPosition = highlighted
				? originalTextPosition + Vector2.right * textMoveDistance
				: originalTextPosition;
		}

		targetLineWidth = highlighted
			? highlightedLineWidth
			: normalLineWidth;

		targetTextColor = highlighted
			? highlightedTextColor
			: normalTextColor;
	}
}