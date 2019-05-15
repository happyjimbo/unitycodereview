using System;
using System.Collections.Generic;
using EventDispatcher;
using IoC;
using UnityEngine;

namespace MatchTileGrid
{
    public class ObstacleTilesModel : IObstacleTilesModel
    {
	    [Inject] public IEventDispatcher eventDispatcher { private get; set; }
	    
        public bool regeneratorObstacleRemoved { get; set; }

		private Dictionary<Vector2, ObstacleTile> obstacleGrid = new Dictionary<Vector2, ObstacleTile>();

		public void AddObstacleTile(ObstacleTile obstacleTile)
		{			
			obstacleGrid [obstacleTile.position] = obstacleTile;	
		}

		public void RemoveObstacleTile(Vector2 key)
		{
			//obstacleGrid.Remove (key);
			ObstacleTile tile = GetMatchObstacle (key);
			tile.type = MatchTileObstacleType.Null;
			tile.obstacleObject = null;
		}

		public ObstacleTile GetMatchObstacle(Vector2 pos)
		{
			if (obstacleGrid.ContainsKey(pos))
			{
				return obstacleGrid[pos];
			}

			return null;
		}

		private List<ObstacleTile> removedObstacles = new List<ObstacleTile>();

		public bool ObstacleAlreadyRemoved(ObstacleTile obstacle)
		{
			return removedObstacles.Contains (obstacle);
		}

		public void RemovedObstacle(ObstacleTile obstacle)
		{
			removedObstacles.Add (obstacle);
		}

		public void ClearRemovedObstacles()
		{
			removedObstacles.Clear();
		}

		public List<ObstacleTile> GetAllObstaclesOfType(MatchTileObstacleType type)
		{
			List<ObstacleTile> obstacles = new List<ObstacleTile> ();
			foreach(KeyValuePair<Vector2, ObstacleTile> entry in obstacleGrid)
			{
				ObstacleTile obstacle = entry.Value;
				if (obstacle.type.Equals(type))
				{
					obstacles.Add (obstacle);
				}
			}

			return obstacles;
		}

		public bool ObstacleRemoveByAdjacentTile(MatchTileObstacleType type)
		{
			switch(type)
			{
				case MatchTileObstacleType.MatchToken_Blocker_1:
				case MatchTileObstacleType.MatchToken_Blocker_2:
				case MatchTileObstacleType.MatchToken_Blocker_3:
				case MatchTileObstacleType.MatchToken_Regenerator:
				case MatchTileObstacleType.MatchToken_Key:
					return true;
			}
			return false;
		}

		public void SwapObstacles(Vector2 firstPos, Vector2 secondPos)
		{
			ObstacleTile firstTile = GetMatchObstacle (firstPos);
			ObstacleTile secondTile = GetMatchObstacle (secondPos);

			firstTile.position = secondPos;
			secondTile.position = firstPos;

			AddObstacleTile (firstTile);
			AddObstacleTile (secondTile);
		}

		public MatchTileObstacleType GetMatchTileObstacleType(string str)
		{
			MatchTileObstacleType[] types = (MatchTileObstacleType[]) Enum.GetValues (typeof(MatchTileObstacleType));
			for (int i = 0; i < types.Length; i++)
			{
				if (types[i].ToString() == str)
				{
					return types [i];
				}
			}
			return MatchTileObstacleType.Null;
		}

		public bool CanMove(Vector2 pos)
		{
			var obstacle = GetMatchObstacle (pos);
			return CanMove(obstacle);
		}
		
		public bool CanMove(ObstacleTile obstacleTile)
		{
			if (obstacleTile != null)
			{
				if (obstacleTile.type == MatchTileObstacleType.MatchToken_Blocker_1 ||
				    obstacleTile.type == MatchTileObstacleType.MatchToken_Blocker_2 ||
				    obstacleTile.type == MatchTileObstacleType.MatchToken_Blocker_3 ||
				    obstacleTile.type == MatchTileObstacleType.MatchToken_Locker_1 ||
				    obstacleTile.type == MatchTileObstacleType.MatchToken_Locker_2 ||
				    obstacleTile.type == MatchTileObstacleType.MatchToken_Locker_3 || 
				    obstacleTile.type == MatchTileObstacleType.MatchToken_Trapper || 
				    obstacleTile.type == MatchTileObstacleType.MatchToken_Regenerator ||
				    obstacleTile.type == MatchTileObstacleType.MatchToken_Key)
				{
					return false;
				}
			}	

			return true;
		}

		public bool CanTouchTile(Vector2 pos)
		{
			if (obstacleGrid [pos].type != MatchTileObstacleType.Null &&
			    obstacleGrid [pos].type != MatchTileObstacleType.MatchToken_Sticker &&
			    obstacleGrid [pos].type != MatchTileObstacleType.MatchToken_Trapper)
			{
				return false;
			}

			return true;
		}
		
		/****************** Obstacles Neeeded ******************/

		private Dictionary<MatchTileObstacleType, int> obstaclesCollected = new Dictionary<MatchTileObstacleType, int> ();
		private Dictionary<MatchTileObstacleType, int> obstaclesNeeded = new Dictionary<MatchTileObstacleType, int> ();

		public void SetObstaclesNeeded(MatchTileObstacleType type, int amount)
		{
			obstaclesNeeded [type] = amount;
		}

		public void AddObstaclesCollected(MatchTileObstacleType type)
		{
			if (!obstaclesCollected.ContainsKey(type))
			{
				obstaclesCollected [type] = 0;
			}

			obstaclesCollected [type]++;

			eventDispatcher.Broadcast (MatchTileGridMessage.OBSTACLE_COLLECTED, type);
		}

		public int GetObstaclesNeeded(MatchTileObstacleType type)
		{
			if (!obstaclesCollected.ContainsKey(type))
			{
				obstaclesCollected [type] = 0;
			}

			return obstaclesNeeded [type] - obstaclesCollected [type];
		}

		private Dictionary<MatchTileObstacleType, List<ObstacleTile>> obstacleOriginalType = new Dictionary<MatchTileObstacleType, List<ObstacleTile>> ();

		public void AddObstacleOriginalType(ObstacleTile obstacle)
		{
			MatchTileObstacleType type = obstacle.type;

			if (!obstacleOriginalType.ContainsKey(type))
			{
				obstacleOriginalType [type] = new List<ObstacleTile> ();
			}

			MatchTileObstacleType previousType = ObstacleOriginalType (obstacle);

			// Don't add again, as we want to keep the original state
			if (!obstacleOriginalType [type].Contains(obstacle) &&
			    previousType == obstacle.type)
			{
				obstacleOriginalType [type].Add (obstacle);	
			}
		}

		public MatchTileObstacleType ObstacleOriginalType(ObstacleTile obstacle)
		{
			foreach (KeyValuePair<MatchTileObstacleType, List<ObstacleTile>> entry in obstacleOriginalType )
			{
				List<ObstacleTile> tiles = entry.Value;

				if (tiles.Contains(obstacle))
				{
					return entry.Key;
				}
			}

			return obstacle.type;
		}
    }
}