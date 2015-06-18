using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IoC;
using Command;
using DG.Tweening;
using EventDispatcher;

namespace MatchTileGrid
{
	public class RemoveMatchTilesCommand : ICommand
	{
		[Inject]
		public IMatchTileGridModel matchTileGridModel { private get; set; }

		[Inject]
		public IMatchTileFactory matchTileFactory { private get; set; }

		[Inject]
		public IEventDispatcher eventDispatcher { private get; set; }

		public RemoveTile removeTile { private get; set; }

		// These are unfortunatly needed for our unit tests so that they can
		// manually walk through the entire IEnumerator methods in order to test.
		public IEnumerator enumerator { get; private set; }
		public IEnumerator loopEnumerator { get; private set; }
		public IEnumerator endEnumerator { get; private set; }

		private List<RemoveTile> matchTileQueue = new List<RemoveTile>();
		private int tilesToReplace = 0;

		public void Execute()
		{
			matchTileQueue.Add (removeTile);

			if (matchTileQueue.Count == 1)
			{
				matchTileGridModel.pauseTilesFalling = true;

				enumerator = Remove ();
				Coroutiner.StartCoroutine (enumerator);
			}
		}

		private IEnumerator Remove()
		{
			yield return new WaitForSeconds (0.05f);

			if (matchTileQueue.Count > 0)
			{
				RemoveTile removeTile = matchTileQueue [0];
				List<MatchTile> matchTiles = removeTile.matchTiles;

				for (int i = 0; i < matchTiles.Count; i++)
				{					
					Remove (matchTiles[i], removeTile);
				}

				matchTileQueue.RemoveAt (0);

				loopEnumerator = Remove ();
				Coroutiner.StartCoroutine (loopEnumerator);
			}
			else
			{
				endEnumerator = End ();
				Coroutiner.StartCoroutine (endEnumerator);
			}
		}

		private void Remove(MatchTile matchTile, RemoveTile removeTile)
		{	
			if (matchTileGridModel.CanMove(matchTile) )
			{
				tilesToReplace++;
				matchTileFactory.RemoveMatchTile (matchTile);	
			}
		}

		private IEnumerator End()
		{
			eventDispatcher.Broadcast (MatchTileGridMessage.ALLOW_FALLING_TILES);

			yield return new WaitForSeconds (0.15f);

			matchTileGridModel.tilesToReplace = tilesToReplace;
			matchTileGridModel.ClearTilesTouched ();

			tilesToReplace = 0;
		}

	}
}