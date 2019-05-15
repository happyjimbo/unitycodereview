using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IoC;
using Command;
using DG.Tweening;

namespace MatchTileGrid
{
	public class UpdateTrapperTileCommand : ICommand
	{
		[Inject] public IMatchTileGridModel matchTileGridModel { private get; set; }
		[Inject] public IObstacleTilesModel obstacleTilesModel { private get; set; }
		[Inject] public IMatchTileFactory matchTileFactory { private get; set; }

		public void Execute ()
		{
			UpdateCounter ();
		}

		private void UpdateCounter()
		{
			List<MatchTile> touchedTiles = matchTileGridModel.GetTilesTouched ();
			int touched = touchedTiles.Count;

			for (int i = 0; i < touched; i++)
			{
				MatchTile tile = touchedTiles [i];
				ObstacleTile obstacle = obstacleTilesModel.GetMatchObstacle (tile.position);

				if (obstacle.type == MatchTileObstacleType.MatchToken_Trapper)
				{
					obstacle.counter -= touched;

					TrapperComponent trapper = obstacle.obstacleObject.GetComponent<TrapperComponent> ();
					trapper.UpdateCounter ();

					if (obstacle.counter <= 0)
					{
						matchTileFactory.RemoveMatchTile (tile);
						matchTileFactory.RemoveObstacle (obstacle);
					}
				}
			}
		}

		public void SwapTiles()
		{
			List<MatchTile> matchTiles = matchTileGridModel.GetAllMatchTilesNotOfType (MatchTileType.Blank);

			for (int i = 0; i < matchTiles.Count; i++)
			{
				MatchTile tile = matchTiles [i];
				ObstacleTile obstacle = obstacleTilesModel.GetMatchObstacle (tile.position);

				if (obstacle.type == MatchTileObstacleType.MatchToken_Trapper)
				{
					List<Vector2> positions = new List<Vector2> () 
					{
						new Vector2 (tile.position.x - 1, tile.position.y), // left
						new Vector2 (tile.position.x + 1, tile.position.y), // right
						new Vector2 (tile.position.x, tile.position.y + 1), // up
						new Vector2 (tile.position.x, tile.position.y - 1) // down
					};

					int posLength = positions.Count;
					for (int j = 0; j < posLength; j++)
					{
						if (positions.Count > 0)
						{
							int ran = Random.Range (0, positions.Count);
							Vector2 moveToPosition = positions [ran];

							MatchTile newTile = matchTileGridModel.GetMatchTile (moveToPosition);
							if (MoveTo (moveToPosition, tile.position, newTile, tile))
								break;

							positions.Remove (moveToPosition);
							j--;
						}
					}
				}
			}
		}

		private bool MoveTo(Vector2 moveToPosition, Vector2 currentPos, MatchTile newTile, MatchTile currentTile)
		{
			if (newTile != null &&
				newTile.canMove &&
				moveToPosition.y >= 0 && 
				moveToPosition.y < matchTileGridModel.gridSize.y &&
				moveToPosition.x >= 0 &&
				moveToPosition.x < matchTileGridModel.gridSize.x)
			{
				currentTile.tileObject.transform.DOLocalMove (moveToPosition, matchTileGridModel.moveSpeed).SetEase (Ease.Linear);
				newTile.tileObject.transform.DOLocalMove (currentPos, matchTileGridModel.moveSpeed).SetEase (Ease.Linear);

				ObstacleTile currentObstacle = obstacleTilesModel.GetMatchObstacle (currentPos);
				currentObstacle.obstacleObject.transform.DOLocalMove (moveToPosition, matchTileGridModel.moveSpeed).SetEase (Ease.Linear);

				ObstacleTile newObstacle = obstacleTilesModel.GetMatchObstacle (moveToPosition);
				//newObstacle.obstacleObject.transform.DOLocalMove (currentPos, matchTileGridModel.moveSpeed).SetEase (Ease.Linear);

				matchTileGridModel.SwapTiles (currentPos, moveToPosition);
				obstacleTilesModel.SwapObstacles (currentPos, moveToPosition);
				
				currentTile.canMove = matchTileGridModel.CanMove(currentTile) && obstacleTilesModel.CanMove(currentObstacle);	
				newTile.canMove = matchTileGridModel.CanMove(newTile) && obstacleTilesModel.CanMove(newObstacle);;	

				return true;
			}

			return false;
		}
	}
}
