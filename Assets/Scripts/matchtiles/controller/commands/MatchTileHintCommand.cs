using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IoC;
using Command;

namespace MatchTileGrid
{
	public class MatchTileHintCommand : ICommand
	{
		[Inject]
		public IMatchTileGridModel matchTileGridModel { private get; set; }

		public IEnumerator enumerator { get; private set; }

		public void Execute()
		{
			enumerator = CheckTouch ();
			Coroutiner.StartCoroutine (enumerator);
		}

		private IEnumerator CheckTouch()
		{
			yield return new WaitForSeconds (1);

			if ((Time.time - matchTileGridModel.lastTouchedTimestamp) >= matchTileGridModel.secondsUntilHint)
			{
				matchTileGridModel.lastTouchedTimestamp = Time.time;
				DisplayHint ();
			}

			Coroutiner.StartCoroutine (CheckTouch());	
		}

		private void DisplayHint ()
		{
			List<MatchTile> hintMatchTiles = matchTileGridModel.GetHintMatchTiles ();
			for (int i = 0; i < hintMatchTiles.Count; i++)
			{
				MatchTile tile = hintMatchTiles [i];

				IMatchTileComponent matchTileComponent = matchTileGridModel.GetMatchTileComponent (tile);
				matchTileComponent.Hint (i);
			}
		}
	}
}