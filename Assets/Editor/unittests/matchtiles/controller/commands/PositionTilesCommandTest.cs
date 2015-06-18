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
	internal class PositionTilesCommandTest
	{
		private PositionTilesCommand positionTilesCommand;
		private IMatchTileGridModel matchTileGridModel;

		[SetUp]
		public void SetUp()
		{
			matchTileGridModel = Substitute.For<IMatchTileGridModel>();

			positionTilesCommand = new PositionTilesCommand ();
			positionTilesCommand.matchTileGridModel = matchTileGridModel;
		}

		[Test]
		public void GivenGridIsSetUpWithMatchTilesAndEmptySlot_WhenExecute_ThenMatchTileGridModelMoveTileIsCalled()
		{
			matchTileGridModel.gridSize = new Vector2 (1, 2);

			Vector2 first = new Vector2 (0, 1);
			Vector2 second = new Vector2 (0, 0);

			Dictionary<Vector2, MatchTile> matchTilesDic = new Dictionary<Vector2, MatchTile> ();
			matchTilesDic [first] = CreateMatchTile (MatchTileType.MatchToken_A, first);

			matchTileGridModel.GetMatchTiles ().Returns (matchTilesDic);
			matchTileGridModel.GetMatchTile (first).Returns (matchTilesDic [first]);
			// We can't set this as null directly, but using a lambda enables us to..
			matchTileGridModel.GetMatchTile (second).Returns (a => null);

			positionTilesCommand.Execute ();

			IEnumerator iEnum = positionTilesCommand.enumerator;
			iEnum.MoveNext ();
			iEnum.MoveNext ();

			matchTileGridModel.Received().MoveTile (Arg.Any<Vector2>(), Arg.Any<Vector2>());
		}

		[Test]
		public void GivenGridIsSetUpWithMatchTilesNoEmptySlot_WhenExecute_ThenMatchTileGridModelMoveTileIsNotCalled()
		{
			matchTileGridModel.gridSize = new Vector2 (1, 2);

			Vector2 first = new Vector2 (0, 1);
			Vector2 second = new Vector2 (0, 0);

			Dictionary<Vector2, MatchTile> matchTilesDic = new Dictionary<Vector2, MatchTile> ();
			matchTilesDic [first] = CreateMatchTile (MatchTileType.MatchToken_A, first);
			matchTilesDic [second] = CreateMatchTile (MatchTileType.MatchToken_A, second);

			matchTileGridModel.GetMatchTiles ().Returns (matchTilesDic);
			matchTileGridModel.GetMatchTile (first).Returns (matchTilesDic [first]);
			matchTileGridModel.GetMatchTile (second).Returns (matchTilesDic [second]);

			positionTilesCommand.Execute ();

			IEnumerator iEnum = positionTilesCommand.enumerator;
			iEnum.MoveNext ();
			iEnum.MoveNext ();

			matchTileGridModel.DidNotReceive().MoveTile (Arg.Any<Vector2>(), Arg.Any<Vector2>());
		}

		private MatchTile CreateMatchTile(MatchTileType type, Vector2 position)
		{
			MatchTile tile = new MatchTile ();
			tile.canMove = true;
			tile.type = type;
			tile.position = position;
			tile.tileObject = new GameObject ();

			return tile;
		}
	}
}