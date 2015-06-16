using System;
using System.Collections.Generic;
using UnityEngine;
using IoC;
using Command;
using DG.Tweening;
using Touched;

namespace MatchTileGrid
{
	public class MatchTileTouchedCommand : ICommand
	{
		[Inject]
		public IMatchTileGridModel matchTileGridModel { private get; set; }

		public TouchedObject touchedObject { private get; set; }

		public void Execute()
		{
			if (matchTileGridModel.allowTouch)
			{
				TileSelected ();
				matchTileGridModel.lastTouchedTimestamp = Time.time;	
			}
		}

		private void TileSelected()
		{
			Vector3 pos = touchedObject.objectHit.transform.localPosition;
			Vector2 tilePos = new Vector2 (pos.x, pos.y);

			MatchTile tile = matchTileGridModel.GetMatchTile (tilePos);

			if (matchTileGridModel.CanTouchTile(tile))
			{
				Messenger.Broadcast (MatchTileGridMessage.TILE_SELECTED, tile.type);

				matchTileGridModel.AddTileTouched (tilePos, tile);

				CalculateScore (tile.type, true);

				Messenger.Broadcast (MatchTileGridMessage.HIDE_INVALID_TILES, tile.type);

				MatchTileComponent matchTileComponent = tile.tileObject.GetComponent<MatchTileComponent> ();
				if (matchTileComponent != null)
				{
					matchTileComponent.HighLight ();
				}

				float punch = 0.1f;
				Vector3 punchSize = new Vector3 (punch, punch, punch);
				tile.tileObject.transform.DOPunchScale (punchSize, 0.5f, 3);

				return;
			}

			PreviousTileSelected (tilePos);
		}

		private void CalculateScore(MatchTileType type, bool addScore)
		{
			if (addScore)
			{
				matchTileGridModel.score += matchTileGridModel.ScoreValueOfMatchTile (type);
			}
			else
			{
				matchTileGridModel.score -= matchTileGridModel.ScoreValueOfMatchTile (type);	
			}

			Messenger.Broadcast (MatchTileGridMessage.MATCH_SCORE_ADDED);
		}

		private void PreviousTileSelected(Vector2 tilePos)
		{
			List<MatchTile> matchTiles = matchTileGridModel.GetTilesTouched ();
			if (matchTiles.Count > 1)
			{
				MatchTile previousMatchTileAdded = matchTiles [matchTiles.Count - 2];

				if (tilePos == previousMatchTileAdded.position)
				{
					MatchTile lastTileAdded = matchTiles [matchTiles.Count - 1];
					matchTileGridModel.RemoveTileTouched (lastTileAdded);

					CalculateScore (lastTileAdded.type, false);

					MatchTile previousTileTouched = matchTileGridModel.GetMatchTile (lastTileAdded.position);
					MatchTileComponent matchTileComponent = previousTileTouched.tileObject.GetComponent<MatchTileComponent> ();
					if (matchTileComponent != null)
					{
						matchTileComponent.Tile ();
					}
				}
			}
		}

	}
}