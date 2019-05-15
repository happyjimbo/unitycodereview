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
		public ISpecialTilesModel specialTilesModel { private get; set; }
		
		[Inject]
		public IObstacleTilesModel obstacleTilesModel { private get; set; }

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
//					MatchTile tile = new MatchTile ();
//					tile.position = position;

					ObstacleTile obstacleTile = new ObstacleTile ();
					obstacleTile.position = position;

					if (layout.matchTiles.ContainsKey(position))
					{
						MatchTileData data = layout.matchTiles [position];
						MatchTileType type = matchTileGridModel.GetMatchTileType (data.gameObject.name);
						MatchTileObstacleType obstacle = obstacleTilesModel.GetMatchTileObstacleType (data.gameObject.name);
						MatchTileSpecialType special = specialTilesModel.GetMatchTileSpecialType (data.gameObject.name);
						
//						tile.type = type;
//						obstacleTile.type = obstacle;

						if (type != MatchTileType.Null || obstacle != MatchTileObstacleType.Null)
						{
							matchTileFactory.CreateMatchTile (type, obstacle, position);	
						}
						else
						{
							matchTileFactory.CreateRandomMatchTile (position, MatchTileObstacleType.Null);
							
							if (special != MatchTileSpecialType.Null)
							{
								matchTileFactory.CreateSpecialTile (position, special);
							}
						}
					}
					else
					{
						matchTileFactory.CreateRandomMatchTile (position, MatchTileObstacleType.Null);
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