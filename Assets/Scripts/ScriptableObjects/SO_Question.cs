using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Question", menuName = "ScriptableObjects/Question", order = 1)]
public class SO_Question : ScriptableObject
{
	public string Text;
	public SO_Answer CorrectAnswer;
	public List<SO_Answer> IncorrectAnswers;
}
