using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using NSubstitute;
using UnityEngine;
using Touched;
using EventDispatcher;

// Using the Given When Then approach:
// http://martinfowler.com/bliki/GivenWhenThen.html
namespace MatchTileGrid
{
	[TestFixture]
	internal class CreateMatchTileGridCommandTest
	{
		private CreateMatchTileGridCommand createMatchTileGridCommand;
		private IMatchTileGridModel matchTileGridModel;
		private IMatchTileFactory matchTileFactory;
		private IEventDispatcher eventDispatcher;

		[SetUp]
		public void SetUp()
		{
			matchTileGridModel = Substitute.For<IMatchTileGridModel>();
			matchTileFactory = Substitute.For<IMatchTileFactory>();
			eventDispatcher = Substitute.For<IEventDispatcher> ();
			
			createMatchTileGridCommand = new CreateMatchTileGridCommand ();
			createMatchTileGridCommand.matchTileGridModel = matchTileGridModel;
			createMatchTileGridCommand.matchTileFactory = matchTileFactory;
			createMatchTileGridCommand.eventDispatcher = eventDispatcher;

			eventDispatcher.CleanAndDestroy();
		}

		[Test]
		public void GivenMatchTilesDataLoadedTwoByTwoWithNoMatchTokenData_WhenExecute_ThenCreateFourRandomMatchTile()
		{
			MatchTilesData layout = ScriptableObject.CreateInstance<MatchTilesData> ();
			layout.matchTilesData = new List<MatchTileData> ();

			matchTileGridModel.matchTilesData.Returns (layout);
			matchTileGridModel.gridSize = new Vector2 (2, 2);

			createMatchTileGridCommand.Execute ();

			matchTileFactory.Received (4).CreateRandomMatchTile (Arg.Any<Vector2> ());
		}

		[Test]
		public void GivenMatchTilesDataLoadedWithExistingMatchTokenData_WhenExecute_ThenCreateGivenMatchTileAndThreeRandomTiles()
		{
			MatchTileType type = MatchTileType.MatchToken_A;

			GameObject gameObject = new GameObject ();
			gameObject.name = type.ToString ();

			MatchTileData data = new MatchTileData ();
			data.position = Vector2.zero;
			data.gameObject = gameObject;

			List<MatchTileData> matchTilesData = new List<MatchTileData> ();
			matchTilesData.Add (data);

			MatchTilesData layout = ScriptableObject.CreateInstance<MatchTilesData> ();
			layout.matchTilesData = matchTilesData;

			matchTileGridModel.matchTilesData.Returns (layout);
			matchTileGridModel.GetMatchTileType (gameObject.name).Returns(type);
			matchTileGridModel.gridSize = new Vector2 (2, 2);

			createMatchTileGridCommand.Execute ();

			matchTileFactory.Received (1).CreateMatchTile (Arg.Any<MatchTileType> (), Arg.Any<Vector2> ());
			matchTileFactory.Received (3).CreateRandomMatchTile (Arg.Any<Vector2> ());
		}

		[Test]
		public void GivenMatchTilesDataLoadedWithExistingNullMatchTokenData_WhenExecute_ThenCreateFourRandomMatchTile()
		{
			MatchTileType type = MatchTileType.Null;

			GameObject gameObject = new GameObject ();
			gameObject.name = type.ToString ();

			MatchTileData data = new MatchTileData ();
			data.position = Vector2.zero;
			data.gameObject = gameObject;

			List<MatchTileData> matchTilesData = new List<MatchTileData> ();
			matchTilesData.Add (data);

			MatchTilesData layout = ScriptableObject.CreateInstance<MatchTilesData> ();
			layout.matchTilesData = matchTilesData;

			matchTileGridModel.matchTilesData.Returns (layout);
			matchTileGridModel.GetMatchTileType (gameObject.name).Returns(type);
			matchTileGridModel.gridSize = new Vector2 (2, 2);

			createMatchTileGridCommand.Execute ();

			matchTileFactory.Received (4).CreateRandomMatchTile (Arg.Any<Vector2> ());
		}

		[Test]
		public void GivenAddListenerForCheckMovesRemaining_WhenExecute_ThenBroadcaseCheckMovesRemaining()
		{
			createMatchTileGridCommand.Execute ();
			eventDispatcher.Received ().Broadcast (MatchTileGridMessage.CHECK_MOVES_REMAINING);
		}
	}
}