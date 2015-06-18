using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IoC;
using Command;
using EventDispatcher;

namespace MatchTileGrid
{
	public class CreateMatchTileGridCommand : ICommand
	{
		[Inject]
		public IMatchTileGridModel matchTileGridModel { private get; set; }

		[Inject]
		public IMatchTileFactory matchTileFactory { private get; set; }

		[Inject]
		public IEventDispatcher eventDispatcher { private get; set; }

		public void Execute()
		{
			CreateGrid ();
		}

		private void CreateGrid()
		{	
			MatchTilesData layout = matchTileGridModel.matchTilesData;

			float yMax = matchTileGridModel.gridSize.y;
			float xMax = matchTileGridModel.gridSize.x;

			for (int y = 0; y < yMax; y++)
			{
				for (int x = 0; x < xMax; x++)
				{
					Vector2 position = new Vector2 (x, y);

					if (layout.matchTiles.ContainsKey(position))
					{
						MatchTileData data = layout.matchTiles [position];
						MatchTileType type = matchTileGridModel.GetMatchTileType (data.gameObject.name);

						if (type != MatchTileType.Null)
						{
							matchTileFactory.CreateMatchTile (type, position);	
						}
						else
						{
							matchTileFactory.CreateRandomMatchTile (position);
						}
					}
					else
					{
						matchTileFactory.CreateRandomMatchTile (position);
					}
				}
			}

			GridCreationComplete ();
		}

		private void GridCreationComplete()
		{
			eventDispatcher.Broadcast (MatchTileGridMessage.CHECK_MOVES_REMAINING);
		}
	}
}