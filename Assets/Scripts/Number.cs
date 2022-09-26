using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Number : MonoBehaviour
{
	public enum CollisionSide { VERTICAL, LEFT, RIGHT };

	public int initialValue;

	int decimalValue;
	int[] digits = new int[3];

	Animator animator;
	MusicManager musicManager;
	Board board;

	public int DecimalValue
	{
		get => decimalValue;

		set
		{
			int clampedValue = Mathf.Clamp(value, 0, 7);
			initialValue = clampedValue;

			// update binary representation

			digits[2] = clampedValue % 2 == 1 ? 1 : 0;
			digits[1] = clampedValue / 2 % 2 == 1 ? 1 : 0;
			digits[0] = clampedValue / 2 / 2 % 2 == 1 ? 1 : 0;

			decimalValue = clampedValue;

			UpdateVisuals();
		}
	}

	public int[] Digits
	{
		get => digits;

		set
		{
			UpdateDigits(value);
		}
	}

	[Header("Visuals")]
	public SpriteRenderer[] DigitsRenderer = new SpriteRenderer[3];
	public Sprite Sprite0;
	public Sprite Sprite1;
	public TMP_Text numberText;

	#region TESTING
	[Space(10)]
	[Header("TESTING")]
	public bool[] NewDigits;

	void Update()
	{
		/*if (Input.GetKeyDown(KeyCode.Space))
		    Digits = NewDigits;*/
	}
	#endregion

	void Start()
	{
		animator = GetComponentInParent<Animator>();
		musicManager = FindObjectOfType<MusicManager>();
		board = FindObjectOfType<Board>();

		DecimalValue = initialValue;
		UpdateVisuals();
	}

	public void OnPlayerCollision(Number other, Direction collisionSide)
	{
		string soundToPlay = SoundNames.Interaction_Horizontal;
		string additionalSoundToPlay = "";
		switch (collisionSide)
		{
			case Direction.UP:
			case Direction.DOWN:
				if (other.DecimalValue + DecimalValue <= 7)
				{
					if (other.DecimalValue == 4)
						additionalSoundToPlay = SoundNames.Player_Loose_100;

					other.DecimalValue += DecimalValue;
					DecimalValue = 0;
					soundToPlay = SoundNames.Interaction_Vertical;
					animator.Play(collisionSide == Direction.DOWN ? "UpEffect" : "DownEffect", 0);

					if (other.DecimalValue == 4)
						additionalSoundToPlay = SoundNames.Player_Get_100;
				}
				else
				{
					soundToPlay = SoundNames.Invalid_Action;
				}

				break;
			case Direction.RIGHT:
				if (other.DecimalValue == 4)
					additionalSoundToPlay = SoundNames.Player_Loose_100;

				soundToPlay = SoundNames.Interaction_Horizontal;

				Digits[0] = Digits[0] == 1 ? 0 : 1;

				other.Digits[2] = other.Digits[2] == 1 ? 0 : 1;

				animator.Play("LeftEffect", 0);

				if (other.DecimalValue == 4)
					additionalSoundToPlay = SoundNames.Player_Get_100;
				break;
			case Direction.LEFT:
				if (other.DecimalValue == 4)
					additionalSoundToPlay = SoundNames.Player_Loose_100;
				
				soundToPlay = SoundNames.Interaction_Horizontal;

				Digits[2] = Digits[2] == 1 ? 0 : 1;

				other.Digits[0] = other.Digits[0] == 1 ? 0 : 1;

				animator.Play("RightEffect", 0);

				if (other.DecimalValue == 4)
					additionalSoundToPlay = SoundNames.Player_Get_100;
				break;
			default:
				break;
		}

		UpdateDigits(Digits);
		other.UpdateDigits(other.Digits);

		musicManager.Play(soundToPlay);
		if(additionalSoundToPlay != "")
			musicManager.Play(additionalSoundToPlay);
	}

	void UpdateVisuals()
	{
		//for (int i = 0; i < Digits.Length; i++)
		//{
		//	if (Digits[i])
		//		DigitsRenderer[i].sprite = Sprite1;
		//	else
		//		DigitsRenderer[i].sprite = Sprite0;
		//}

		numberText.text =
			Digits[0].ToString() + Digits[1].ToString() + Digits[2].ToString();
    }

	void UpdateDigits(int[] newDigits)
	{
		// update decimal value

		decimalValue = digits[2] + digits[1] * 2 + digits[0] * 4;

		digits = newDigits;

		UpdateVisuals();
	}
}
