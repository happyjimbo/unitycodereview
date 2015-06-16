using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using IoC;
using Command;
using DG.Tweening;

namespace MatchTileGrid
{
	public class CalculateTilesToRemoveCommand : ICommand
	{
		[Inject]
		public IMatchTileGridModel matchTileGridModel { private get; set; }

		public void Execute()
		{
			if (matchTileGridModel.allowTouch)
			{
				Touched ();		
			}
		}

		private void Touched()
		{
			matchTileGridModel.pauseTilesFalling = true;

			List<MatchTile> tilesTouched = matchTileGridModel.GetTilesTouched ();

			if (matchTileGridModel.ValidMoveTouchEnded())
			{
				CalculateTilesToRemove (tilesTouched);	
			}
			else
			{
				matchTileGridModel.ClearTilesTouched ();
			}
		}

		private void CalculateTilesToRemove(List<MatchTile> tilesTouched)
		{
			for (int i = 0; i < tilesTouched.Count; i++)
			{
				MatchTile tile = tilesTouched [i];

				if (tile.tileObject != null)
				{
					int touchedCount = i + 1;

					RemoveTile removeTile = new RemoveTile ();
					removeTile.touched = true;
					removeTile.matchTiles = new List<MatchTile> ();
					removeTile.matchTiles.Add (tile);

					if (touchedCount == tilesTouched.Count)
					{
						removeTile.lastInChain = true;
						removeTile.tilesChained = touchedCount;
					}

					Messenger.Broadcast (MatchTileGridMessage.REMOVE_TILES, removeTile);
				}
			}
		}
	}
}