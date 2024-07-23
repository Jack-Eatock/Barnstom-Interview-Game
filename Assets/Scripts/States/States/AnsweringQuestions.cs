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
			AudioManager.Instance.PlayClip("UiClick", 1f);
		}
	}
}

