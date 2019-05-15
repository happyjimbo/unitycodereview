using System.Collections.Generic;
using UnityEngine;
using IoC;
using Command;
using EventDispatcher;

namespace MatchTileGrid
{
	public class SpecialSurroundingTileCommand : ICommand
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
			int tilesToRemove = 12;
			//int tilesForEachSide = (tilesToRemove - 1) / 2;

			RemoveTile removeTile = new RemoveTile ();
			removeTile.matchTiles = new List<MatchTile> ();
			removeTile.matchTiles.Add (matchTile);

			eventDispatcher.Broadcast (MatchTileGridMessage.REMOVE_TILES, removeTile);

			switch(tilesToRemove)
			{
				case 4:
					DestroyFirstFour ();	
					break;

				case 8:
					DestroyFirstFour ();	
					DestroySecondFour ();
					break;

				case 12:
					DestroyFirstFour ();	
					DestroyThirdFour ();
					break;
			}
		}

		private void DestroyFirstFour()
		{
			RemoveTile removeTile = new RemoveTile ();
			removeTile.matchTiles = new List<MatchTile> ();

			int xpos = (int)matchTile.position.x;
			int ypos = (int)matchTile.position.y;

			AddTile (removeTile, xpos, ypos + 1);
			AddTile (removeTile, xpos, ypos - 1);

			AddTile (removeTile, xpos + 1, ypos);
			AddTile (removeTile, xpos - 1, ypos);

			eventDispatcher.Broadcast (MatchTileGridMessage.REMOVE_TILES, removeTile);
		}

		private void DestroySecondFour()
		{
			RemoveTile removeTile = new RemoveTile ();
			removeTile.matchTiles = new List<MatchTile> ();

			int xpos = (int)matchTile.position.x;
			int ypos = (int)matchTile.position.y;

			AddTile (removeTile, xpos - 1, ypos + 1);
			AddTile (removeTile, xpos - 1, ypos - 1);

			AddTile (removeTile, xpos + 1, ypos + 1);
			AddTile (removeTile, xpos + 1, ypos - 1);

			eventDispatcher.Broadcast (MatchTileGridMessage.REMOVE_TILES, removeTile);
		}

		private void DestroyThirdFour()
		{
			RemoveTile removeTile = new RemoveTile ();
			removeTile.matchTiles = new List<MatchTile> ();

			int xpos = (int)matchTile.position.x;
			int ypos = (int)matchTile.position.y;

			AddTile (removeTile, xpos, ypos + 2);
			AddTile (removeTile, xpos, ypos - 2);

			AddTile (removeTile, xpos + 2, ypos);
			AddTile (removeTile, xpos - 2, ypos);

			eventDispatcher.Broadcast (MatchTileGridMessage.REMOVE_TILES, removeTile);
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