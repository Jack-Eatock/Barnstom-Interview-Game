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

		public void ButtonClicked(SO_Answer answer) 
		{
			AudioManager.Instance.PlayClip("UiClick", 1f);
			bool didTheyAnswerCorrect = answer == gameManager.Question.CorrectAnswer;
			AnsweringQuestionsUI.Instance.DisplayAnswer(didTheyAnswerCorrect, GameManager.Instance.Question.CorrectAnswer);
		}
	}
}

