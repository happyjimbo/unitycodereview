using System;
using System.Linq;
using UnityEngine;
using IoC;
using ObjectPool;

namespace MatchTileGrid
{
	public class CreateSpecialTile
	{
		[Inject] public IMatchTileGridModel matchTileGridModel { private get; set; }
		[Inject] public IObjectPoolModel objectPoolModel { private get; set; }

		public void Create(Vector2 position, MatchTileSpecialType type)
		{
			var matchTile = matchTileGridModel.GetMatchTile(position);
			matchTile.specialTile.type = type;

			float scale = matchTileGridModel.matchTileOriginalScale;

			GameObject specialObject = objectPoolModel.GetObjectForType (type.ToString());
			specialObject.transform.localScale = new Vector3 (scale, scale, scale);
			specialObject.transform.parent = matchTile.tileObject.transform;
			specialObject.layer = LayerModel.GAME_LAYER_2D;
			specialObject.transform.localPosition = Vector3.zero;

			matchTile.specialTile.specialObject = specialObject;
		}
	}
}