using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Category", menuName = "ScriptableObjects/Category", order = 1)]
public class SO_Category : ScriptableObject
{
	public string Text;
	public List<SO_Question> Questions;
}
