using System.Collections;
using System.Collections.Generic;
using IoC;
using Command;
using UnityEngine;

namespace MatchTileGrid
{
	public class CalculateSpecialTileChargeCommand : ICommand
	{
		[Inject] public IMatchTileFactory matchTileFactory { private get; set; }
		[Inject] public IMatchTileGridModel matchTileGridModel { private get; set; }
		[Inject] public ISpecialTilesModel specialTilesModel { private get; set; }

		public void Execute ()
		{
			if (matchTileGridModel.ValidMoveTouchEnded())
			{
				Coroutiner.StartCoroutine (Calculate ());
			}
		}

		private IEnumerator Calculate()
		{
			yield return new WaitForEndOfFrame ();

			List<MatchTile> tiles = matchTileGridModel.GetTilesTouched ();
			bool removeTile = tiles.Count >= matchTileGridModel.minTouchRequired ? true : false;
			if (removeTile)
			{
				MatchTileType type = tiles [0].type;

				IncreaseSpecialTileCharge (type, tiles.Count);
				CreateSpecialTile (type, tiles);
			}
		}

		private void IncreaseSpecialTileCharge(MatchTileType type, int amount)
		{
			specialTilesModel.IncreaseSpecialTileCharge (type, amount);
		}

		private void CreateSpecialTile(MatchTileType type, List<MatchTile> tiles)
		{
			SpecialTileCharge specialTileCharge = specialTilesModel.GetSpecialTileCharge (type);

			int remainingToCollect = specialTileCharge.chargeNeeded - specialTileCharge.currentCharge;
			if (remainingToCollect <= 0)
			{
				// Drop a special tile.
				specialTileCharge.currentCharge = 0;

				MatchTile matchTile = tiles [tiles.Count - 1];
				matchTileFactory.CreateSpecialTile (matchTile.position, specialTileCharge.specialType);

				specialTilesModel.createSpecialTile = true;
			}
		}
	}
}