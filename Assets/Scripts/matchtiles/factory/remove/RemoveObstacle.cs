using UnityEngine;
using IoC;
using DG.Tweening;
using EventDispatcher;
using ObjectPool;

namespace MatchTileGrid
{
	public class RemoveObstacle  
	{
		[Inject] public MatchTileGridModel matchTileGridModel { private get; set; }
		[Inject] public IMatchTileFactory matchTileFactory { private get; set; }
		[Inject] public IObstacleTilesModel obstacleTilesModel { private get; set; }
		[Inject] public IObjectPoolModel objectPoolModel { private get; set; }
		[Inject] public IEventDispatcher eventDispatcher { private get; set; }

		public void Remove(ObstacleTile obstacle)
		{
			if (obstacle.type == MatchTileObstacleType.MatchToken_Regenerator)
			{
				obstacleTilesModel.regeneratorObstacleRemoved = true;
			}

			UpdateObstacle (obstacle);

			MatchTile matchTile = matchTileGridModel.GetMatchTile (obstacle.position);
			if (matchTile != null)
			{
				matchTile.canMove = matchTileGridModel.CanMove(matchTile) && obstacleTilesModel.CanMove(obstacle); 	
			}
		}

		private void UpdateObstacle(ObstacleTile obstacle)
		{
			if (obstacle.type == MatchTileObstacleType.MatchToken_Sticker || 
				obstacle.type == MatchTileObstacleType.MatchToken_Blocker_1 ||
				obstacle.type == MatchTileObstacleType.MatchToken_Locker_1 ||
				obstacle.type == MatchTileObstacleType.MatchToken_Trapper ||
				obstacle.type == MatchTileObstacleType.MatchToken_Regenerator ||  
				obstacle.type == MatchTileObstacleType.MatchToken_Key)
			{
				ObstacleCollected (obstacle);	
			}
			else if (obstacle.type == MatchTileObstacleType.MatchToken_Blocker_2)
			{
				PoolAndCreateNew (obstacle, MatchTileObstacleType.MatchToken_Blocker_1);
			} 
			else if (obstacle.type == MatchTileObstacleType.MatchToken_Blocker_3)
			{
				PoolAndCreateNew (obstacle, MatchTileObstacleType.MatchToken_Blocker_2);
			}	
			else if (obstacle.type == MatchTileObstacleType.MatchToken_Locker_2)
			{
				PoolAndCreateNew (obstacle, MatchTileObstacleType.MatchToken_Locker_1);
			}
			else if (obstacle.type == MatchTileObstacleType.MatchToken_Locker_3)
			{
				PoolAndCreateNew (obstacle, MatchTileObstacleType.MatchToken_Locker_2);
			}
		}

		private void PoolAndCreateNew(ObstacleTile obstacle, MatchTileObstacleType type)
		{
			objectPoolModel.PoolObject (obstacle.obstacleObject);

			obstacle.type = type;
			matchTileFactory.CreateObstacle (type, obstacle.position);
		}

		private void ObstacleCollected(ObstacleTile obstacle)
		{
			MatchTileObstacleType type = obstacleTilesModel.ObstacleOriginalType (obstacle);		
			obstacleTilesModel.AddObstaclesCollected (type);

			objectPoolModel.PoolObject (obstacle.obstacleObject);
			obstacleTilesModel.RemoveObstacleTile (obstacle.position);
		}
	}
}