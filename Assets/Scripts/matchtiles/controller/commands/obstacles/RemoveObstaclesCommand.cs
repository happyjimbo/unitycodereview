using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using IoC;
using Command;
using DG.Tweening;

namespace MatchTileGrid
{
	public class RemoveObstaclesCommand : ICommand
	{
		[Inject] public IObstacleTilesModel obstacleTilesModel { private get; set; }
		[Inject] public IMatchTileFactory matchTileFactory { private get; set; }

		public RemoveTile removeTile { private get; set; }

		public void Execute()
		{
			RemoveNearByObstacles ();
		}

		private void RemoveNearByObstacles()
		{
			for (int i = 0; i < removeTile.matchTiles.Count; i++)
			{
				MatchTile tile = removeTile.matchTiles [i];
				ObstacleTile obstacle = obstacleTilesModel.GetMatchObstacle (tile.position);

				if (obstacle.type != MatchTileObstacleType.MatchToken_Trapper)
				{
					CheckForNearbyObstaclesToRemove (tile.position);	
				}
			}	
		}

		private void CheckForNearbyObstaclesToRemove(Vector2 currentPos)
		{
			ObstacleTile obstacle = obstacleTilesModel.GetMatchObstacle (currentPos);
			RemoveObstacle (obstacle, true);

			// We only want tiles that have been touched to check for blocker stickers 
			// near by, so special moves don't do this check.
			if (removeTile.touched)// && matchTileGridModel.ObstacleIsBlocker(obstacle.type))
			{
				// Bottom
				Vector2 moveToPosition = new Vector2 (currentPos.x, currentPos.y - 1);
				obstacle = obstacleTilesModel.GetMatchObstacle (moveToPosition);
				RemoveObstacle (obstacle);

				// Top
				moveToPosition = new Vector2 (currentPos.x, currentPos.y + 1);
				obstacle = obstacleTilesModel.GetMatchObstacle (moveToPosition);
				RemoveObstacle (obstacle);

				// Left
				moveToPosition = new Vector2 (currentPos.x - 1, currentPos.y);
				obstacle = obstacleTilesModel.GetMatchObstacle (moveToPosition);
				RemoveObstacle (obstacle);

				// Right
				moveToPosition = new Vector2 (currentPos.x + 1, currentPos.y);
				obstacle = obstacleTilesModel.GetMatchObstacle (moveToPosition);
				RemoveObstacle (obstacle);
			}
		}

		private void RemoveObstacle(ObstacleTile obstacle, bool samePositionAsTile = false)
		{
			if (obstacle != null && !obstacleTilesModel.ObstacleAlreadyRemoved (obstacle))
			{
				if (samePositionAsTile)
				{
					Remove (obstacle);
				} 
				else if (!obstacleTilesModel.CanMove (obstacle) &&
				         obstacleTilesModel.ObstacleRemoveByAdjacentTile(obstacle.type))				
				{
					Remove (obstacle);
				}
			}
		}

		private void Remove(ObstacleTile obstacle)
		{
			matchTileFactory.RemoveObstacle (obstacle);
			obstacleTilesModel.RemovedObstacle (obstacle);
		}
	}
}