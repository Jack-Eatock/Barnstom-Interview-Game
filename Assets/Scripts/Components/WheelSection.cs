using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WheelSection : MonoBehaviour
{
    [SerializeField]
    private Transform line1, line2;
    private Image image1, image2;
    [SerializeField]
    private TextMeshProUGUI labelText;
    [SerializeField]
    private Transform textHolder;
    private SO_Category category;
    private Color defaultColour;
    private float startAngle = 0, endAngle = 0;

    public SO_Category Category => category;

	private void Awake()
	{
		image1 = line1.GetComponent<Image>();
        image2 = line2.GetComponent<Image>();
        defaultColour = image1.color;
	}

	public void Setup(float degreesPerSection, int offset, int numElements)
    {
        category = GameManager.Instance.GameConfig.Categories[offset];
        labelText.text = category.Text;
        startAngle = degreesPerSection * offset;
        endAngle = degreesPerSection * (offset + 1);
		labelText.fontSize = 36 - (numElements * 2);
		textHolder.transform.localRotation = Quaternion.Euler(0, 0, degreesPerSection / 2);
		line2.transform.localRotation = Quaternion.Euler(0, 0, degreesPerSection);
		transform.rotation = Quaternion.Euler(0, 0, degreesPerSection * offset);
		gameObject.SetActive(true);
    }

    /// <summary>
    /// Checks if the this section of the wheel has been landed on.
    /// </summary>
    public bool AngleInBounds(float angle)
    {
        return angle > startAngle && angle < endAngle;
    }

    /// <summary>
    /// Make this section "Shine", the lines and text change colour.
    /// </summary>
    public void Shine(bool toggle)
    {
        if (toggle)
        {
            transform.SetAsLastSibling();
			labelText.color = GameManager.Instance.GoodColour; 
			image1.color = GameManager.Instance.GoodColour;
			image2.color = GameManager.Instance.GoodColour; 
		}
        else
        {
			labelText.color = defaultColour;
			image1.color = defaultColour;
			image2.color = defaultColour;
		}
    }
}
