using System;
using System.Collections.Generic;
using UnityEngine;

namespace MatchTileGrid
{
	public interface IMatchTileGridModel
	{
		string matchTileLayout { get; }
		string matchTileLocation { get; }
		string gameHolderName { get; }
		int minTouchRequired { get; }
		float matchTileOriginalScale { get; }
		float moveSpeed { get; }
		float gridYPos { get; }
		int secondsUntilHint { get; }

		bool pauseTilesFalling { get; set; }

		GameObject gridHolder { get; set; }
		GameObject gridParent { get; set; }
		Vector2 gridSize { get; set; }
		int movesRemaining { get; set; }
		int tilesToReplace { get; set; }

		bool allowTouch { get; set; }

		float score { get; set; }

		MatchTilesData matchTilesData { get; set; }

		void AddNewTile(MatchTile tile);

		void RemoveTile(Vector2 tileKey);

		MatchTile GetMatchTile(Vector2 pos);

		Dictionary<Vector2, MatchTile> GetMatchTiles();

		void MoveTile (Vector2 currentPosition, Vector2 newPosition);

		List<MatchTile> GetAllMatchTilesNotOfType (MatchTileType type);

		MatchTileType GetMatchTileType (string str);

		int ScoreValueOfMatchTile (MatchTileType type);

		float lastTouchedTimestamp { get; set; }

		MatchTileType selectedMatchTileType { get; }

		void AddTileTouched (Vector2 pos, MatchTile matchTile);

		List<MatchTile> GetTilesTouched ();

		void RemoveTileTouched (MatchTile tile);

		void ClearTilesTouched ();

		bool CanTouchTile (MatchTile matchTile);

		bool ValidMoveTouchEnded ();

		void AddHintMatchTile (MatchTile tile);

		List<MatchTile> GetHintMatchTiles ();

		void ClearHintMatchTiles ();

		bool CheckIfMoveAbove (Vector2 currentPos);

		bool CanMove (MatchTile tile);

		bool ValidMatchTile (MatchTileType type);
	}
}