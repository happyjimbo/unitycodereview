using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using IoC;

namespace MatchTileGrid
{
	public class CreateRandomMatchTile
	{
		[Inject]
		public IMatchTileGridModel matchTileGridModel { private get; set; }

		[Inject]
		public IMatchTileFactory matchTileFactory { private get; set; }

		public MatchTile Create(Vector2 position, MatchTileObstacleType obstacleType)
		{
			MatchTileType[] matchTilesAvailable = (MatchTileType[]) Enum.GetValues(typeof(MatchTileType));

			int ran = UnityEngine.Random.Range (0, matchTilesAvailable.Length);
			MatchTileType type = matchTilesAvailable [ran];

			while(!matchTileGridModel.ValidMatchTile(type))
			{
				ran = UnityEngine.Random.Range (0, matchTilesAvailable.Length);
				type = matchTilesAvailable [ran];
			}

			return matchTileFactory.CreateMatchTile (type, obstacleType, position);
		}
	}
}