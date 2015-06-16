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
	internal class AllowFallingTilesCommandTest
	{
		private AllowFallingTilesCommand allowFallingTilesCommand;
		private IMatchTileGridModel matchTileGridModel;

		[SetUp]
		public void SetUp()
		{
			matchTileGridModel = Substitute.For<IMatchTileGridModel>();

			allowFallingTilesCommand = new AllowFallingTilesCommand ();
			allowFallingTilesCommand.matchTileGridModel = matchTileGridModel;

			Messenger.CleanAndDestroy();
		}

		[Test]
		public void GivenPauseTilesFallingTrue_WhenIEnumeratorMoveNext_ThenPauseTilesFallingSetToFalse()
		{
			matchTileGridModel.pauseTilesFalling = true;

			allowFallingTilesCommand.Execute ();

			IEnumerator iEnum = allowFallingTilesCommand.enumerator;
			iEnum.MoveNext ();
			iEnum.MoveNext ();

			Assert.False (matchTileGridModel.pauseTilesFalling);
		}

		[Test]
		public void GivenMovesRemainingSetToOne_WhenIEnumeratorMoveNext_ThenMovesRemainingSetToZero()
		{
			matchTileGridModel.movesRemaining = 1;

			allowFallingTilesCommand.Execute ();

			IEnumerator iEnum = allowFallingTilesCommand.enumerator;
			iEnum.MoveNext ();
			iEnum.MoveNext ();

			Assert.AreEqual (matchTileGridModel.movesRemaining, 0);
		}

		[Test]
		public void GivenAddListenerForMessage_WhenIEnumeratorMoveNext_ThenMessageBroadcastSuccessful()
		{
			Messenger.AddListener(MatchTileGridMessage.CREATE_NEW_TILE, () =>
			{
				Assert.Pass();
			});

			allowFallingTilesCommand.Execute ();

			IEnumerator iEnum = allowFallingTilesCommand.enumerator;
			iEnum.MoveNext ();
			iEnum.MoveNext ();

			// If the test is successful then this is not called.
			Assert.Fail ();
		}
	}
}