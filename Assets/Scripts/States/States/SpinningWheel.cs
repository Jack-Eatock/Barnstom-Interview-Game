using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace States
{
	public class SpinningWheel : BaseState
	{
		public override StatesDefinitions.ChangeInState InteractionDraggingReleased()
		{
			gameManager.SetNextState(StatesDefinitions.GameStates.AnsweringQuestions);
			return StatesDefinitions.ChangeInState.NextState;
		}
	}
}
