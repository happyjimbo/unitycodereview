using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IoC;
using Command;
using DG.Tweening;

namespace MatchTileGrid
{
	public class CreateNewTileCommand : ICommand
	{
		[Inject]
		public IMatchTileGridModel matchTileGridModel { private get; set; }

		[Inject]
		public IMatchTileFactory matchTileFactory { private get; set; }

		public void Execute()
		{
			int tilesTouched = matchTileGridModel.tilesToReplace;

			bool previousAllowTouch = matchTileGridModel.allowTouch;
			matchTileGridModel.allowTouch = false;

			Coroutiner.StartCoroutine (CreateNewTile (tilesTouched, previousAllowTouch));
		} 

		private IEnumerator CreateNewTile(int pos, bool previousAllowTouch)
		{
			yield return new WaitForSeconds (0.21f);

			bool createMore = false;

			for (int x = 0; x < matchTileGridModel.gridSize.x; x++)
			{
				float y = matchTileGridModel.gridSize.y - 1f;

				Vector2 tilePos = new Vector2 (x, y);
				MatchTile matchTile = matchTileGridModel.GetMatchTile (tilePos);

				if (matchTile == null)
				{
					createMore = true;

					matchTileFactory.CreateRandomMatchTile (tilePos);
				}
			}

			if (createMore)
			{				
				Coroutiner.StartCoroutine (CreateNewTile (pos, previousAllowTouch));				
			}
			else
			{
				AllowTouch (previousAllowTouch);
			}
		}

		private void AllowTouch(bool previousAllowTouch)
		{
			if (previousAllowTouch)
			{
				matchTileGridModel.allowTouch = true;	
			}

			Messenger.Broadcast (MatchTileGridMessage.CHECK_MOVES_REMAINING);
		}
	}
}