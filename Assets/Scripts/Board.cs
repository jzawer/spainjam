using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
	private const string AXIS_VERTICAL = "Vertical";
	private const string AXIS_HORIZONTAL = "Horizontal";

	public GameObject Player;
	public float CellSize = 1f;
	public GameObject CellDefault;
	public GameObject CellWithNumber;
	public GameObject CellWithGoal;
	public TextAsset map;

	private GameObject[,] BoardGrid;
	private int totalRows;
	private int totalColumns;
	private Cell playerCell;
	private float isInMovement;

	protected Cell PlayerCell { 
		get => playerCell;

		set {
			playerCell = value;

			if (playerCell.value == CellTypes.Player)
				playerCell.value = CellTypes.Platform;
		}
	}

	private void Start()
	{
		if (!map || !CellDefault) return;

		GenerateBoardFromMap();
	}

	private void Update()
	{
		// TODO: Cooldown del movimiento
		if (isInMovement != 0f && isInMovement + Time.deltaTime > .4f)
		{
			isInMovement = 0f;
		} else if (isInMovement != 0f)
		{
			isInMovement += Time.deltaTime;
			return;
		}

		Number.CollisionSide collision = Number.CollisionSide.VERTICAL;
		Cell newCell = null;
		if (Input.GetAxisRaw(AXIS_VERTICAL) != 0)
		{
			newCell = GetContinuousCell(PlayerCell, AXIS_VERTICAL, out collision);
		}
		else if (Input.GetAxisRaw(AXIS_HORIZONTAL) != 0)
		{
			newCell = GetContinuousCell(PlayerCell, AXIS_HORIZONTAL, out collision);
		}

		if (!newCell) return;

		if (newCell.value <= 0)
		{
			UpdatePlayer(newCell);
		} else
		{
			newCell.NumberComponent.OnPlayerCollision(Player.GetComponentInChildren<Number>(), collision);
			var playerNumber = Player.GetComponentInChildren<Number>();
			Debug.Log($"{ playerNumber.DecimalValue } || {playerNumber.Digits[0]} {playerNumber.Digits[2]} | {playerNumber.Digits[2]}");
		}

		// TODO: Cooldown del movimiento
		isInMovement = Time.deltaTime;
	}

	public void GenerateBoardFromMap()
	{
		var mapArray = BoardGenerator.MapFromJsonString(map.text);

		this.totalRows = mapArray.GetLength(0);
		this.totalColumns = mapArray.GetLength(1);

		BoardGrid = new GameObject[totalRows, totalColumns];

		for (int row = 0; row < BoardGrid.GetLength(0); row++)
		{
			for (int column = 0; column < BoardGrid.GetLength(1); column++)
			{
				var position = GetWorldPosition(row, column);
				var cellValue = mapArray[row, column];
				GameObject cell;
				if (cellValue == CellTypes.Player)
				{
					cell = Instantiate(CellDefault);
					PlayerCell = cell.GetComponent<Cell>();
					UpdatePlayer(PlayerCell);
				} else if (cellValue == CellTypes.Platform)
				{
					cell = Instantiate(CellDefault);
				} else if (cellValue == CellTypes.Obstacle)
				{
					cell = Instantiate(CellDefault);
				}
				else if (cellValue == CellTypes.Empty)
				{
					cell = null;
				}
				else
				{
					cell = Instantiate(CellWithNumber);
					cell.GetComponent<Cell>().value = cellValue;
				}

				if (cell)
				{
					var cellComponent = cell.GetComponent<Cell>();

					cellComponent.transform.SetParent(this.transform);
					cell.transform.position = position;
					cell.transform.Rotate(90f, 0, 0);
					cellComponent.Row = row;
					cellComponent.Column = column;
					BoardGrid[row, column] = cell;
				}
			}
		}

		if (!PlayerCell)
			PlayerCell = BoardGrid[0, 0].GetComponent<Cell>();

		UpdatePlayer(PlayerCell);
	}

	private Vector3 GetWorldPosition(int column, int row)
	{
		return new Vector3(row, 0, column) * CellSize;
	}

	private Cell GetContinuousCell(Cell currentCell, string axisName, out Number.CollisionSide collision)
	{
		var axisInput = Input.GetAxisRaw(axisName: axisName);
		var newColumn = currentCell.Column;
		var newRow = currentCell.Row;

		if (axisName == AXIS_HORIZONTAL)
		{
			newColumn += (int)axisInput;
		}
		else if (axisName == AXIS_VERTICAL)
		{
			newRow += (int)axisInput;
		}

		collision = axisName == AXIS_VERTICAL ? Number.CollisionSide.VERTICAL : axisInput < 0 ? Number.CollisionSide.RIGHT : Number.CollisionSide.LEFT;

		if (newRow >= totalRows || newRow < 0) return null;
		if (newColumn >= totalColumns || newColumn < 0) return null;

		GameObject cell = BoardGrid[newRow, newColumn];

		if (!cell) return null;

		return BoardGrid[newRow, newColumn].GetComponent<Cell>();
	}

	private void UpdatePlayer(Cell newCell)
	{
		PlayerCell = newCell;
		Player.transform.position = new Vector3(
			newCell.transform.position.x,
			Player.transform.position.y,
			newCell.transform.position.z
		);

		Debug.Log($"PlayerCell: {PlayerCell.Row} : {PlayerCell.Column}");
	}
}
