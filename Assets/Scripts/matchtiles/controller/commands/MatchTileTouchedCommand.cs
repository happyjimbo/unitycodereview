﻿using System;
using System.Collections.Generic;
using UnityEngine;
using IoC;
using Command;
using DG.Tweening;
using Touched;
using EventDispatcher;

namespace MatchTileGrid
{
	public class MatchTileTouchedCommand : ICommand
	{
		[Inject]
		public IMatchTileGridModel matchTileGridModel { private get; set; }

		[Inject]
		public IEventDispatcher eventDispatcher { private get; set; }

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
				eventDispatcher.Broadcast (MatchTileGridMessage.TILE_SELECTED, tile.type);

				matchTileGridModel.AddTileTouched (tilePos, tile);

				eventDispatcher.Broadcast (MatchTileGridMessage.HIDE_INVALID_TILES, tile.type);

				HighLight (tile);
				Punch (tile.tileObject);

				return;
			}

			PreviousTileSelected (tilePos);
		}

		private void HighLight(MatchTile tile)
		{
			IMatchTileComponent matchTileComponent = matchTileGridModel.GetMatchTileComponent(tile);
			if (matchTileComponent != null)
			{
				matchTileComponent.HighLight ();
			}
		}

		private void Punch(GameObject tileObject)
		{
			float punch = 0.1f;
			Vector3 punchSize = new Vector3 (punch, punch, punch);
			tileObject.transform.DOPunchScale (punchSize, 0.5f, 3);
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