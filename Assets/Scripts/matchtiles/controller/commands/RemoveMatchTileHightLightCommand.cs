using System;
using System.Collections.Generic;
using UnityEngine;
using Command;
using IoC;

namespace MatchTileGrid
{
	public class RemoveMatchTileHightLightCommand : ICommand
	{
		[Inject]
		public IMatchTileGridModel matchTileGridModel { private get; set; }

		public void Execute()
		{
			List<MatchTile> tilesTouched = matchTileGridModel.GetTilesTouched ();
			DisplayTiles (tilesTouched);
		}

		private void DisplayTiles(List<MatchTile> tilesTouched)
		{
			for (int i = 0; i < tilesTouched.Count; i++)
			{
				MatchTile tile = tilesTouched [i];

				if (tile.tileObject != null)
				{
					MatchTileComponent matchTileComponent = tile.tileObject.GetComponent<MatchTileComponent> ();
					if (matchTileComponent != null)
					{
						matchTileComponent.Tile ();
					}
				}
			}
		}
	}
}