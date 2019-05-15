using System.Collections.Generic;
using UnityEngine;
using IoC;
using Command;
using EventDispatcher;

namespace MatchTileGrid
{
	public class SpecialMatchingTIleCommand : ICommand
	{
		[Inject] public IMatchTileGridModel matchTileGridModel { private get; set; }
		[Inject] public IEventDispatcher eventDispatcher { private get; set; }

		public MatchTile matchTile { private get; set; }

		public void Execute ()
		{
			RemoveTile removeTile = new RemoveTile ();
			removeTile.matchTiles = new List<MatchTile> ();

			List<MatchTile> tiles = matchTileGridModel.GetAllMatchTilesOfType (matchTile.type);
			removeTile.matchTiles.AddRange (tiles);

			eventDispatcher.Broadcast (MatchTileGridMessage.REMOVE_TILES, removeTile);
		}
	}
}