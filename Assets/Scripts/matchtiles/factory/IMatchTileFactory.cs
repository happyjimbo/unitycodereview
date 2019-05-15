using System;
using UnityEngine;

namespace MatchTileGrid
{
	public interface IMatchTileFactory
	{
		MatchTile CreateMatchTile(MatchTileType matchTile, MatchTileObstacleType obstacleTile, Vector2 position);
		MatchTile CreateRandomMatchTile(Vector2 position, MatchTileObstacleType obstacleType);

		ObstacleTile CreateObstacle (MatchTileObstacleType obstacleTile, Vector2 position);

		void CreateSpecialTile(Vector2 position, MatchTileSpecialType specialType);

		void RemoveMatchTile (MatchTile matchTile);
		void RemoveObstacle (ObstacleTile obstacleTile);
		
//		MatchTile CreateMatchTile(MatchTileType type, Vector2 position);
//		MatchTile CreateRandomMatchTile(Vector2 position);
//		void RemoveMatchTile (MatchTile matchTile);
	}
}