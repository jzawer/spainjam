using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
	public string Name;
	
	public AudioClip Clip;

	[Range(0f, 1f)]
	public float Volume = 1f;

	[Range(0f, 1f)]
	public float SpatialBlend = 1f;

	public bool Loop;

	[HideInInspector]
	public AudioSource Source;

	public Sound(string name)
	{
		Name = name;
	}

	public Sound() { }
}

public class SoundNames
{
	public const string UnresolvedGamePlay = "UnresolvedGamePlay";
	public const string ResolvedGamePlay = "ResolvedGamePlay";
	public const string StartGame = "StartGame";
	public const string CompletedGame = "CompletedGame";
	public const string ValidHorizontalMovement = "ValidHorizontalMovement";
	public const string ValidVerticalMovement = "ValidVerticalMovement";
	public const string InvalidMovement = "InvalidMovement";
	public const string InvalidOperation = "InvalidOperation";
	public const string PlayerMovement = "PlayerMovement";
	public const string PlayerEffect = "PlayerEffect"; 
	public const string GoalWithOutCorrectValue = "GoalWithOutCorrectValue";
	public const string Menu = "Menu";
	public const string CellDown = "CellDown";
	public const string CellUp = "CellUp";
	public const string MenuLoop = "MenuLoop";
}
