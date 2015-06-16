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
		public void GivenGetHintMatchTilesReturnsList_WhenExecuteDisplayHint_ThenGetHintMatchTiles()
		{
			List<MatchTile> hintMatchTiles = new List<MatchTile> ();

			matchTileGridModel.GetHintMatchTiles ().Returns (hintMatchTiles);

			matchTileGridModel.lastTouchedTimestamp = Time.time - 10;
			matchTileHintCommand.Execute ();

			IEnumerator iEnum = matchTileHintCommand.enumerator;
			iEnum.MoveNext ();
			iEnum.MoveNext ();

			matchTileGridModel.Received ().GetHintMatchTiles ();
		}
	}
}