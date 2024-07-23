using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public static InputHandler Instance;
	private Inputs inputs;

	// Actions
	private InputAction primaryDownAction;
	private InputAction primaryPosAction;

	[SerializeField]
	private float interactionTimeUntilDrag, movementToTriggerDrag;

	private Vector2 primaryPos, lastPrimaryPos;
	private bool primaryInteractionIsDragging;
	private float primaryInteractionStartTime;
	private IEnumerator checkingPrimaryInteraction;

	#region Getters


	public Vector2 PrimaryPos => primaryPos;

	#endregion

	private void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(this.gameObject);

		// Create inputs class and enable action map to start listening to events
		inputs = new Inputs();
		inputs.Default.Enable();

		// Link actions
		primaryDownAction = inputs.Default.PrimaryDown;
		primaryPosAction  = inputs.Default.PrimaryPos;
	}

	private void OnEnable()
	{
		// Link action events to appropriate functions
		primaryDownAction.performed += PrimaryDownAction_performed;
		primaryPosAction.performed += PrimaryPosAction_performed; 
	}

	private void OnDisable()
	{
		primaryDownAction.performed -= PrimaryDownAction_performed;
		primaryPosAction.performed -= PrimaryPosAction_performed; 
	}

	private void PrimaryPosAction_performed(InputAction.CallbackContext ctx)
	{
		primaryPos = ctx.ReadValue<Vector2>();
	}

	private void PrimaryDownAction_performed(InputAction.CallbackContext ctx)
	{
		if (checkingPrimaryInteraction != null)
			StopCoroutine(checkingPrimaryInteraction);

		checkingPrimaryInteraction = PrimaryCheckForClickOrDrag();
		StartCoroutine(checkingPrimaryInteraction);
	}

	/// <summary>
	/// Check if they are clicking or dragging
	/// </summary>
	/// <returns></returns>
	private IEnumerator PrimaryCheckForClickOrDrag()
	{
		lastPrimaryPos = primaryPos;
		primaryInteractionIsDragging = false;
		primaryInteractionStartTime = Time.time;

		// Happened this frame, wait for next.
		yield return new WaitForEndOfFrame();

		// If mobile, touch event is delayed, so the last position needs to be updated on the frame after before checks.
#if UNITY_IOS || UNITY_ANDROID
		lastPrimaryPos = primaryPos;
#endif

		while (true)
		{
			// They have released their finger or mouse this frame. Check if drag release or click.
			if (primaryDownAction.WasReleasedThisFrame())
			{
				// Finished dragging
				if (primaryInteractionIsDragging)
					PrimaryInteractionDragReleased(); 

				// Clicked
				else
					PrimaryInteractionClicked();

				primaryInteractionIsDragging = false;
				StopCoroutine(checkingPrimaryInteraction);
			}

			// They have clicked or put their finger down. Have they been down for long enough or moved enough to switch from a click to a drag?
			else
			{
				bool mouseMovedStartDrag = (Vector2.Distance(lastPrimaryPos, primaryPos) > movementToTriggerDrag);
				bool mouseDownLongEnoughToDrag = (Time.time - primaryInteractionStartTime) >= interactionTimeUntilDrag;
				if (primaryInteractionIsDragging || mouseDownLongEnoughToDrag || mouseMovedStartDrag)
				{
					if (!primaryInteractionIsDragging)
						primaryInteractionIsDragging = true;

					PrimaryInteractionDragging(); // Dragging
				}
			}
			yield return new WaitForEndOfFrame();
		}
	}

	private void PrimaryInteractionDragging()
	{
		GameManager.Instance.CheckIfStateShouldChange(GameManager.Instance.ActiveState.InteractionDragging());
	}

	private void PrimaryInteractionDragReleased()
	{
		GameManager.Instance.CheckIfStateShouldChange(GameManager.Instance.ActiveState.InteractionDraggingReleased());
	}

	private void PrimaryInteractionClicked()
	{
		GameManager.Instance.CheckIfStateShouldChange(GameManager.Instance.ActiveState.InteractionClicked());
	}

	public List<RaycastResult> GetEventSystemRaycastResults()
	{
		PointerEventData eventData = new PointerEventData(EventSystem.current);
		eventData.position = primaryPos;
		List<RaycastResult> raycastResults = new List<RaycastResult>();
		EventSystem.current.RaycastAll(eventData, raycastResults);
		return raycastResults;
	}
}
