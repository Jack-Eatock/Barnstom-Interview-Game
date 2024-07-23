using States;
using UnityEngine;

namespace States
{
	public class BaseState : StatesDefinitions.IState
	{
		protected GameManager gameManager;

		public virtual void StateEnter()
		{
			Debug.Log("Entered state " + this);
			gameManager = GameManager.Instance;
		}

		public virtual void StateExit()
		{
			Debug.Log("Exited state " + this);
		}

		public virtual void StateUpdate()
		{

		}

		public virtual StatesDefinitions.ChangeInState InteractionClicked()
		{
			Debug.Log("Clicked");
			return StatesDefinitions.ChangeInState.NoChange;
		}

		public virtual StatesDefinitions.ChangeInState InteractionDragging()
		{
			Debug.Log("Dragging");
			return StatesDefinitions.ChangeInState.NoChange;
		}

		public virtual StatesDefinitions.ChangeInState InteractionDraggingReleased()
		{
			Debug.Log("Released Drag");
			return StatesDefinitions.ChangeInState.NoChange;
		}

	}
}
