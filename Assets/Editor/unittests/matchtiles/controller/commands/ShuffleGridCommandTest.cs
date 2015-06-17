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
	internal class ShuffleGridCommandTest
	{
		private ShuffleGridCommand shuffleGridCommand;
		private IMatchTileGridModel matchTileGridModel;

		[SetUp]
		public void SetUp()
		{
			matchTileGridModel = Substitute.For<IMatchTileGridModel>();

			shuffleGridCommand = new ShuffleGridCommand ();
			shuffleGridCommand.matchTileGridModel = matchTileGridModel;

			Messenger.CleanAndDestroy();
		}

		[Test]
		public void GivenMatchTilesCreated_WhenExecute_ThenRemoveBothTilesFromModel()
		{
			matchTileGridModel.gridSize = new Vector2 (1, 2);

			MatchTile matchTile = new MatchTile ();
			matchTile.canMove = true;
			matchTile.position = new Vector2 (0, 0);
			matchTile.tileObject = new GameObject();

			MatchTile secondMatchTile = new MatchTile ();
			secondMatchTile.canMove = true;
			secondMatchTile.position = new Vector2 (0, 1);
			secondMatchTile.tileObject = new GameObject();

			Dictionary<Vector2, MatchTile> matchTilesDic = new Dictionary<Vector2, MatchTile> ();
			matchTilesDic [matchTile.position] = matchTile;
			matchTilesDic [secondMatchTile.position] = secondMatchTile;

			matchTileGridModel.GetMatchTiles ().Returns (matchTilesDic);

			shuffleGridCommand.Execute ();

			matchTileGridModel.Received(2).RemoveTile (Arg.Any<Vector2> ());
		}

		[Test]
		public void GivenMatchTilesCreated_WhenExecute_ThenAddNewTileToModel()
		{
			matchTileGridModel.gridSize = new Vector2 (1, 2);

			MatchTile matchTile = new MatchTile ();
			matchTile.canMove = true;
			matchTile.position = new Vector2 (0, 0);
			matchTile.tileObject = new GameObject();

			MatchTile secondMatchTile = new MatchTile ();
			secondMatchTile.canMove = true;
			secondMatchTile.position = new Vector2 (0, 1);
			secondMatchTile.tileObject = new GameObject();

			Dictionary<Vector2, MatchTile> matchTilesDic = new Dictionary<Vector2, MatchTile> ();
			matchTilesDic [matchTile.position] = matchTile;
			matchTilesDic [secondMatchTile.position] = secondMatchTile;

			matchTileGridModel.GetMatchTiles ().Returns (matchTilesDic);

			shuffleGridCommand.Execute ();

			matchTileGridModel.Received(2).AddNewTile (Arg.Any<MatchTile> ());
		}

		[Test]
		public void GivenAddListenerCheckMovesRemaining_WhenExecute_ThenBroadcastCheckMovesRemaining()
		{
			Messenger.AddListener (MatchTileGridMessage.CHECK_MOVES_REMAINING, () => 
			{
				Assert.Pass();		
			});

			shuffleGridCommand.Execute ();

			Assert.Fail ();
		}

	}
}