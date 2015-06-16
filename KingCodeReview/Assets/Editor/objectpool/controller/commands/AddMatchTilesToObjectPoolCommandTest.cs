using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using NSubstitute;
using UnityEngine;
using MatchTileGrid;

// Using the Given When Then approach:
// http://martinfowler.com/bliki/GivenWhenThen.html
namespace ObjectPool
{
	[TestFixture]
	internal class AddMatchTilesToObjectPoolCommandTest
	{
		private AddMatchTilesToObjectPoolCommand addMatchTilesToObjectPoolCommand;
		private MatchTileGridModel matchTileGridModel;
		private IObjectPoolModel objectPoolModel;

		[SetUp]
		public void SetUp()
		{
			matchTileGridModel = Substitute.For<MatchTileGridModel>();
			objectPoolModel = Substitute.For<IObjectPoolModel>();

			addMatchTilesToObjectPoolCommand = new AddMatchTilesToObjectPoolCommand ();
			addMatchTilesToObjectPoolCommand.matchTileGridModel = matchTileGridModel;
			addMatchTilesToObjectPoolCommand.objectPoolModel = objectPoolModel;
		}

		[Test]
		public void GivenAddMatchTilesToObjectPoolCommand_WhenExecute_ThenVerifyObjectPoolEntryDataAddedToObjectPool()
		{
			addMatchTilesToObjectPoolCommand.Execute ();

			objectPoolModel.Received().AddObjectPoolEntry(Arg.Any<ObjectPoolEntry>());
		}

		[Test]
		public void GivenAddEveryTypeOfMatchTileTypeToObjectPoolExecptNullValue_WhenExecute_ThenVerifyObjectPoolEntryDataAddedEveryMatchTileType()
		{
			addMatchTilesToObjectPoolCommand.Execute ();

			MatchTileType[] matchTileTypes = (MatchTileType[]) Enum.GetValues(typeof(MatchTileType));
			// -1 as we don't want to count for the Null value
			int allMatchTiles = matchTileTypes.Length - 1;

			objectPoolModel.Received (allMatchTiles).AddObjectPoolEntry (Arg.Any<ObjectPoolEntry> ());
		}
	}
}
