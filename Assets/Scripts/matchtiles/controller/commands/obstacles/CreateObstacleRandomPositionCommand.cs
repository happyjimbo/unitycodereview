using UnityEngine;
using IoC;
using Command;

namespace MatchTileGrid
{
	public class CreateObstacleRandomPositionCommand : ICommand
	{
		[Inject] public IMatchTileGridModel matchTileGridModel { private get; set; }
		[Inject] public IObstacleTilesModel obstacleTilesModel { private get; set; }
		[Inject] public IMatchTileFactory matchTileFactory { private get; set; }

		public MatchTileObstacleType obstacleType { private get; set; }

		public void Execute()
		{
			Create();
		}

		private void Create()
		{
			var newTile = matchTileGridModel.GetRandomTileSpace ();
			var obstacle = obstacleTilesModel.GetMatchObstacle (newTile.position);

			if (obstacle.type == MatchTileObstacleType.Null)
			{
				obstacle.type = obstacleType;
				matchTileFactory.CreateObstacle (obstacleType, newTile.position);	
			}
			else
			{
				Create ();
			}
		}
	}
}