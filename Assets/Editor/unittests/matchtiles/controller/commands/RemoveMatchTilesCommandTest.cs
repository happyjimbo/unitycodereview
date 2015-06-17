using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using NSubstitute;
using UnityEngine;
using Touched;

// Using the Given When Then approach:
// http://martinfowler.com/bliki/GivenWhenThen.html
namespace MatchTileGrid
{
	[TestFixture]
	internal class RemoveMatchTilesCommandTest
	{
		private RemoveMatchTilesCommand removeMatchTilesCommand;
		private IMatchTileGridModel matchTileGridModel;
		private IMatchTileFactory matchTileFactory;

		[SetUp]
		public void SetUp()
		{
			matchTileGridModel = Substitute.For<IMatchTileGridModel>();
			matchTileFactory = Substitute.For<IMatchTileFactory>();

			removeMatchTilesCommand = new RemoveMatchTilesCommand ();
			removeMatchTilesCommand.matchTileGridModel = matchTileGridModel;
			removeMatchTilesCommand.matchTileFactory = matchTileFactory;

			Messenger.CleanAndDestroy();
		}

		[Test]
		public void GiveMatchTilesAddedToRemoveTile_WhenExecute_ThenRemoveMatchTile()
		{
			MatchTile matchTile = new MatchTile ();
			matchTile.type = MatchTileType.MatchToken_A;
			matchTile.canMove = true;

			List<MatchTile> matchTiles = new List<MatchTile> ();
			matchTiles.Add (matchTile);

			RemoveTile removeTile = new RemoveTile ();
			removeTile.matchTiles = matchTiles;

			matchTileGridModel.CanMove (matchTile).Returns (true);

			removeMatchTilesCommand.removeTile = removeTile;
			removeMatchTilesCommand.Execute ();

			IEnumerator iEnum = removeMatchTilesCommand.enumerator;
			iEnum.MoveNext ();
			iEnum.MoveNext ();

			matchTileFactory.Received().RemoveMatchTile (matchTile);	
		}

		[Test]
		public void GiveTwoMatchTilesAddedToRemoveTile_WhenExecute_ThenRemoveBothMatchTiles()
		{
			MatchTile matchTile = new MatchTile ();
			matchTile.type = MatchTileType.MatchToken_A;
			matchTile.canMove = true;

			List<MatchTile> matchTiles = new List<MatchTile> ();
			matchTiles.Add (matchTile);
			matchTiles.Add (matchTile);

			RemoveTile removeTile = new RemoveTile ();
			removeTile.matchTiles = matchTiles;

			matchTileGridModel.CanMove (matchTile).Returns (true);

			removeMatchTilesCommand.removeTile = removeTile;
			removeMatchTilesCommand.Execute ();

			IEnumerator iEnum = removeMatchTilesCommand.enumerator;
			iEnum.MoveNext ();
			iEnum.MoveNext ();
			iEnum.MoveNext ();
			iEnum.MoveNext ();

			matchTileFactory.Received(2).RemoveMatchTile (matchTile);	
		}

		[Test]
		public void GivenOneMatchTileInRemoveTile_WhenExecute_ThenClearTilesTouched()
		{
			MatchTile matchTile = new MatchTile ();
			matchTile.type = MatchTileType.MatchToken_A;
			matchTile.canMove = true;

			List<MatchTile> matchTiles = new List<MatchTile> ();
			matchTiles.Add (matchTile);

			RemoveTile removeTile = new RemoveTile ();
			removeTile.matchTiles = matchTiles;

			removeMatchTilesCommand.removeTile = removeTile;
			removeMatchTilesCommand.Execute ();

			IEnumerator iEnum = removeMatchTilesCommand.enumerator;
			iEnum.MoveNext ();
			iEnum.MoveNext ();

			IEnumerator loopEnumerator = removeMatchTilesCommand.loopEnumerator;
			loopEnumerator.MoveNext ();
			loopEnumerator.MoveNext ();

			IEnumerator endEnumerator = removeMatchTilesCommand.endEnumerator;
			endEnumerator.MoveNext ();
			endEnumerator.MoveNext ();

			matchTileGridModel.Received ().ClearTilesTouched ();
		}

		[Test]
		public void GivenOneMatchTileInRemoveTile_WhenExecute_ThenBroadcastRemoveTilesComplete()
		{
			MatchTile matchTile = new MatchTile ();
			matchTile.type = MatchTileType.MatchToken_A;
			matchTile.canMove = true;

			List<MatchTile> matchTiles = new List<MatchTile> ();
			matchTiles.Add (matchTile);

			RemoveTile removeTile = new RemoveTile ();
			removeTile.matchTiles = matchTiles;


			removeMatchTilesCommand.removeTile = removeTile;
			removeMatchTilesCommand.Execute ();

			IEnumerator iEnum = removeMatchTilesCommand.enumerator;
			iEnum.MoveNext ();
			iEnum.MoveNext ();

			IEnumerator loopEnumerator = removeMatchTilesCommand.loopEnumerator;
			loopEnumerator.MoveNext ();
			loopEnumerator.MoveNext ();

			Messenger.AddListener (MatchTileGridMessage.REMOVE_TILES_COMPLETE, () => 
			{
				Assert.Pass();	
			});

			IEnumerator endEnumerator = removeMatchTilesCommand.endEnumerator;
			endEnumerator.MoveNext ();
			endEnumerator.MoveNext ();
		}

	}
}