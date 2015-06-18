using System.Collections;
using UnityEngine;
using IoC;
using Command;
using EventDispatcher;

namespace MatchTileGrid
{
	public class AllowFallingTilesCommand : ICommand
	{
		[Inject]
		public IMatchTileGridModel matchTileGridModel { private get; set; }

		[Inject]
		public IEventDispatcher eventDispatcher { private get; set; }

		public IEnumerator enumerator { get; private set; }

		public void Execute ()
		{
			enumerator = Allow ();
			Coroutiner.StartCoroutine (enumerator);
		}

		private IEnumerator Allow()
		{
			yield return new WaitForSeconds (0.25f);

			UpdateMoves ();

			eventDispatcher.Broadcast (MatchTileGridMessage.CREATE_NEW_TILE);
		}

		private void UpdateMoves()
		{
			matchTileGridModel.pauseTilesFalling = false;
			matchTileGridModel.movesRemaining--;
		}
	}
}