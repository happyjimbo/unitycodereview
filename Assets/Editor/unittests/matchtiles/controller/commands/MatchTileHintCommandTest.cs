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
	internal class MatchTileHintCommandTest
	{
		private MatchTileHintCommand matchTileHintCommand;
		private IMatchTileGridModel matchTileGridModel;

		[SetUp]
		public void SetUp()
		{
			matchTileGridModel = Substitute.For<IMatchTileGridModel>();

			matchTileHintCommand = new MatchTileHintCommand ();
			matchTileHintCommand.matchTileGridModel = matchTileGridModel;
		}

		[Test]
		public void GivenGetHintMatchTilesReturnsList_WhenExecuteDisplayHint_ThenMatchTileComponentHint()
		{
			List<MatchTile> hintMatchTiles = new List<MatchTile> ();
			hintMatchTiles.Add (new MatchTile ());

			matchTileGridModel.GetHintMatchTiles ().Returns (hintMatchTiles);

			IMatchTileComponent matchTileComponenet = Substitute.For<IMatchTileComponent> ();
			matchTileGridModel.GetMatchTileComponent (Arg.Any<MatchTile>()).Returns (matchTileComponenet);

			matchTileGridModel.lastTouchedTimestamp = Time.time - 10;
			matchTileHintCommand.Execute ();

			IEnumerator iEnum = matchTileHintCommand.enumerator;
			iEnum.MoveNext ();
			iEnum.MoveNext ();

			matchTileComponenet.Received ().Hint (Arg.Any<int>());
		}
	}
}