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
	internal class MatchTileTouchedCommandTest
	{
		private MatchTileTouchedCommand matchTileTouchedCommand;
		private IMatchTileGridModel matchTileGridModel;

		[SetUp]
		public void SetUp()
		{
			matchTileGridModel = Substitute.For<IMatchTileGridModel>();

			matchTileTouchedCommand = new MatchTileTouchedCommand ();
			matchTileTouchedCommand.matchTileGridModel = matchTileGridModel;

			GameObject gameObject = new GameObject ();
			gameObject.transform.localPosition = Vector3.zero;

			TouchedObject touchedObject = new TouchedObject ();
			touchedObject.objectHit = gameObject;

			matchTileTouchedCommand.touchedObject = touchedObject;

			Messenger.CleanAndDestroy();
		}

		[Test]
		public void GivenCanTouchTile_WhenTouchObjectExecute_ThenAddTileTouchedToModel()
		{
			matchTileGridModel.allowTouch = true;

			MatchTile matchTile = new MatchTile ();
			matchTile.canTouch = true;
			matchTile.position = Vector2.zero;
			matchTile.tileObject = new GameObject ();

			matchTileGridModel.GetMatchTile (Vector2.zero).Returns (matchTile);
			matchTileGridModel.CanTouchTile (matchTile).Returns (true);

			matchTileTouchedCommand.Execute ();

			matchTileGridModel.Received ().AddTileTouched (Arg.Any<Vector2> (), Arg.Any<MatchTile> ());
		}

		[Test]
		public void GivenMatchTilesTouchedAddedToModel_WhenTouchObjectExecute_ThenPreviousTileSelected()
		{
			MatchTile matchTile = new MatchTile ();
			matchTile.canTouch = true;
			matchTile.position = Vector2.zero;
			matchTile.tileObject = new GameObject ();

			List<MatchTile> matchTiles = new List<MatchTile>();
			matchTiles.Add (matchTile);
			matchTiles.Add (matchTile);
			 
			matchTileGridModel.allowTouch = true;
			matchTileGridModel.GetTilesTouched ().Returns(matchTiles);
			matchTileGridModel.GetMatchTile (Vector2.zero).Returns (matchTile);

			matchTileTouchedCommand.Execute ();

			matchTileGridModel.Received ().RemoveTileTouched (Arg.Any<MatchTile> ());
		}

		/*[Test]
		public void GivenMatchTileSlotIsNotEmpty_WhenExecute_ThenBroadcastCheckMovesRemaining()
		{
			Messenger.AddListener(MatchTileGridMessage.CHECK_MOVES_REMAINING, () =>
				{
					Assert.Pass();
				});

			matchTileGridModel.gridSize = new Vector2 (1, 1);
			matchTileGridModel.GetMatchTile (new Vector2(0, 0)).Returns (new MatchTile());

			matchTileTouchedCommand.Execute ();

			IEnumerator iEnum = matchTileTouchedCommand.enumerator;
			iEnum.MoveNext ();
			iEnum.MoveNext ();

			Assert.Fail ();
		}*/
	}
}