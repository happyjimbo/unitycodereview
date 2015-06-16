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
	internal class CreateMatchTileGridHolderCommandTest
	{
		private CreateMatchTileGridHolderCommand createMatchTileGridHolderCommand;
		private IMatchTileGridModel matchTileGridModel;

		[SetUp]
		public void SetUp()
		{
			matchTileGridModel = Substitute.For<IMatchTileGridModel>();
			matchTileGridModel.gridHolder = new GameObject ();

			createMatchTileGridHolderCommand = new CreateMatchTileGridHolderCommand ();
			createMatchTileGridHolderCommand.matchTileGridModel = matchTileGridModel;

			MatchTilesData matchTilesData = ScriptableObject.CreateInstance<MatchTilesData> ();
			matchTileGridModel.matchTilesData.Returns (matchTilesData);

			Messenger.CleanAndDestroy();
		}

		[Test]
		public void GivenGridParentIsNull_WhenExecute_ThenGridParentIsNotNull()
		{
			Assert.Null (matchTileGridModel.gridParent);

			createMatchTileGridHolderCommand.Execute ();

			Assert.NotNull (matchTileGridModel.gridParent);
		}

		[Test]
		public void GivenAddListenerForGridHolderCreated_WhenExecute_ThenBroadcaseGridHolderCreated()
		{
			Messenger.AddListener(MatchTileGridMessage.GRID_HOLDER_CREATED, () =>
			{
				Assert.Pass();
			});

			createMatchTileGridHolderCommand.Execute ();

			Assert.Fail();
		}
	}
}