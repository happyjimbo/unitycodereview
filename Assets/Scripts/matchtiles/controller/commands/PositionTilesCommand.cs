using System.Collections;
using System.Collections.Generic;
using IoC;
using Command;
using UnityEngine;
using DG.Tweening;

namespace MatchTileGrid
{
	public class PositionTilesCommand : ICommand
	{
		[Inject]
		public IMatchTileGridModel matchTileGridModel { private get; set; }

		// The public accessable IEnumerator is unfortunatly needed for our 
		// unit tests so that they can manually walk through the entire 
		// IEnumerator method in order to test code that's executed after the yeild.
		public IEnumerator enumerator { get; private set; }

		public void Execute()
		{
			enumerator = Loop ();
			Coroutiner.StartCoroutine (enumerator);
		}

		private IEnumerator Loop()
		{
			yield return new WaitForSeconds (0.05f); 
			CheckIfEmptySpaceNearBy ();

			enumerator = Loop ();
			Coroutiner.StartCoroutine (enumerator);
		}

		private void CheckIfEmptySpaceNearBy()
		{
			if (!matchTileGridModel.pauseTilesFalling)
			{
				Dictionary<Vector2, MatchTile> matchTiles = matchTileGridModel.GetMatchTiles ();
				List<KeyValuePair<Vector2, MatchTile>> list = new List<KeyValuePair<Vector2, MatchTile>>(matchTiles);
				foreach(KeyValuePair<Vector2, MatchTile> entry in list)
				{
					GetTile (entry.Key);
				}	
			}
		}

		private void GetTile(Vector2 currentPos)
		{
			// Bottom
			Vector2 moveToPosition = new Vector2(currentPos.x, currentPos.y - 1);
			MatchTile tile = matchTileGridModel.GetMatchTile (moveToPosition);
			if (MoveTo (moveToPosition, currentPos, tile)) return;

			// Move Bottom and Left, check that there's not a moveable piece above first
			moveToPosition = new Vector2 (currentPos.x - 1, currentPos.y - 1);
			tile = matchTileGridModel.GetMatchTile (moveToPosition);
			if (tile == null)
			{
				if (!matchTileGridModel.CheckIfMoveAbove(moveToPosition))
				{
					if (MoveTo (moveToPosition, currentPos, tile)) return;	
				}
			}

			// Move Bottom and Right, check that there's not a moveable piece above first
			moveToPosition = new Vector2 (currentPos.x + 1, currentPos.y - 1);
			tile = matchTileGridModel.GetMatchTile (moveToPosition);
			if (tile == null)
			{
				if (!matchTileGridModel.CheckIfMoveAbove(moveToPosition))
				{
					if (MoveTo (moveToPosition, currentPos, tile)) return;	
				}
			}
		}

		private bool MoveTo(Vector2 moveToPosition, Vector2 currentPos, MatchTile newTile)
		{
			if (newTile == null && 
				moveToPosition.y >= 0 && 
				moveToPosition.y < matchTileGridModel.gridSize.y &&
				moveToPosition.x >= 0 &&
				moveToPosition.x < matchTileGridModel.gridSize.x)
			{
				MatchTile currentTile = matchTileGridModel.GetMatchTile (currentPos);
				if (currentTile.canMove)
				{
					currentTile.tileObject.transform.DOLocalMove (moveToPosition, matchTileGridModel.moveSpeed).SetEase (Ease.Linear);
					matchTileGridModel.MoveTile (currentPos, moveToPosition);
					return true;
				}
			}

			return false;
		}
	}
}