using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class AnsweringQuestionsUI : MonoBehaviour
{
	public static AnsweringQuestionsUI Instance;

	[SerializeField]
	private TextMeshProUGUI CategoryText, QuestionText;

	[SerializeField]
	private Transform buttonHolder;
	[SerializeField]
	private ButtonScript button;
	private IEnumerator coroutine;
	[SerializeField]
	private float timeToWaitBeforeShowingAnswer = 2, timeToWaitAfterShowingAnswer = 2;

	private List<ButtonScript> buttons = new List<ButtonScript>();

	private void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(Instance);

		foreach (Transform button in buttonHolder)
			Destroy(button.gameObject);
	}

	public void ShowHide(bool show)
	{
		transform.GetChild(0).gameObject.SetActive(show);
	}

	public void Setup(SO_Category category, SO_Question question)
	{
		CategoryText.text = "Category: " + category.Text;
		QuestionText.text = question.Text;

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
			buttons[i].Setup(answerArray[i], OnButtonClicked);

		// Shuffle the buttons. So it is not always the first button.
		foreach (Transform button in buttonHolder)
			button.SetSiblingIndex(Random.Range(0, buttonHolder.childCount));
    }

	private void OnButtonClicked(SO_Answer answer)
	{
		Debug.Log("Clicked " + answer.Text);

		// Was it correct??
		if (coroutine != null)
			StopCoroutine(coroutine);

		coroutine = DelayedAnswer();
		StartCoroutine(coroutine);
	}

	private IEnumerator DelayedAnswer()
	{
		GameManager.Instance.EventSystem.enabled = false;

		yield return new WaitForSecondsRealtime(timeToWaitBeforeShowingAnswer);

		// Is it the correct answer?
		foreach (ButtonScript button in buttons)
		{
			if (button.gameObject.activeSelf && button.Answer == GameManager.Instance.Question.CorrectAnswer)
			{
				button.FlashColour(Color.green, timeToWaitAfterShowingAnswer);
				break;
			}
		}

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
