using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IoC;
using Command;
using DG.Tweening;

namespace MatchTileGrid
{
	public class ShuffleGridCommand : ICommand
	{
		[Inject]
		public IMatchTileGridModel matchTileGridModel { private get; set; }

		public void Execute()
		{
			Shuffle ();
		}

		private void Shuffle()
		{
			List<MatchTile> matchTileList = new List<MatchTile>();
			List<Vector2> matchTilePositions = new List<Vector2>();

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

			Messenger.Broadcast (MatchTileGridMessage.CHECK_MOVES_REMAINING);
		}

	}
}