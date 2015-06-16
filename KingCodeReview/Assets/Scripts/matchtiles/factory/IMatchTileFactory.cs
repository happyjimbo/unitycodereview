using System;
using UnityEngine;

namespace MatchTileGrid
{
	public interface IMatchTileFactory
	{
		MatchTile CreateMatchTile(MatchTileType type, Vector2 position);
		MatchTile CreateRandomMatchTile(Vector2 position);

		void RemoveMatchTile (MatchTile matchTile);
	}
}