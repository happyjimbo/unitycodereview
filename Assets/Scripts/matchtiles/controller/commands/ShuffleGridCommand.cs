﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IoC;
using Command;
using DG.Tweening;
using EventDispatcher;

namespace MatchTileGrid
{
	public class ShuffleGridCommand : ICommand
	{
		[Inject]
		public IMatchTileGridModel matchTileGridModel { private get; set; }

		[Inject]
		public IEventDispatcher eventDispatcher { private get; set; }

		private List<MatchTile> matchTileList = new List<MatchTile>();
		private List<Vector2> matchTilePositions = new List<Vector2>();

		public void Execute()
		{
			matchTileList.Clear ();
			matchTilePositions.Clear ();

			RemoveTiles ();
			AddTilesToNewPositions ();

			eventDispatcher.Broadcast (MatchTileGridMessage.CHECK_MOVES_REMAINING);
		}

		private void RemoveTiles()
		{
			Dictionary<Vector2, MatchTile> matchTiles = matchTileGridModel.GetMatchTiles ();

			float yMax = matchTileGridModel.gridSize.y;
			float xMax = matchTileGridModel.gridSize.x;

			for (int y = 0; y < yMax; y++)
			{
				for (int x = 0; x < xMax; x++)
				{
					Vector2 pos = new Vector2 (x, y);
					if (matchTiles.ContainsKey (pos))
					{
						MatchTile matchTile = matchTiles [pos];
						if (matchTile.canMove)
						{
							matchTileList.Add (matchTile);
							matchTilePositions.Add (pos);

							matchTileGridModel.RemoveTile (pos);
						}	
					}
				}
			}	
		}

		private void AddTilesToNewPositions()
		{
			for (int i = 0; i < matchTileList.Count; i++)
			{
				int random = Random.Range (0, matchTilePositions.Count);
				Vector2 newPos = matchTilePositions [random];

				MatchTile tile = matchTileList [i];
				tile.position = newPos;
				tile.tileObject.transform.DOLocalMove (newPos, matchTileGridModel.moveSpeed).SetEase (Ease.Linear);
				matchTileGridModel.AddNewTile(tile);

				matchTilePositions.Remove (newPos);
			}
		}

	}
}