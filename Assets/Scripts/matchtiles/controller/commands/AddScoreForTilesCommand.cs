using System;
using System.Collections.Generic;
using IoC;
using Command;

namespace MatchTileGrid
{
	public class AddScoreForTilesCommand : ICommand
	{
		[Inject]
		public IMatchTileGridModel matchTileGridModel { private get; set; }

		public void Execute ()
		{
			if (matchTileGridModel.ValidMoveTouchEnded())
			{
				List<MatchTile> tiles = matchTileGridModel.GetTilesTouched ();

				for (int i = 0; i < tiles.Count; i++)
				{
					MatchTileType type = tiles [i].type;
					matchTileGridModel.AddMatchTileCollected (type);
				}	
			}

			matchTileGridModel.score = 0;
		}
	}
}