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
	internal class CheckMovesRemainingCommandTest
	{
		private CheckMovesRemainingCommand checkMovesRemainingCommand;
		private IMatchTileGridModel matchTileGridModel;

		[SetUp]
		public void SetUp()
		{
			matchTileGridModel = Substitute.For<IMatchTileGridModel>();

			checkMovesRemainingCommand = new CheckMovesRemainingCommand ();
			checkMovesRemainingCommand.matchTileGridModel = matchTileGridModel;

			Messenger.CleanAndDestroy();
		}

		[Test]
		public void GivenMatchTilesPopulatedWithNoMatches_WhenExecute_ThenShuffleGrid()
		{
			matchTileGridModel.gridSize = new Vector2 (2, 2);

			Vector2 first = new Vector2 (0, 0);
			Vector2 second = new Vector2 (0, 1);
			Vector2 third = new Vector2 (1, 0);
			Vector2 fourth = new Vector2 (1, 1);

			Dictionary<Vector2, MatchTile> matchTilesDic = new Dictionary<Vector2, MatchTile> ();
			matchTilesDic [first] = CreateMatchTile (MatchTileType.MatchToken_A, first);
			matchTilesDic [second] = CreateMatchTile (MatchTileType.MatchToken_B, second);
			matchTilesDic [third] = CreateMatchTile (MatchTileType.MatchToken_C, third);
			matchTilesDic [fourth] = CreateMatchTile (MatchTileType.MatchToken_D, fourth);

			matchTileGridModel.GetMatchTiles ().Returns (matchTilesDic);

			Messenger.AddListener (MatchTileGridMessage.SHUFFLE_GRID, () =>
			{
				Assert.Pass();
			});

			checkMovesRemainingCommand.Execute ();

			Messenger.CleanAndDestroy();
			Assert.Fail();
		}

		[Test]
		public void GivenMatchTilesPopulatedWithMatches_WhenExecute_ThenDontShuffleGrid()
		{
			matchTileGridModel.gridSize = new Vector2 (2, 2);

			Vector2 first = new Vector2 (0, 0);
			Vector2 second = new Vector2 (0, 1);
			Vector2 third = new Vector2 (1, 0);
			Vector2 fourth = new Vector2 (1, 1);

			MatchTile firstMatch = CreateMatchTile (MatchTileType.MatchToken_A, first);
			MatchTile secondMatch = CreateMatchTile (MatchTileType.MatchToken_A, second);
			MatchTile thirdMatch = CreateMatchTile (MatchTileType.MatchToken_A, third);
			MatchTile fourthMMatch = CreateMatchTile (MatchTileType.MatchToken_A, fourth);

			Dictionary<Vector2, MatchTile> matchTilesDic = new Dictionary<Vector2, MatchTile> ();
			matchTilesDic [first] = firstMatch;
			matchTilesDic [second] = secondMatch;
			matchTilesDic [third] = thirdMatch;
			matchTilesDic [fourth] = fourthMMatch;

			matchTileGridModel.GetMatchTiles ().Returns (matchTilesDic);

			matchTileGridModel.GetMatchTile (first).Returns (matchTilesDic [first]);
			matchTileGridModel.GetMatchTile (second).Returns (matchTilesDic [second]);
			matchTileGridModel.GetMatchTile (third).Returns (matchTilesDic [third]);
			matchTileGridModel.GetMatchTile (fourth).Returns (matchTilesDic [fourth]);

			matchTileGridModel.GetHintMatchTiles ().Returns (new List<MatchTile>());

			Messenger.AddListener (MatchTileGridMessage.SHUFFLE_GRID, () =>
			{
				Assert.Fail();
			});

			checkMovesRemainingCommand.Execute ();

			Assert.Pass();
		}

		private MatchTile CreateMatchTile(MatchTileType type, Vector2 position)
		{
			MatchTile tile = new MatchTile ();
			tile.canMove = true;
			tile.type = type;
			tile.position = position;

			return tile;
		}
	}
}