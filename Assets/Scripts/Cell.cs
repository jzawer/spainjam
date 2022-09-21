using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
	[HideInInspector]
	public Board ParentBoard;
	[HideInInspector]
	public int Row;
	[HideInInspector]
	public int Column;
	[HideInInspector]
	public int CellSize;
	private int Value;

	public GameObject NumberObject;

	public Number NumberComponent
	{
		get { return NumberObject.GetComponent<Number>(); }
	}

	public int value
	{
		get { return Value; }
		set { updateNumberComponentValue(value); }
	}

	private void updateNumberComponentValue(int newValue)
	{
		Value = newValue;
		if (!NumberComponent || !NumberObject) return;

		NumberObject.SetActive(newValue >= 0);
		NumberComponent.DecimalValue = newValue;
	}
}
