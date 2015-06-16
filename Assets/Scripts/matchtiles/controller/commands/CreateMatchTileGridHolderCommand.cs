using System;
using UnityEngine;
using Command;
using IoC;

namespace MatchTileGrid
{
	public class CreateMatchTileGridHolderCommand : ICommand
	{
		[Inject]
		public IMatchTileGridModel matchTileGridModel { private get; set; }

		public void Execute()
		{
			MatchTilesData layout = matchTileGridModel.matchTilesData;
			CreateGameHolder(layout);

			Messenger.Broadcast (MatchTileGridMessage.GRID_HOLDER_CREATED);
		}

		private void CreateGameHolder(MatchTilesData layout)
		{
			GameObject gameHolder = new GameObject ();
			gameHolder.name = matchTileGridModel.gameHolderName;
			gameHolder.transform.parent = matchTileGridModel.gridHolder.transform;

			matchTileGridModel.gridSize = layout.gridSize;
			matchTileGridModel.gridParent = gameHolder;

			float xMax = matchTileGridModel.gridSize.x;
			float xpos = -((xMax - 1) / 2f);
			float ypos = matchTileGridModel.gridYPos;
			gameHolder.transform.localPosition = new Vector3 (xpos, ypos, 0);
		}
	}
}