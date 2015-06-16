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
	internal class LoadMatchTileGridDataCommandTest
	{
		private LoadMatchTileGridDataCommand loadMatchTileGridDataCommand;
		private IMatchTileGridModel matchTileGridModel;

		[SetUp]
		public void SetUp()
		{
			matchTileGridModel = Substitute.For<IMatchTileGridModel>();

			loadMatchTileGridDataCommand = new LoadMatchTileGridDataCommand ();
			loadMatchTileGridDataCommand.matchTileGridModel = matchTileGridModel;
		}

		[Test]
		public void GivenMatchTilesDataSetToNotBeNull_WhenExecute_ThenMatchTilesDataIsNullAsItHasBeenModified()
		{
			MatchTilesData matchTilesData = ScriptableObject.CreateInstance<MatchTilesData> ();
			matchTileGridModel.matchTilesData = matchTilesData;

			Assert.NotNull (matchTileGridModel.matchTilesData);

			loadMatchTileGridDataCommand.Execute ();

			Assert.Null (matchTileGridModel.matchTilesData);
		}
	}
}