using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IoC;
using Command;
using DG.Tweening;

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
			float punch = 0.1f;
			Vector3 punchSize = new Vector3 (punch, punch, punch);

			List<MatchTile> hintMatchTiles = matchTileGridModel.GetHintMatchTiles ();
			for (int i = 0; i < hintMatchTiles.Count; i++)
			{
				MatchTile tile = hintMatchTiles [i];
				tile.tileObject.transform.DOPunchScale (punchSize, 1, 1).SetDelay (i);
			}
		}
	}
}