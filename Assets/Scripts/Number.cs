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

		DecimalValue = initialValue;
		UpdateVisuals();
	}

	public void OnPlayerCollision(Number other, CollisionSide collisionSide)
	{
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
				Digits[0] = Digits[0] == 1 ? 0 : 1;

				other.Digits[2] = other.Digits[2] == 1 ? 0 : 1;

				animator.Play("LeftEffect", 0);
				break;
			case CollisionSide.RIGHT:
				Digits[2] = Digits[2] == 1 ? 0 : 1;

				other.Digits[0] = other.Digits[0] == 1 ? 0 : 1;

				animator.Play("RightEffect", 0);
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
