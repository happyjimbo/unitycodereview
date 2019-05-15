using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IoC;
using Command;
using EventDispatcher;

namespace MatchTileGrid
{
	public class SpecialVerticleTileCommand : ICommand
	{
		[Inject] public IMatchTileGridModel matchTileGridModel { private get; set; }
		[Inject] public IEventDispatcher eventDispatcher { private get; set; }

		public MatchTile matchTile { private get; set; }

		public void Execute ()
		{
			matchTileGridModel.pauseTilesFalling = true;

			DestroyMatchTiles ();
		}

		private void DestroyMatchTiles()
		{
			int tilesToRemove = 7;
			int tilesForEachSide = (tilesToRemove - 1) / 2;

			RemoveTile removeTile = new RemoveTile ();
			removeTile.matchTiles = new List<MatchTile> ();
			removeTile.matchTiles.Add (matchTile);

			eventDispatcher.Broadcast (MatchTileGridMessage.REMOVE_TILES, removeTile);

			DestroyTiles (tilesForEachSide);
		}

		private void DestroyTiles(int tilesForEachSide)
		{
			for (int i = 1; i <= tilesForEachSide; i++)
			{
				RemoveTile removeTile = new RemoveTile ();
				removeTile.matchTiles = new List<MatchTile> ();

				int xpos = (int)matchTile.position.x;
				int ypos = (int)matchTile.position.y;

				AddTile (removeTile, xpos, ypos - i);
				AddTile (removeTile, xpos, ypos + i);

				eventDispatcher.Broadcast (MatchTileGridMessage.REMOVE_TILES, removeTile);
			}
		}

		private void AddTile(RemoveTile removeTile, int x, int y)
		{
			MatchTile tile = matchTileGridModel.GetMatchTile (new Vector2 (x, y));
			if (tile != null)
			{
				removeTile.matchTiles.Add (tile);
			}
		}
	}
}
