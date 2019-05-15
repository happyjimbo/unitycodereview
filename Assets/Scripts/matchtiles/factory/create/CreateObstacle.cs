using System;
using UnityEngine;
using IoC;
using ObjectPool;

namespace MatchTileGrid
{
	public class CreateObstacle
	{
		[Inject] public IMatchTileGridModel matchTileGridModel { private get; set; }
		[Inject] public IObstacleTilesModel obstacleTilesModel { private get; set; }
		[Inject] public IObjectPoolModel objectPoolModel { private get; set; }

		public ObstacleTile Create(MatchTileObstacleType type, Vector2 position)
		{
			var obstacleTile = new ObstacleTile {type = type, position = position};
			obstacleTilesModel.AddObstacleTile (obstacleTile);

			if (type != MatchTileObstacleType.Null)
			{
				float scale = matchTileGridModel.matchTileOriginalScale;

				GameObject obstacleObject = objectPoolModel.GetObjectForType(type.ToString());
				obstacleObject.transform.localScale = new Vector3(scale, scale, scale);
				obstacleObject.transform.parent = matchTileGridModel.gridParent.transform;
				obstacleObject.layer = LayerModel.GAME_LAYER_2D;
				obstacleObject.transform.localPosition = new Vector3(position.x, position.y, 0);

				obstacleTile.obstacleObject = obstacleObject;

				// We change the types of certain obstacles, such as blockers with 3 stages,
				// so this is how we store it's original type, which is used with goals etc.
				obstacleTilesModel.AddObstacleOriginalType(obstacleTile);

				Trapper(type, obstacleTile);

				MatchTile matchTile = matchTileGridModel.GetMatchTile(obstacleTile.position);
				if (matchTile != null)
				{
					matchTile.canMove = matchTileGridModel.CanMove(matchTile) &&
					                    obstacleTilesModel.CanMove(obstacleTile);
				}
			}

			return obstacleTile;
		}

		private void Trapper(MatchTileObstacleType type, ObstacleTile obstacleTile)
		{
			if (type == MatchTileObstacleType.MatchToken_Trapper)
			{
				obstacleTile.counter = matchTileGridModel.GetTrapperAmount();
				TrapperComponent trapper = obstacleTile.obstacleObject.GetComponent<TrapperComponent> ();
				trapper.SetObstacleTile (obstacleTile);
			}
		}
	}
}