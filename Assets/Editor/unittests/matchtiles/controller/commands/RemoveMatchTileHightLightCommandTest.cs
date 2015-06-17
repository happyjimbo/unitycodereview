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
	internal class RemoveMatchTileHightLightCommandTest
	{
		private RemoveMatchTileHightLightCommand removeMatchTileHightLightCommand;
		private IMatchTileGridModel matchTileGridModel;

		[SetUp]
		public void SetUp()
		{
			matchTileGridModel = Substitute.For<IMatchTileGridModel>();

			removeMatchTileHightLightCommand = new RemoveMatchTileHightLightCommand ();
			removeMatchTileHightLightCommand.matchTileGridModel = matchTileGridModel;

			Messenger.CleanAndDestroy();
		}

		[Test]
		public void GivenMatchTilesTouched_WhenExecute_ThenMatchTileComponentTileMethodCalled()
		{
			MatchTile matchTile = new MatchTile ();
			matchTile.canTouch = true;
			matchTile.position = Vector2.zero;
			matchTile.tileObject = new GameObject();

			List<MatchTile> matchTiles = new List<MatchTile>();
			matchTiles.Add (matchTile);
			matchTileGridModel.GetTilesTouched ().Returns(matchTiles);

			IMatchTileComponent matchTileComponenet = Substitute.For<IMatchTileComponent> ();
			matchTileGridModel.GetMatchTileComponent (Arg.Any<MatchTile>()).Returns (matchTileComponenet);

			removeMatchTileHightLightCommand.Execute ();

			matchTileComponenet.Received ().Tile ();
		}
	}
}