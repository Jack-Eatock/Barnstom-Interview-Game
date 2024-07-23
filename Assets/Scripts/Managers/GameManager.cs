using System;
using UnityEngine;
using States;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;

	// Game State

	[SerializeField]
	protected StatesDefinitions.GameStates activeState, prevState, nextState;
	protected StatesDefinitions.IState _activeState, _prevState, _nextState;

	#region Getters and Setters

	public StatesDefinitions.IState ActiveState => _activeState;

	#endregion

	private void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);

		nextState = StatesDefinitions.GameStates.SpinningWheel;
		CheckIfStateShouldChange(StatesDefinitions.ChangeInState.NextState);
	}

	#region Handling Game State

	/// <summary>
	/// Check if the statemachine should switch to a different state
	/// </summary>
	/// <param name="changeInState"></param>
	public void CheckIfStateShouldChange(StatesDefinitions.ChangeInState changeInState)
	{
		// Switch to the next state
		if (changeInState == StatesDefinitions.ChangeInState.NextState)
			SwitchState(nextState);

		// Go back to the last state
		else if (changeInState == StatesDefinitions.ChangeInState.PreviousState)
			SwitchState(prevState);
	}

	/// <summary>
	/// Switch to a different state. Exiting last and Starting the desired state.
	/// </summary>
	/// <param name="state">The state to switch to</param>
	public void SwitchState(StatesDefinitions.GameStates state)
	{
		// First exit the current state.
		if (_activeState != null)
		{
			_activeState.StateExit();
			prevState = activeState;
		}

		// Create a new class for the new state and enter it
		string classDesired = $"States.{state}";
		Debug.Log(classDesired);
		Type t = Type.GetType(classDesired);
		StatesDefinitions.IState newState = (StatesDefinitions.IState) Activator.CreateInstance(t);

		_activeState = newState;
		activeState = state;
		_activeState.StateEnter();
	}

	public void SetNextState(StatesDefinitions.GameStates state)
	{
		nextState = state;
	}


	#endregion


}
