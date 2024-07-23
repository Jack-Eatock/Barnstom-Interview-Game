using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinTheWheelUI : MonoBehaviour
{
    public static SpinTheWheelUI Instance;

	private void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(Instance);
	}

	public void ShowHide(bool show)
	{
		transform.GetChild(0).gameObject.SetActive(show);
	}
}
