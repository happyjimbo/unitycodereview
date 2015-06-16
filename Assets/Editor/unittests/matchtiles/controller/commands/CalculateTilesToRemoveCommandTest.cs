using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using NSubstitute;
using UnityEngine;

// Using the Given When Then approach:
// http://martinfowler.com/bliki/GivenWhenThen.html
namespace MatchTileGrid
{
	[TestFixture]
	internal class CalculateTilesToRemoveCommandTest
	{
		private CalculateTilesToRemoveCommand calculateTilesToRemoveCommand;
		private IMatchTileGridModel matchTileGridModel;

		[SetUp]
		public void SetUp()
		{
			matchTileGridModel = Substitute.For<IMatchTileGridModel>();

			calculateTilesToRemoveCommand = new CalculateTilesToRemoveCommand ();
			calculateTilesToRemoveCommand.matchTileGridModel = matchTileGridModel;

			matchTileGridModel.allowTouch = true;

			Messenger.CleanAndDestroy();
		}

		[Test]
		public void GivenValidMoveTouchEndedTrue_WhenExecute_ThenBroadcastRemoveTiles()
		{
			MatchTile tile = new MatchTile ();
			tile.tileObject = new GameObject ();

			List<MatchTile> matchTiles = new List<MatchTile> ();
			matchTiles.Add (tile);

			matchTileGridModel.GetTilesTouched ().Returns (matchTiles);

			matchTileGridModel.ValidMoveTouchEnded ().Returns (true);

			Messenger.AddListener <RemoveTile> (MatchTileGridMessage.REMOVE_TILES, (RemoveTile removeTile) =>
			{
				Assert.Pass();
			});

			calculateTilesToRemoveCommand.Execute ();

			Assert.Fail ();
		}

		[Test]
		public void GivenValidMoveTouchEndeFalse_WhenExecute_ThenMatchTileGridModelClearTilesTouched()
		{
			matchTileGridModel.ValidMoveTouchEnded ().Returns (false);

			calculateTilesToRemoveCommand.Execute ();

			matchTileGridModel.Received ().ClearTilesTouched ();
		}
	}
}