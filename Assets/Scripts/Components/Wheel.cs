using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
	[SerializeField]
	private WheelSection wheelLine;
	[SerializeField]
	private Transform lineHolder;

	private List<WheelSection> wheelLines = new List<WheelSection>();
	private float degreesPerSection;

	public void SetupWheel()
	{
		Debug.Log("Setup wheel " + GameManager.Instance.NumCategories);

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

		degreesPerSection = (float) 360 / GameManager.Instance.NumCategories;
		Debug.Log(degreesPerSection);

		// Iterate over the options and place the lines accordingly.
		for (int i = 0; i < GameManager.Instance.NumCategories; i++)
		{
			wheelLines[i].Setup(degreesPerSection, i, GameManager.Instance.NumCategories);
		}
			
	}

	private void DisableLines()
	{
		foreach (Transform line in lineHolder)
			line.gameObject.SetActive(false);
	}
}
