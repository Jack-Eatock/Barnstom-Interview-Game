using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace States
{
	public class SpinningWheel : BaseState
	{
		private bool firstFrameDragging = true;
		private bool draggingWheel = false;
		private Vector2 lastDragPos;
		private bool wheelSpinning = false;
		private IEnumerator awaitingSpinFinish;

		public override void StateEnter()
		{
			base.StateEnter();

			if (gameManager.ActiveWheelSection != null)
			{
				gameManager.ActiveWheelSection.Shine(false);
				
			}

			gameManager.Wheel.Shine(false);
			gameManager.ToggleWheelSlider(true);
			SpinTheWheelUI.Instance.ShowHide(true);
			gameManager.EventSystem.enabled = true;
		}

		public override void StateExit()
		{
			base.StateExit();
			SpinTheWheelUI.Instance.ShowHide(false);
		}

		public override StatesDefinitions.ChangeInState InteractionDraggingReleased()
		{
			firstFrameDragging = true;

			if (draggingWheel)
				ReleasedTheWheel();

			return StatesDefinitions.ChangeInState.NoChange;
		}

		public override StatesDefinitions.ChangeInState InteractionDragging()
		{
			if (wheelSpinning)
				return StatesDefinitions.ChangeInState.NoChange;

			if (firstFrameDragging)
			{
				if (IsOverWheel(InputHandler.Instance.GetEventSystemRaycastResults()))
				{
					draggingWheel = true;
					lastDragPos = InputHandler.Instance.PrimaryPos;
					firstFrameDragging = false;
				}
			}

			if (draggingWheel)
			{
				// If they release the wheel after starting a spin.
				if (IsOverWheel(InputHandler.Instance.GetEventSystemRaycastResults()))
				{
					Vector2 dif = (InputHandler.Instance.PrimaryPos - lastDragPos);
					float value = 0; // CLockWise

					// Right Left = TOP OR BOTTOM
					if (InputHandler.Instance.PrimaryPos.y > gameManager.Wheel.transform.position.y)
						value += dif.x;
					else
						value -= dif.x;

					// UP Down = LEft or RIGHT of the wheel
					if (InputHandler.Instance.PrimaryPos.x > gameManager.Wheel.transform.position.x)
						value -= dif.y;
					else
						value += dif.y;

					gameManager.Wheel.ApplySpin(value);
					lastDragPos = InputHandler.Instance.PrimaryPos;
				}
				else
					ReleasedTheWheel();
			}
			return StatesDefinitions.ChangeInState.NoChange;
		}

		private void ReleasedTheWheel()
		{
			draggingWheel = false;

			// Good spin
			if (MathF.Abs(gameManager.Wheel.Velocity) > 150)
			{
			
				if (awaitingSpinFinish != null)
					gameManager.StartCoroutine(awaitingSpinFinish);

				awaitingSpinFinish = AwaitingSpinFinishes();
				gameManager.StartCoroutine(AwaitingSpinFinishes());

			}
		}

		private IEnumerator AwaitingSpinFinishes()
		{
			gameManager.ToggleWheelSlider(false);

			wheelSpinning = true;

			while (Mathf.Abs(gameManager.Wheel.Velocity) > .1f)
				yield return new WaitForEndOfFrame();

			// What category??
			gameManager.LandedOnWheelSection(gameManager.Wheel.WhichWheelSectionDidWeLandOn());

			// Have the text and area shine for a second. Before switching to questions.
			gameManager.ActiveWheelSection.Shine(true);
			gameManager.Wheel.Shine(true);
			Debug.Log("section " + gameManager.ActiveWheelSection.Category.Text);

			yield return new WaitForSecondsRealtime(4);

			gameManager.SwitchState(StatesDefinitions.GameStates.AnsweringQuestions);
		}

		private bool IsOverWheel(List<RaycastResult> eventSystemRaysastResults)
		{
			for (int index = 0; index < eventSystemRaysastResults.Count; index++)
			{
				RaycastResult curRaysastResult = eventSystemRaysastResults[index];
				if (curRaysastResult.gameObject.tag == "Wheel")
					return true;
			}
			return false;
		}
	}
}
