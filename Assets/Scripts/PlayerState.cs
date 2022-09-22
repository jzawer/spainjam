using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
	public Number numberComponent;
	public int DecimalValue { get { return numberComponent != null ? numberComponent.DecimalValue : 0; } }

	private void Start()
	{
		numberComponent = GetComponentInChildren<Number>();
	}
}
