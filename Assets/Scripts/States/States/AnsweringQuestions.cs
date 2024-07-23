using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace States
{
	public class AnsweringQuestions : BaseState
	{
		private IEnumerator awaitingSpinFinish;
		WheelSection section;

		public override void StateEnter()
		{
			base.StateEnter();
			if (awaitingSpinFinish != null)
				gameManager.StartCoroutine(awaitingSpinFinish);

			awaitingSpinFinish = AwaitingSpinFinishes();
			gameManager.StartCoroutine(AwaitingSpinFinishes());
		}

		private IEnumerator AwaitingSpinFinishes()
		{


			while (Mathf.Abs(gameManager.Wheel.Velocity) > .1f)
				yield return new WaitForEndOfFrame();


			Debug.Log("COMPLETE!");

			// What category??
			section = gameManager.Wheel.WhichWheelSectionDidWeLandOn();
			Debug.Log("section " + section.Category.Text);
		}
	}
}

