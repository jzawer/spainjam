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
	public const string Player_Move = "Player_Move";
	public const string Player_Waiting = "Player_Waiting";
	public const string Interaction_Horizontal = "Interaction_Horizontal";
	public const string Interaction_Vertical = "Interaction_Vertical";
	public const string Invalid_Action = "Invalid_Action";
	public const string Number_Up = "Number_Up";
	public const string Number_Down = "Number_Down";
	public const string Level_Start = "Level_Start";
	public const string Player_Get_100 = "Player_Get_100";
	public const string Player_Loose_100 = "Player_Loose_100";
	public const string Level_Completed = "Level_Completed"; 
	public const string StartMenu_Play = "StartMenu_Play";
	public const string StartMenu_Hover = "StartMenu_Hover";
	public const string StartMenu_Quit = "StartMenu_Quit";
	public const string StartMenu_Start = "StartMenu_Start";
	public const string StartMenu_Loop = "StartMenu_Loop";
	public const string Gameplay_UnresolvedLoop = "Gameplay_UnresolvedLoop";
	public const string Gameplay_ResolvedLoop = "Gameplay_ResolvedLoop";
}
