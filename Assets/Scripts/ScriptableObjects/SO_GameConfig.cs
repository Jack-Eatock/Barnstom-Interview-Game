using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Config", menuName = "ScriptableObjects/Config", order = 1)]
public class SO_GameConfig : ScriptableObject
{
	public List<SO_Category> Categories = new List<SO_Category>();
}

