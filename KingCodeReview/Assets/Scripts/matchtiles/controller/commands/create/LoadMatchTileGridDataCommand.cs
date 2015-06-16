using System;
using IoC;
using Command;
using UnityEngine;

namespace MatchTileGrid
{
	public class LoadMatchTileGridDataCommand : ICommand
	{
		[Inject]
		public IMatchTileGridModel matchTileGridModel { private get; set; }

		public void Execute()
		{			
			MatchTilesData matchTilesData = Resources.Load(matchTileGridModel.matchTileLayout) as MatchTilesData;
			matchTileGridModel.matchTilesData = matchTilesData;
		}
	}
}