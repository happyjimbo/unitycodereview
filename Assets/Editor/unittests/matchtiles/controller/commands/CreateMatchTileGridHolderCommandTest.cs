using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using NSubstitute;
using UnityEngine;
using EventDispatcher;

// Using the Given When Then approach:
// http://martinfowler.com/bliki/GivenWhenThen.html
namespace MatchTileGrid
{
	[TestFixture]
	internal class CreateMatchTileGridHolderCommandTest
	{
		private CreateMatchTileGridHolderCommand createMatchTileGridHolderCommand;
		private IMatchTileGridModel matchTileGridModel;
		private IEventDispatcher eventDispatcher;

		[SetUp]
		public void SetUp()
		{
			matchTileGridModel = Substitute.For<IMatchTileGridModel>();
			matchTileGridModel.gridHolder = new GameObject ();

			eventDispatcher = Substitute.For<IEventDispatcher> ();

			createMatchTileGridHolderCommand = new CreateMatchTileGridHolderCommand ();
			createMatchTileGridHolderCommand.matchTileGridModel = matchTileGridModel;
			createMatchTileGridHolderCommand.eventDispatcher = eventDispatcher;

			MatchTilesData matchTilesData = ScriptableObject.CreateInstance<MatchTilesData> ();
			matchTileGridModel.matchTilesData.Returns (matchTilesData);

			eventDispatcher.CleanAndDestroy();
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
			createMatchTileGridHolderCommand.Execute ();
			eventDispatcher.Received ().Broadcast (MatchTileGridMessage.GRID_HOLDER_CREATED);
		}
	}
}