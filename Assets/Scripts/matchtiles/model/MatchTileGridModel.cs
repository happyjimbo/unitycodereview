using System;
using UnityEngine;
using System.Collections.Generic;
using IoC;

namespace MatchTileGrid
{
	public class MatchTileGridModel : IMatchTileGridModel
	{
		// models should not reference other models - remove this.
		[Inject] public IObstacleTilesModel obstacleTilesModel { private get; set; }
		
		
		/****************** Ready Only ******************/

		// Hardcoded for the sake of this code demo, this is normally 
		// tied to the level that the player has loaded in.
		private readonly string _matchTileLayout = "MatchTileGridData/07_MatchTileLayout_9x7";
		public string matchTileLayout { get { return _matchTileLayout; } }

		private readonly string _matchTileLocation = "Prefabs/2DSprites/MatchTokens/";
		public string matchTileLocation { get { return _matchTileLocation; } }

		private readonly string _gameHolderName = "GameHolder";
		public string gameHolderName { get { return _gameHolderName; } }

		private readonly int _minTouchRequired = 3;
		public int minTouchRequired { get { return _minTouchRequired; } }

		private readonly float _matchTileOriginalScale = 0.75f;
		public float matchTileOriginalScale { get { return _matchTileOriginalScale; } }

		private readonly float _moveSpeed = 0.125f;
		public float moveSpeed { get { return _moveSpeed; } }

		private readonly float _gridYPos = -2f;
		public float gridYPos { get { return _gridYPos; } }

		private readonly int _secondsUntilHint = 5;
		public int secondsUntilHint { get { return _secondsUntilHint; } }

		/****************** Public Properties ******************/

		public bool pauseTilesFalling { get; set; }

		public GameObject gridHolder { get; set; }
		public GameObject gridParent { get; set; }
		public Vector2 gridSize { get; set; }
		public int movesRemaining { get; set; }
		public int tilesToReplace { get; set; }

		private bool _allowTouch = true;
		public bool allowTouch 
		{
			get { return _allowTouch; }
			set { _allowTouch = value; }
		}
		
		public int GetTrapperAmount()
		{
			return 10; // Need to get data from MatchTileData eventually
		}

		/****************** matchTileGrid ******************/

		public MatchTilesData matchTilesData { get; set; }

		private Dictionary<Vector2, MatchTile> matchTileGrid = new Dictionary<Vector2, MatchTile>();

		public void AddNewTile(MatchTile tile)
		{
			matchTileGrid [tile.position] = tile;
		}

		public void RemoveTile(Vector2 tileKey)
		{
			matchTileGrid.Remove (tileKey);
		}

		public MatchTile GetMatchTile(Vector2 pos)
		{
			if (matchTileGrid.ContainsKey(pos))
			{
				return matchTileGrid[pos];
			}

			return null;
		}

		public Dictionary<Vector2, MatchTile> GetMatchTiles()
		{
			return matchTileGrid;
		}

		public void MoveTile(Vector2 currentPosition, Vector2 newPosition)
		{
			MatchTile tile = GetMatchTile (currentPosition);
			if (tile != null && tile.canMove)
			{
				RemoveTile (currentPosition);

				tile.position = newPosition;

				AddNewTile (tile);
			}
		}
		
		public void SwapTiles(Vector2 firstPos, Vector2 secondPos)
		{
			MatchTile firstTile = GetMatchTile (firstPos);
			MatchTile secondTile = GetMatchTile (secondPos);
			if (firstTile != null && secondTile != null)
			{
				RemoveTile (firstPos);
				RemoveTile (secondPos);

				firstTile.position = secondPos;
				secondTile.position = firstPos;

				AddNewTile (firstTile);
				AddNewTile (secondTile);
			}
		}

		public List<MatchTile> GetAllMatchTilesNotOfType(MatchTileType type)
		{
			List<MatchTile> tiles = new List<MatchTile> ();

			foreach(KeyValuePair<Vector2, MatchTile> entry in GetMatchTiles())
			{
				MatchTile tile = entry.Value;

				if (!tile.type.Equals(type) && tile.canTouch)
				{
					tiles.Add (tile);
				}
			}

			return tiles;
		}

		public MatchTileType GetMatchTileType(string str)
		{
			MatchTileType[] types = (MatchTileType[]) Enum.GetValues (typeof(MatchTileType));
			for (int i = 0; i < types.Length; i++)
			{
				if (types[i].ToString() == str)
				{
					return types [i];
				}
			}
			return MatchTileType.Null;
		}
		
		public List<MatchTile> GetAllMatchTilesOfType(MatchTileType type)
		{
			List<MatchTile> tiles = new List<MatchTile> ();

			foreach(KeyValuePair<Vector2, MatchTile> entry in GetMatchTiles())
			{
				MatchTile tile = entry.Value;

				if (tile.type.Equals(type))
				{
					tiles.Add (tile);
				}
			}

			return tiles;
		}

