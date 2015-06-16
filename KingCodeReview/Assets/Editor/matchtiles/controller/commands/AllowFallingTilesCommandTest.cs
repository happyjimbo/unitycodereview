using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using NSubstitute;
using UnityEngine;
using MatchTileGrid;

// Using the Given When Then approach:
// http://martinfowler.com/bliki/GivenWhenThen.html
namespace MatchTileGrid
{
	[TestFixture]
	internal class AllowFallingTilesCommandTest
	{
		private AllowFallingTilesCommand allowFallingTilesCommand;
		private MatchTileGridModel matchTileGridModel;

		[SetUp]
		public void SetUp()
		{
			matchTileGridModel = Substitute.For<MatchTileGridModel>();

			allowFallingTilesCommand = new AllowFallingTilesCommand ();
			allowFallingTilesCommand.matchTileGridModel = matchTileGridModel;
		}

		[Test]
		public void Given_When_Then()
		{
			allowFallingTilesCommand.Execute ();

			matchTileGridModel.Received ().pauseTilesFalling = false;
			matchTileGridModel.Received ().movesRemaining--;

			//objectPoolModel.Received().AddObjectPoolEntry(Arg.Any<ObjectPoolEntry>());
		}

		/*[Test]
		public void GivenAddEveryTypeOfMatchTileTypeToObjectPoolExecptNullValue_WhenExecute_ThenVerifyObjectPoolEntryDataAddedEveryMatchTileType()
		{
			addMatchTilesToObjectPoolCommand.Execute ();

			MatchTileType[] matchTileTypes = (MatchTileType[]) Enum.GetValues(typeof(MatchTileType));
			// -1 as we don't want to count for the Null value
			int allMatchTiles = matchTileTypes.Length - 1;

			objectPoolModel.Received (allMatchTiles).AddObjectPoolEntry (Arg.Any<ObjectPoolEntry> ());
		}*/
	}
}
