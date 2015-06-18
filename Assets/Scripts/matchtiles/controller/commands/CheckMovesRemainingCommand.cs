using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IoC;
using Command;
using EventDispatcher;

namespace MatchTileGrid
{
	public class CheckMovesRemainingCommand : ICommand
	{
		[Inject]
		public IMatchTileGridModel matchTileGridModel { private get; set; }

		[Inject]
		public IEventDispatcher eventDispatcher { private get; set; }

		public void Execute()
		{
			CheckMovesRemaining ();
		}

		private void CheckMovesRemaining()
		{
			// If there's no match-3 then we want to dispatch the shuffle command.
			bool matches = CheckMatches ();
			if (!matches)
			{
				Debug.Log ("No matches, shuffle grid!");
				eventDispatcher.Broadcast (MatchTileGridMessage.SHUFFLE_GRID);
			}
		}

		private bool CheckMatches()
		{
			Dictionary<Vector2, MatchTile> matchTiles = matchTileGridModel.GetMatchTiles ();

			float yMax = matchTileGridModel.gridSize.y;
			float xMax = matchTileGridModel.gridSize.x;

			for (int y = 0; y < yMax; y++)
			{
				for (int x = 0; x < xMax; x++)
				{
					Vector2 pos = new Vector2 (x, y);
					if (matchTiles.ContainsKey(pos))
					{						
						MatchTile matchTile = matchTiles [pos];
						if (matchTile.canMove)
						{
							// Cleaar any hint tiles saved from previous loop.
							matchTileGridModel.ClearHintMatchTiles ();

							if (ValidMatchTileSequence(matchTile, pos))
							{						
								return true;
							}
						}	
					}
				}
			}

			return false;
		}

		// Constant check for the match count for performance reasons,
		// whilst it would be 'cleaner code' to just check once at the bottom
		// it's better to not run each check if it's not needed.
		private bool ValidMatchTileSequence(MatchTile currentTile, Vector2 currentPos)
		{
			int matchCount = 0;

			// Top
			Vector2 top = new Vector2(currentPos.x, currentPos.y + 1);
			matchCount = Match (currentTile, top, matchCount);

			// Bottom
			Vector2 bottom = new Vector2(currentPos.x, currentPos.y - 1);
			matchCount = Match (currentTile, bottom, matchCount);
			if (matchCount >= 2) return true;

			// Left
			Vector2 left = new Vector2 (currentPos.x - 1, currentPos.y);
			matchCount = Match (currentTile, left, matchCount);
			if (matchCount >= 2) return true;

			// Right
			Vector2 right = new Vector2 (currentPos.x + 1, currentPos.y);
			matchCount = Match (currentTile, right, matchCount);
			if (matchCount >= 2) return true;

			// TopRight
			Vector2 topRight = new Vector2 (currentPos.x + 1, currentPos.y + 1);
			matchCount = Match (currentTile, topRight, matchCount);
			if (matchCount >= 2) return true;

			// TopLeft
			Vector2 topLeft = new Vector2 (currentPos.x - 1, currentPos.y + 1);
			matchCount = Match (currentTile, topLeft, matchCount);
			if (matchCount >= 2) return true;

			// BottomRight
			Vector2 bottomRight = new Vector2 (currentPos.x + 1, currentPos.y - 1);
			matchCount = Match (currentTile, bottomRight, matchCount);
			if (matchCount >= 2) return true;

			// BottomLeft
			Vector2 bottomLeft = new Vector2 (currentPos.x - 1, currentPos.y - 1);
			matchCount = Match (currentTile, bottomLeft, matchCount);
			if (matchCount >= 2) return true;	

			return false;
		}

		private int Match(MatchTile currentTile, Vector2 pos, int matchCount)
		{
			MatchTile tile = matchTileGridModel.GetMatchTile (pos);

			if (tile != null && 
				currentTile.type.Equals(tile.type) &&
				tile.canMove)
			{
				matchCount++;
				matchTileGridModel.AddHintMatchTile (tile);

				// We want the current title to be second in the list so that
				// when we loop through the tiles to highlight it's in a more
				// natural order for the user, as the current title tends to be
				// in the middle.
				if (!matchTileGridModel.GetHintMatchTiles().Contains(currentTile))
				{
					matchTileGridModel.AddHintMatchTile (currentTile);	
				}

			}

			return matchCount;
		}
	}
}