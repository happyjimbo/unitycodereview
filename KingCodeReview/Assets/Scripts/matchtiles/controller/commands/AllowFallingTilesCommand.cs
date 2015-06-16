using System.Collections;
using UnityEngine;
using IoC;
using Command;

namespace MatchTileGrid
{
	public class AllowFallingTilesCommand : ICommand
	{
		[Inject]
		public IMatchTileGridModel matchTileGridModel { private get; set; }

		public void Execute ()
		{
			Coroutiner.StartCoroutine (Allow ());
		}

		private IEnumerator Allow()
		{
			yield return new WaitForSeconds (0.25f);

			UpdateMoves ();

			Messenger.Broadcast (MatchTileGridMessage.CREATE_NEW_TILE);
		}

		private void UpdateMoves()
		{
			matchTileGridModel.pauseTilesFalling = false;
			matchTileGridModel.movesRemaining--;
		}
	}
}