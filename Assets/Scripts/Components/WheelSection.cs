using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WheelSection : MonoBehaviour
{
    [SerializeField]
    Transform line1, line2;
    [SerializeField]
    private TextMeshProUGUI labelText;
    [SerializeField]
    private Transform textHolder;
    private SO_Category category;

    public void Setup(float degreesPerSection, int offset, int numElements)
    {
        category = GameManager.Instance.GameConfig.Categories[offset];
        labelText.text = category.Text;

		labelText.fontSize = 36 - (numElements * 2);

		textHolder.transform.localRotation = Quaternion.Euler(0, 0, degreesPerSection / 2);
		line2.transform.localRotation = Quaternion.Euler(0, 0, degreesPerSection);
		transform.rotation = Quaternion.Euler(0, 0, degreesPerSection * offset);
		gameObject.SetActive(true);
    }
}
