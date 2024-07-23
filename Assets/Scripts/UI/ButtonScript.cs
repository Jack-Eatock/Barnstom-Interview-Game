using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text;
	private Button button;

	private SO_Answer cachedAnswer;
	private Action<SO_Answer> callback;
	private IEnumerator flashingColour;

	public SO_Answer Answer => cachedAnswer;

	private void Awake()
	{
		button = GetComponent<Button>();
	}

	private void OnEnable()
	{
		button.onClick.AddListener(Clicked);
	}

	private void OnDisable()
	{
		button.onClick.RemoveListener(Clicked);
	}

	public void FlashColour(Color colour, float time)
	{
		if (flashingColour != null)
			StopCoroutine(flashingColour);

		flashingColour = FlashingColour(colour, time);
		StartCoroutine(flashingColour);
	}

	private IEnumerator FlashingColour(Color colour, float time)
	{
		Color defaultColour = button.targetGraphic.color;
		button.enabled = false;
		button.targetGraphic.color = colour;

		yield return new WaitForSecondsRealtime(time);

		button.targetGraphic.color = defaultColour;
		button.enabled = true;
	}

	public void ToggleActive(bool active)
	{
		button.enabled = active;
	}

	public void Setup(SO_Answer answer, Action<SO_Answer> _callback)
    {
		cachedAnswer = answer;
		text.text = answer.Text;
		callback = _callback;
		gameObject.SetActive(true);
		ToggleActive(true);
	}

	private void Clicked()
	{
		callback?.Invoke(cachedAnswer);
	}
}
