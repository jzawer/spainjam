using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Number : MonoBehaviour
{
	public enum CollisionSide { VERTICAL, LEFT, RIGHT };

	public int initialValue;

	int decimalValue;
	bool[] digits = new bool[3];

	public int DecimalValue
	{
		get => decimalValue;

		set
		{
			int clampedValue = Mathf.Clamp(value, 0, 7);
			initialValue = clampedValue;

			// update binary representation

			digits[2] = clampedValue % 2 == 1;
			digits[1] = clampedValue / 2 % 2 == 1;
			digits[0] = clampedValue / 2 / 2 % 2 == 1;

			decimalValue = clampedValue;

			UpdateVisuals();
		}
	}

	public bool[] Digits
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
		DecimalValue = initialValue;
		UpdateVisuals();
	}

	public void OnPlayerCollision(Number other, CollisionSide collisionSide)
	{
		bool[] otherDigits = other.Digits;
		string soundToPlay = SoundNames.ValidHorizontalMovement;
		switch (collisionSide)
		{
			case CollisionSide.VERTICAL:
				if (other.DecimalValue + DecimalValue <= 7)
				{
					other.DecimalValue += DecimalValue;
					DecimalValue = 0;
					soundToPlay = SoundNames.ValidVerticalMovement;
				}
				else
				{
					soundToPlay = SoundNames.InvalidOperation;
					Debug.Log("Invalid operation");
				}

				break;
			case CollisionSide.LEFT:
				Digits[0] = !Digits[0];

				other.Digits[2] = !otherDigits[2];
				break;
			case CollisionSide.RIGHT:
				Digits[2] = !Digits[2];

				other.Digits[0] = !otherDigits[0];
				break;
			default:
				break;
		}

		UpdateDigits(Digits);
		other.UpdateDigits(other.Digits);

		var musicManager = FindObjectOfType<MusicManager>();
		if (musicManager)
			musicManager.Play(soundToPlay);
	}

	void UpdateVisuals()
	{
		for (int i = 0; i < Digits.Length; i++)
		{
			if (Digits[i])
				DigitsRenderer[i].sprite = Sprite1;
			else
				DigitsRenderer[i].sprite = Sprite0;
		}
	}

	void UpdateDigits(bool[] newDigits)
	{
		// update decimal value

		int newValue = 0;
		if (digits[2])
			newValue += 1;
		if (digits[1])
			newValue += 2;
		if (digits[0])
			newValue += 4;

		DecimalValue = newValue;

		digits = newDigits;

		UpdateVisuals();
	}
}
