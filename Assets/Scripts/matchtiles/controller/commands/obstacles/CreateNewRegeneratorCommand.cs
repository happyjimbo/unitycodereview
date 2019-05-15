using System.Collections.Generic;
using IoC;
using Command;
using UnityEngine;
using DG.Tweening;

namespace MatchTileGrid
{
	public class CreateNewRegeneratorCommand : ICommand
	{
		[Inject] public IMatchTileGridModel matchTileGridModel { private get; set; }
		[Inject] public IObstacleTilesModel obstacleTilesModel { private get; set; }

		[Inject] public IMatchTileFactory matchTileFactory { private get; set; }

		public void Execute ()
		{
			if (!obstacleTilesModel.regeneratorObstacleRemoved)
			{
				var tiles = obstacleTilesModel.GetAllObstaclesOfType (MatchTileObstacleType.MatchToken_Regenerator);	
				if (tiles.Count > 0)
				{
					FindValidMatchTileToConvert (tiles);	
				}
			}
		}

		private void FindValidMatchTileToConvert(List<ObstacleTile> tiles) 
		{
			int ran = Random.Range (0, tiles.Count);

			ObstacleTile tile = tiles [ran];

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
					int ranPos = Random.Range (0, positions.Count);
					Vector2 moveToPosition = positions [ranPos];

					MatchTile newTile = matchTileGridModel.GetMatchTile (moveToPosition);
					if (CreateRegenerator (moveToPosition, tile.position, newTile))
						return;

					positions.Remove (moveToPosition);
					j--;
				}
			}

			FindValidMatchTileToConvert (tiles);
		}

		private bool CreateRegenerator(Vector2 moveToPosition, Vector2 currentPos, MatchTile newTile)
		{
			if (newTile != null &&
				newTile.canMove &&
				moveToPosition.y >= 0 && 
				moveToPosition.y < matchTileGridModel.gridSize.y &&
				moveToPosition.x >= 0 &&
				moveToPosition.x < matchTileGridModel.gridSize.x)
			{
				var obstacle = obstacleTilesModel.GetMatchObstacle (moveToPosition);

				obstacle.type = MatchTileObstacleType.MatchToken_Regenerator;
				matchTileFactory.CreateObstacle (obstacle.type, moveToPosition);

				obstacle.obstacleObject.transform.localPosition = currentPos;
				obstacle.obstacleObject.transform.DOLocalMove (moveToPosition, matchTileGridModel.moveSpeed).SetEase (Ease.Linear);
				return true;
			}

			return false;
		}
	}
}

