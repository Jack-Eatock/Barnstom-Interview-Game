namespace States
{
	public class StatesDefinitions
	{
		public enum GameStates { SpinningWheel, AnsweringQuestions }
		public enum ChangeInState { NoChange, NextState, PreviousState }

		public interface IState
		{
			void StateEnter();

			void StateExit();

			void StateUpdate();

			ChangeInState InteractionDragging();

			ChangeInState InteractionDraggingReleased();

			ChangeInState InteractionClicked();

		}
	}
}



