using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class Board : MonoBehaviour
{
	private const string AXIS_VERTICAL = "Vertical";
	private const string AXIS_HORIZONTAL = "Horizontal";

	[Header("Board Generation")]
	public float CellSize = 1f;
	public GameObject CellDefault;
	public GameObject CellWithNumber;
	public GameObject CellWithGoal;
	public TextAsset map;
	[Header("Player Movement")]
	public GameObject Player;
	public float PlayerMoveDuration;

	private GameObject[,] BoardGrid;
	private int totalRows;
	private int totalColumns;
	private Cell playerCell;
	private List<CellMovement> NextMovementCells = new List<CellMovement>();
	private bool playerIsMoving;
	private MusicManager musicManager;
	private List<Cell> currentNeighbours = new List<Cell>();
	Animator playerAnimator;

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
		playerAnimator = Player.GetComponent<Animator>();

		if (!map || !CellDefault) return;

		musicManager = MusicManager.Instance;
		GenerateBoardFromMap();

		if (!musicManager) return;

		musicManager.Play(SoundNames.Level_Start);
		musicManager.Play(SoundNames.Gameplay_UnresolvedLoop);
		musicManager.Play(SoundNames.Player_Waiting);
	}

	private void Update()
	{
		if (NextMovementCells.Count >= 2)
		{
			if (!playerIsMoving)
				DoNextMovement();

			return;
		}

		CellMovement nextMovement = null;
		Cell playerCell = NextMovementCells.Count > 0 ? NextMovementCells[^1].cell : PlayerCell;

		if (Input.anyKeyDown)
		{
			var verticalAxisInput = Input.GetAxisRaw(AXIS_VERTICAL);
			var horizontalAxisInput = Input.GetAxisRaw(AXIS_HORIZONTAL);

			if (verticalAxisInput != 0)
			{
				nextMovement = GetContinuousCell(playerCell, AXIS_VERTICAL);
			}
			else if (horizontalAxisInput != 0)
			{
				nextMovement = GetContinuousCell(playerCell, AXIS_HORIZONTAL);
			}
		}

		if (nextMovement != null && nextMovement.cell != null)
			NextMovementCells.Add(nextMovement);

		if (playerIsMoving || NextMovementCells.Count <= 0) return;

		DoNextMovement();
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

				switch (cellValue)
				{
					case CellTypes.Player:
						cell = Instantiate(CellDefault);
						cell.GetComponent<Cell>().value = cellValue;
						PlayerCell = cell.GetComponent<Cell>();
						break;

					case CellTypes.Platform:
						cell = Instantiate(CellDefault);
						break;
					case CellTypes.Goal:
						cell = Instantiate(CellWithGoal);
						cell.GetComponentInChildren<Canvas>().worldCamera = Camera.main;
						break;
					case CellTypes.Obstacle:
					case CellTypes.Empty:
						cell = null;
						break;
					default:
						cell = Instantiate(CellWithNumber);
						cell.GetComponentInChildren<Canvas>().worldCamera = Camera.main;
						break;
				}

				if (cell)
				{
					var cellComponent = cell.GetComponent<Cell>();
					cell.GetComponent<Cell>().value = cellValue;
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

		SetPlayerPosition(PlayerCell);
	}

	private Vector3 GetWorldPosition(int column, int row)
	{
		return new Vector3(row, 0, column) * CellSize;
	}

	private CellMovement GetContinuousCell(Cell currentCell, string axisName)
	{
		var cellMovement = new CellMovement();
		var axisInput = Input.GetAxisRaw(axisName: axisName);
		var newColumn = currentCell.Column;
		var newRow = currentCell.Row;

		if (axisName == AXIS_HORIZONTAL)
		{
			newColumn += (int)axisInput;

			cellMovement.direction = axisInput > 0 ? Direction.RIGHT : Direction.LEFT;
		}
		else if (axisName == AXIS_VERTICAL)
		{
			newRow += (int)axisInput;

			cellMovement.direction = axisInput > 0 ? Direction.UP : Direction.DOWN;
		}

		cellMovement.collisionSide = axisName == AXIS_VERTICAL ? Number.CollisionSide.VERTICAL : axisInput < 0 ? Number.CollisionSide.RIGHT : Number.CollisionSide.LEFT;

		if (newRow >= totalRows || newRow < 0) return cellMovement;
		if (newColumn >= totalColumns || newColumn < 0) return cellMovement;

		GameObject cell = BoardGrid[newRow, newColumn];

		if (!cell) return cellMovement;

		cellMovement.cell = BoardGrid[newRow, newColumn].GetComponent<Cell>();

		return cellMovement;
	}

	private void SetPlayerPosition(Cell newCell)
	{
		Player.transform.position = new(newCell.transform.position.x, Player.transform.position.y, newCell.transform.position.z);

		CheckNeighboursCells(newCell);
		PlayerCell = newCell;

		Debug.Log($"PlayerCell: {PlayerCell.Row} : {PlayerCell.Column}");
	}

	private void UpdatePlayer(Cell newCell, Direction moveDirection)
	{
		if (playerIsMoving)
			return;

		Vector3 newPosition = new(newCell.transform.position.x, Player.transform.position.y, newCell.transform.position.z);

        #region MOVE PLAYER
        playerIsMoving = true;
        switch (moveDirection)
        {
            case Direction.RIGHT:
				playerAnimator.SetTrigger("MoveRight");
                break;
            case Direction.LEFT:
				playerAnimator.SetTrigger("MoveLeft");
                break;
            case Direction.UP:
				playerAnimator.SetTrigger("MoveUp");
                break;
            case Direction.DOWN:
				playerAnimator.SetTrigger("MoveDown");
                break;
            default:
                break;
        }
        musicManager.Play(SoundNames.Player_Move);
		Tween playerMoveTween = Player.transform.DOMove(newPosition, PlayerMoveDuration).OnComplete(() =>
		{
			playerIsMoving = false;

			// check if cell is goal
			if (newCell.value == CellTypes.Goal)
			{
				if (Player.GetComponent<PlayerState>().DecimalValue == 4)
                {
					if (musicManager)
                    {
						musicManager.Stop(SoundNames.Gameplay_ResolvedLoop);
						musicManager.Play(SoundNames.Level_Completed);
					}

					FindObjectOfType<ScenesManager>().Win();
				}
				else {
					/*if (musicManager)
						musicManager.Play(SoundNames.GoalWithOutCorrectValue);*/
					Debug.Log("I need you to be 100!");
				}
			}
            CheckNeighboursCells(newCell);
		});
        #endregion

		PlayerCell = newCell;
	}

	public void ToggleMusic()
	{
		if (musicManager == null)
			return;

		if (Player.GetComponent<PlayerState>().DecimalValue == 4)
		{
			//musicManager.Play(SoundNames.Player_Get_100);
			musicManager.ChangeClipTo(SoundNames.Gameplay_UnresolvedLoop, SoundNames.Gameplay_ResolvedLoop, 1f);
		} else
		{
			musicManager.ChangeClipTo(SoundNames.Gameplay_ResolvedLoop, SoundNames.Gameplay_UnresolvedLoop, 1f);
		}
	}

	private void DoNextMovement()
	{
		var nextMovementCell = NextMovementCells[0];
		if (nextMovementCell.cell == null || nextMovementCell.cell.value == CellTypes.Obstacle)
		{
			if (musicManager)
				musicManager.Play(SoundNames.Invalid_Action);
			return;
		}

		// move Player to new Cell or interact with it (in case of Number)
		if (nextMovementCell.cell.value < 0)
		{
			UpdatePlayer(nextMovementCell.cell, nextMovementCell.direction);
		}
		else
		{
			nextMovementCell.cell.NumberComponent.OnPlayerCollision(Player.GetComponentInChildren<Number>(), nextMovementCell.direction);
			ToggleMusic();
		}

		NextMovementCells.RemoveAt(0);
	}

	private void CheckNeighboursCells(Cell cell)
	{
		List<Cell> newNeighbours = new List<Cell>();
		for (int r = -1; r <= 1; r++)
		{
			for (int c = -1; c <= 1; c++)
			{
				if (r == 0 && c == 0) continue;

				var neighbourRow = cell.Row + c;
				var neighbourColumn = cell.Column + r;

				if (neighbourRow < 0 || neighbourRow >= totalRows || neighbourColumn < 0 || neighbourColumn >= totalColumns) continue;
				var cellObject = BoardGrid[neighbourRow, neighbourColumn];

				if (cellObject == null) continue;
				var neighbourCell = cellObject.GetComponent<Cell>();

				if (neighbourCell == cell)
					continue;

				if (neighbourCell.value >= 0)
                {
					newNeighbours.Add(neighbourCell);
                }
			}
		}

		newNeighbours.ForEach(neighbour => UpdateNeighbourState(neighbour, true));
		currentNeighbours.Where(neighbour => !newNeighbours.Any(c => c.Row == neighbour.Row && c.Column == neighbour.Column)).ToList().ForEach(neighbour => UpdateNeighbourState(neighbour, false));

		currentNeighbours = newNeighbours;
	}

	private void UpdateNeighbourState(Cell neighbour, bool up)
    {
		var animator = neighbour.GetComponent<Animator>();
		if (animator == null) return;

		if (musicManager && animator.GetBool("Up") != up)
			musicManager.Play(up ? SoundNames.Number_Up : SoundNames.Number_Down);

		animator.SetBool("Up", up);
	}
}