		public IMatchTileComponent GetMatchTileComponent(MatchTile tile)
		{
			return tile.tileObject.GetComponent (typeof(IMatchTileComponent)) as IMatchTileComponent;
		}

		/****************** tilesTouched ******************/

		public float lastTouchedTimestamp { get; set; }
		public MatchTileType selectedMatchTileType { get; private set; }

		private List<MatchTile> tilesTouched = new List<MatchTile>();
		private Vector2 lastMatchTileAddedPos;

		public void AddTileTouched(Vector2 pos, MatchTile matchTile)
		{
			if (tilesTouched.Count == 0)
			{
				selectedMatchTileType = matchTile.type;
			}

			tilesTouched.Add(matchTile);
			lastMatchTileAddedPos = pos;
		}

		public List<MatchTile> GetTilesTouched()
		{
			return tilesTouched;
		}

		public void RemoveTileTouched(MatchTile tile)
		{
			tilesTouched.Remove (tile);
			Vector2 prevousPos = tilesTouched [tilesTouched.Count - 1].position;
			lastMatchTileAddedPos = prevousPos;
		}

		public void ClearTilesTouched()
		{
			tilesTouched.Clear ();
		}

		public bool CanTouchTile(MatchTile matchTile)
		{
			if (matchTile != null)
			{
				Vector2 pos = matchTile.position;

				var obstacle = obstacleTilesModel.CanTouchTile(pos);
				if (!obstacle)
				{
					return false;
				}

				if (tilesTouched.Count == 0 && matchTile.canTouch)
				{
					return true;
				} 
				else if (matchTile.type.Equals (selectedMatchTileType) &&
				        !tilesTouched.Contains (matchTile) &&
				        IsNearPreviousTile (pos) &&
				        matchTile.canTouch)
				{
					return true;
				}
			}

			return false;
		}

		public bool ValidMoveTouchEnded()
		{
			return tilesTouched.Count >= minTouchRequired;
		}

		private bool IsNearPreviousTile(Vector2 pos)
		{
			Vector2 offset = pos - lastMatchTileAddedPos;

			if ((offset.x <= 1f && offset.x >= -1f) &&
				(offset.y <= 1f && offset.y >= -1f))
			{
				return true;
			}
			return false;
		}

		/****************** Hint ******************/

		private List<MatchTile> hintTiles = new List<MatchTile>();

		public void AddHintMatchTile(MatchTile tile)
		{
			hintTiles.Add (tile);
		}

		public List<MatchTile> GetHintMatchTiles()
		{
			return hintTiles;
		}

		public void ClearHintMatchTiles()
		{
			hintTiles.Clear ();
		}

		/****************** Grid Scanning ******************/

		public bool CheckIfMoveAbove(Vector2 currentPos)
		{
			for (int y = (int)currentPos.y; y < gridSize.y; y++)
			{
				Vector2 pos = new Vector2 (currentPos.x, y);

				MatchTile matchTile = GetMatchTile (pos);
				if (matchTile != null)
				{
					if (!CanMove(matchTile) && obstacleTilesModel.CanMove(pos))
					{
						return false;
					}

					return true;
				}
			}

			return true;
		}
		
		public MatchTile GetRandomTileSpace()
		{
			int ranX = UnityEngine.Random.Range (0, (int)gridSize.x);
			int ranY = UnityEngine.Random.Range (0, (int)gridSize.y);

			Vector2 pos = new Vector2 (ranX, ranY);
			MatchTile tile = null;

			if (matchTileGrid.ContainsKey(pos))
			{
				tile = matchTileGrid [pos];
			}

			int count = 0;
			int gridAmount = (int)gridSize.x * (int)gridSize.y;

			while (tile == null || !ValidMatchTile(tile.type) && count <= gridAmount)
			{
				count++;

				ranX = UnityEngine.Random.Range (0, (int)gridSize.x);
				ranY = UnityEngine.Random.Range (0, (int)gridSize.y);

				pos = new Vector2 (ranX, ranY);
				if (matchTileGrid.ContainsKey(pos))
				{
					tile = matchTileGrid [pos];
				}
			}

			return matchTileGrid[pos];
		}

		public bool CanMove(MatchTile tile)
		{
			if (tile != null)
			{
				if (!ValidMatchTile(tile.type))
				{
					return false;
				}
			}

			return true;
		}

		public bool ValidMatchTile(MatchTileType type)
		{
			if (type == MatchTileType.RemainingClearance ||
				type == MatchTileType.Blank ||
				type == MatchTileType.Null)
			{
				return false;
			}

			return true;
		}
	}
}