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
			AnsweringQuestionsUI.Instance.Setup(gameManager.Category, gameManager.Question);
		}

		public override void StateExit()
		{
			base.StateExit();
			AnsweringQuestionsUI.Instance.ShowHide(false);
		}

	}
}

