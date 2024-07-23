using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

		public override StatesDefinitions.ChangeInState InteractionDraggingReleased()
		{
			firstFrameDragging = true;
			draggingWheel = false;

			// Good spin
			if (MathF.Abs(gameManager.Wheel.Velocity) > 150)
			{
				wheelSpinning = true;
				gameManager.SetNextState(StatesDefinitions.GameStates.AnsweringQuestions);
				return StatesDefinitions.ChangeInState.NextState;
			}

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
			}
			return StatesDefinitions.ChangeInState.NoChange;
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
