using System;
using UnityEngine;
using IoC;
using ObjectPool;

namespace MatchTileGrid
{
	public class CreateMatchTile
	{
		[Inject] public IMatchTileGridModel matchTileGridModel { private get; set; }
		[Inject] public IObstacleTilesModel obstacleTilesModel { private get; set; }
		[Inject] public IObjectPoolModel objectPoolModel { private get; set; }

		public MatchTile Create(MatchTileType type, Vector2 position)
		{
			var matchTile = new MatchTile {position = position, type = type};

			var canMove = matchTileGridModel.CanMove(matchTile) && obstacleTilesModel.CanMove(position);

			matchTile.canMove = canMove;
			matchTile.canTouch = matchTileGridModel.ValidMatchTile (matchTile.type);
			matchTile.tileObject = CreateTileGameObject(type, position);
			
			matchTileGridModel.AddNewTile (matchTile);

			return matchTile;
		}

		private GameObject CreateTileGameObject(MatchTileType type, Vector2 position)
		{
			GameObject tile = objectPoolModel.GetObjectForType (type.ToString());

			float scale = matchTileGridModel.matchTileOriginalScale;

			tile.transform.localScale = new Vector3 (scale, scale, scale);
			tile.transform.parent = matchTileGridModel.gridParent.transform;
			tile.transform.localPosition = new Vector3 (position.x, position.y, 0);

			return tile;
		}
	}
}