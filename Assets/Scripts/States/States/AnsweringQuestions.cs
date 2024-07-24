using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace States
{
	public class AnsweringQuestions : BaseState
	{

		public override void StateEnter()
		{
			base.StateEnter();

			AnsweringQuestionsUI.Instance.ShowHide(true);
			AnsweringQuestionsUI.Instance.Setup(gameManager.Category, gameManager.Question, ButtonClicked);
		}

		public override void StateExit()
		{
			base.StateExit();
			AnsweringQuestionsUI.Instance.ShowHide(false);
		}

		/// <summary>
		/// One of the available answers has been clicked. Handles checking if it is correct and informing the ui how to respond.
		/// </summary>
		/// <param name="answer"></param>
		public void ButtonClicked(SO_Answer answer) 
		{
			AudioManager.Instance.PlayClip("UiClick", 1f);
			bool didTheyAnswerCorrect = answer == gameManager.Question.CorrectAnswer;
			AnsweringQuestionsUI.Instance.DisplayAnswer(didTheyAnswerCorrect, GameManager.Instance.Question.CorrectAnswer);
		}
	}
}

