using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AnsweringQuestionsUI : MonoBehaviour
{
	public static AnsweringQuestionsUI Instance;

	[SerializeField]
	private TextMeshProUGUI categoryText, questionText;
	[SerializeField]
	private Transform buttonHolder;
	[SerializeField]
	private ButtonScript button;
	[SerializeField]
	private float timeToWaitBeforeShowingAnswer = 2, timeToWaitAfterShowingAnswer = 2;

	private IEnumerator coroutine;
	private List<ButtonScript> buttons = new List<ButtonScript>();

	private void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(Instance);

		// Clean up buttons
		foreach (Transform button in buttonHolder)
			Destroy(button.gameObject);
	}

	/// <summary>
	/// Show or hide this menu
	/// </summary>
	/// <param name="show"></param>
	public void ShowHide(bool show)
	{
		transform.GetChild(0).gameObject.SetActive(show);
	}

	public void Setup(SO_Category category, SO_Question question, Action<SO_Answer> clickedCB)
	{
		categoryText.text = "Category: " + category.Text;
		questionText.text = question.Text;

		SO_Answer[] answerArray = new SO_Answer[question.IncorrectAnswers.Count + 1];

		// Create array of all the possible answers
		answerArray[0] = question.CorrectAnswer;
		for (int i = 1; i < answerArray.Length; i++)
			answerArray[i] = question.IncorrectAnswers[i -1];

		// Make sure there are enough buttons created to use. Pool them
		int orignalButtonCount = buttons.Count;
		if (buttons.Count < answerArray.Length)
		{
			for (int i = 0; i < answerArray.Length - orignalButtonCount; i++)
			{
				ButtonScript newButton = Instantiate(button, buttonHolder);
				buttons.Add(newButton);
			}
		}

		// Setup all the buttons
		HideAllButtons();
        for (int i = 0; i < answerArray.Length; i++)
			buttons[i].Setup(answerArray[i], clickedCB);

		// Shuffle the buttons. So it is not always the first button.
		foreach (Transform button in buttonHolder)
			button.SetSiblingIndex(UnityEngine.Random.Range(0, buttonHolder.childCount));
    }

	/// <summary>
	/// Plays an animation before displaying the correct answer
	/// </summary>
	public void DisplayAnswer(bool didTheyAnswerCorrect, SO_Answer answerToHighlight)
	{
		// Was it correct??
		if (coroutine != null)
			StopCoroutine(coroutine);

		coroutine = DelayedAnswer(didTheyAnswerCorrect, answerToHighlight);
		StartCoroutine(coroutine);
	}

	private IEnumerator DelayedAnswer(bool didTheyAnswerCorrect, SO_Answer asnwerToHighlight)
	{
		GameManager.Instance.EventSystem.enabled = false;

		yield return new WaitForSecondsRealtime(timeToWaitBeforeShowingAnswer);

		// Highlight the correct button
		foreach (ButtonScript button in buttons)
		{
			if (button.gameObject.activeSelf && button.Answer == asnwerToHighlight)
			{
				button.FlashColour(GameManager.Instance.GoodColour, timeToWaitAfterShowingAnswer);
				break;
			}
		}

		// Play sound
		if (didTheyAnswerCorrect)
			AudioManager.Instance.PlayClip("Success", .8f);
		else
			AudioManager.Instance.PlayClip("Failure", .8f);

		yield return new WaitForSecondsRealtime(timeToWaitAfterShowingAnswer);

		GameManager.Instance.EventSystem.enabled = true;
		GameManager.Instance.SwitchState(States.StatesDefinitions.GameStates.SpinningWheel);
		coroutine = null;
	}


	private void HideAllButtons()
	{
		foreach (Transform child in buttonHolder)
			child.gameObject.SetActive(false);
	}
}
