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
	internal class CreateNewTileCommandTest
	{
		private CreateNewTileCommand createNewTileCommand;
		private IMatchTileGridModel matchTileGridModel;
		private IMatchTileFactory matchTileFactory;
		private IEventDispatcher eventDispatcher;

		[SetUp]
		public void SetUp()
		{
			matchTileGridModel = Substitute.For<IMatchTileGridModel>();
			matchTileFactory = Substitute.For<IMatchTileFactory> ();
			eventDispatcher = Substitute.For<IEventDispatcher> ();

			createNewTileCommand = new CreateNewTileCommand ();
			createNewTileCommand.matchTileGridModel = matchTileGridModel;
			createNewTileCommand.matchTileFactory = matchTileFactory;
			createNewTileCommand.eventDispatcher = eventDispatcher;
		}

		[Test]
		public void GivenMatchTileSlotIsEmpty_WhenExecute_ThenCreateRandomMatchTile()
		{
			matchTileGridModel.gridSize = new Vector2 (1, 1);

			// We can't set this as null directly, but using a lambda enables us to..
			matchTileGridModel.GetMatchTile (new Vector2(0, 0)).Returns (a => null);

			createNewTileCommand.Execute ();

			IEnumerator iEnum = createNewTileCommand.enumerator;
			iEnum.MoveNext ();
			iEnum.MoveNext ();

			matchTileFactory.Received ().CreateRandomMatchTile (Arg.Any<Vector2> (), Arg.Any<MatchTileObstacleType>());
		}

		[Test]
		public void GivenMatchTileSlotIsNotEmpty_WhenExecute_ThenBroadcastCheckMovesRemaining()
		{
			matchTileGridModel.gridSize = new Vector2 (1, 1);
			matchTileGridModel.GetMatchTile (new Vector2(0, 0)).Returns (new MatchTile());

			createNewTileCommand.Execute ();

			IEnumerator iEnum = createNewTileCommand.enumerator;
			iEnum.MoveNext ();
			iEnum.MoveNext ();

			eventDispatcher.Received ().Broadcast (MatchTileGridMessage.CHECK_MOVES_REMAINING);
		}
	}
}