using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Wheel : MonoBehaviour
{
	[SerializeField]
	private WheelSection wheelLine;
	[SerializeField]
	private Transform lineHolder;

	private List<WheelSection> wheelLines = new List<WheelSection>();
	private float degreesPerSection;
	private float velocity = 0;
	private float angle = 0;

	[SerializeField]
	private float lerpSpeed = 5;
	[SerializeField]
	private float spinSpeed = 3;
	[SerializeField]
	private AnimationCurve velocityReductionRateGraph;
	[SerializeField]
	private float velocityReduction = 100;
	[SerializeField]
	private float maxVelocity = 300;
	private Color defaultColour;
	private Image outlineImage;
	private float lastAngle;

	public float Velocity => velocity;

	private void FixedUpdate()
	{
		// Rotate from 0 to 360 based on the velocity.
		angle -= velocity * Time.fixedDeltaTime * spinSpeed; // Mathf.Lerp(0, 360, fraction);
		ClampAngle();

		// every time the angle passes x degrees play a sound.
		if (Mathf.Abs(angle - lastAngle) > GameManager.Instance.DegreesPerCategory)
		{
			lastAngle = angle;
			AudioManager.Instance.PlayClip("UiClick", 1);
		}

		Quaternion desiredRot = Quaternion.Euler(0, 0, angle);
		transform.rotation = Quaternion.Lerp(transform.rotation, desiredRot, Time.fixedDeltaTime * lerpSpeed);
		
		if (Mathf.Abs(velocity) < 1)
			velocity = 0;
		else
		{
			float val = Mathf.Clamp (Mathf.Abs(velocity) / maxVelocity, 0, 1);
			
			if (velocity < 0)
				velocity = Mathf.Clamp(velocity + velocityReductionRateGraph.Evaluate(val) * velocityReduction * Time.fixedDeltaTime, -maxVelocity, 0);
			
			else
				velocity = Mathf.Clamp(velocity - velocityReductionRateGraph.Evaluate(val) * velocityReduction * Time.fixedDeltaTime, 0, maxVelocity);
		}
			
	}

	private void ClampAngle()
	{
		if (angle > 360)
			angle = angle % 360;

		else if (angle < 0)
		{
			float val = Mathf.Abs(angle);
			angle = 360 - (val % 360);
		}
	}

	private void Awake()
	{
		outlineImage = GetComponent<Image>();
		defaultColour = outlineImage.color;
	}

	public void SetupWheel()
	{
		Debug.Log("Setup wheel " + GameManager.Instance.NumCategories);

		transform.rotation = Quaternion.identity;
		lastAngle = 0;

		// Check we have enough lines pooled. Otherwise spawn more to fill the gaps.
		int numWheelLines = wheelLines.Count;
		if (numWheelLines < GameManager.Instance.NumCategories)
		{
			for (int i = 0; i < GameManager.Instance.NumCategories - numWheelLines; i++)
			{
				WheelSection newWheelLine = Instantiate(wheelLine, lineHolder);
				wheelLines.Add(newWheelLine);
			}
		}

		// Hide all the lines
		DisableLines();

		degreesPerSection = GameManager.Instance.DegreesPerCategory;
		Debug.Log(degreesPerSection);

		// Iterate over the options and place the lines accordingly.
		for (int i = 0; i < GameManager.Instance.NumCategories; i++)
			wheelLines[i].Setup(degreesPerSection, i, GameManager.Instance.NumCategories);
	}

	public void ApplySpin(float force)
	{
		velocity = force;
	}

	private void DisableLines()
	{
		foreach (Transform line in lineHolder)
			line.gameObject.SetActive(false);
	}

	public WheelSection WhichWheelSectionDidWeLandOn()
	{
		// Iterate over the bounds of each section and see if within.
		for (int i = 0; i < GameManager.Instance.NumCategories; i++)
		{
			if (wheelLines[i].AngleInBounds(360 - angle))
				return wheelLines[i];
		}

		Debug.LogError("COULD NOT FIND SECTION");
		return null;
	}

	public void Shine(bool toggle)
	{
		if (toggle)
			outlineImage.color = GameManager.Instance.GoodColour;
			
		else
			outlineImage.color = defaultColour;
	}
}
