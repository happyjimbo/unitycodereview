using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using NSubstitute;
using UnityEngine;
using EventDispatcher;

// Using the Given When Then approach:
// http://martinfowler.com/bliki/GivenWhenThen.html
namespace MatchTileGrid
{
	[TestFixture]
	internal class CalculateTilesToRemoveCommandTest
	{
		private CalculateTilesToRemoveCommand calculateTilesToRemoveCommand;
		private IMatchTileGridModel matchTileGridModel;
		private IEventDispatcher eventDispatcher;

		[SetUp]
		public void SetUp()
		{
			matchTileGridModel = Substitute.For<IMatchTileGridModel>();
			eventDispatcher = Substitute.For<IEventDispatcher> ();

			calculateTilesToRemoveCommand = new CalculateTilesToRemoveCommand ();
			calculateTilesToRemoveCommand.matchTileGridModel = matchTileGridModel;
			calculateTilesToRemoveCommand.eventDispatcher = eventDispatcher;

			matchTileGridModel.allowTouch = true;

			eventDispatcher.CleanAndDestroy();
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

			calculateTilesToRemoveCommand.Execute ();

			eventDispatcher.Received ().Broadcast (MatchTileGridMessage.REMOVE_TILES, Arg.Any<RemoveTile> ());
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