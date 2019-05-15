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
		[Inject] public IMatchTileGridModel matchTileGridModel { private get; set; }
		[Inject] public ISpecialTilesModel specialTilesModel{ private get; set; }
		[Inject] public IObstacleTilesModel obstacleTilesModel { private get; set; }
		[Inject] public IMatchTileFactory matchTileFactory { private get; set; }
		[Inject] public IEventDispatcher eventDispatcher { private get; set; }

		public RemoveTile removeTile { private get; set; }

		// The public accessable IEnumerator's are unfortunatly needed for our 
		// unit tests so that they can manually walk through the entire 
		// IEnumerator methods in order to test code that's executed after the yield.
		public IEnumerator enumerator { get; private set; }
		public IEnumerator loopEnumerator { get; private set; }
		public IEnumerator endEnumerator { get; private set; }

		private List<RemoveTile> matchTileQueue = new List<RemoveTile>();
		private List<MatchTileSpecialType> specialTiles = new List<MatchTileSpecialType> ();
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

				eventDispatcher.Broadcast (MatchTileGridMessage.REMOVE_OBSTACLE, removeTile);
				
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
			ObstacleTile obstacle = obstacleTilesModel.GetMatchObstacle (matchTile.position);

			SpecialTiles (matchTile, specialTiles);

			if (removeTile.lastInChain)
			{
				eventDispatcher.Broadcast (MatchTileGridMessage.UPDATE_TRAPPER);
			}

			// This will active already existing special tiles, which is seperate to creating them below.
			if (removeTile.lastInChain && specialTilesModel.createSpecialTile)
			{
				// nasty hack, fix this!!!
				//Debug.Log ("Create Special Tile");
			}
			else if (specialTiles.Count > 0 && removeTile.lastInChain)
			{
				eventDispatcher.Broadcast (MatchTileGridMessage.SPECIAL_TILE, specialTiles, matchTile);
				specialTiles.Clear ();
			}
			else if (matchTileGridModel.CanMove(matchTile) && 
			         obstacleTilesModel.CanMove(obstacle) &&
			         obstacle.type != MatchTileObstacleType.MatchToken_Trapper)
			{
				tilesToReplace++;
				matchTileFactory.RemoveMatchTile (matchTile);	
			}
		}
		
		private void SpecialTiles(MatchTile tile, List<MatchTileSpecialType> specialTiles)
		{
			if (tile.specialTile.type != MatchTileSpecialType.Null)
			{
				specialTiles.Add (tile.specialTile.type);
			}
		}

		private IEnumerator End()
		{
			eventDispatcher.Broadcast (MatchTileGridMessage.ALLOW_FALLING_TILES);

			yield return new WaitForSeconds (0.15f);

			specialTiles.Clear ();

			specialTilesModel.createSpecialTile = false;
			obstacleTilesModel.regeneratorObstacleRemoved = false;
			matchTileGridModel.tilesToReplace = tilesToReplace;
			obstacleTilesModel.ClearRemovedObstacles ();
			matchTileGridModel.ClearTilesTouched ();

			tilesToReplace = 0;
		}

	}
}