using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
	public Number NumberComponent { get { return GetComponentInChildren<Number>(); } }
}
