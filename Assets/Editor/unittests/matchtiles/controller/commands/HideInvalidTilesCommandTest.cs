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
	internal class HideInvalidTilesCommandTest
	{
		private HideInvalidTilesCommand hideInvalidTilesCommand;
		private IMatchTileGridModel matchTileGridModel;

		[SetUp]
		public void SetUp()
		{
			matchTileGridModel = Substitute.For<IMatchTileGridModel>();

			hideInvalidTilesCommand = new HideInvalidTilesCommand ();
			hideInvalidTilesCommand.matchTileGridModel = matchTileGridModel;
		}

		[Test]
		public void GivenHideNotValidTiles_WhenExecute_ThenHideAllMatchTilesNotOfType()
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

			matchTileGridModel.GetAllMatchTilesNotOfType (Arg.Any<MatchTileType> ()).Returns (matchTiles);

			hideInvalidTilesCommand.hideType = HideType.NotValidTiles;
			hideInvalidTilesCommand.Execute ();

			matchTileComponenet.Received ().Hide ();
		}

		[Test]
		public void GivenShowAllTiles_WhenExecute_ThenShowAllMatchTiles()
		{
			matchTileGridModel.allowTouch = true;

			MatchTile matchTile = new MatchTile ();
			matchTile.canTouch = true;
			matchTile.position = Vector2.zero;
			matchTile.tileObject = new GameObject();

			Dictionary<Vector2, MatchTile> matchTilesDic = new Dictionary<Vector2, MatchTile> ();
			matchTilesDic [matchTile.position] = matchTile;

			matchTileGridModel.GetMatchTiles ().Returns (matchTilesDic);

			IMatchTileComponent matchTileComponenet = Substitute.For<IMatchTileComponent> ();
			matchTileGridModel.GetMatchTileComponent (Arg.Any<MatchTile>()).Returns (matchTileComponenet);

			hideInvalidTilesCommand.hideType = HideType.ShowAll;
			hideInvalidTilesCommand.Execute ();

			matchTileComponenet.Received ().Show ();
		}

		[Test]
		public void GivenShowAllTilesAllowTouchFalse_WhenExecute_ThenDoNotShowAllMatchTiles()
		{
			matchTileGridModel.allowTouch = false;

			MatchTile matchTile = new MatchTile ();
			matchTile.canTouch = true;
			matchTile.position = Vector2.zero;
			matchTile.tileObject = new GameObject();

			Dictionary<Vector2, MatchTile> matchTilesDic = new Dictionary<Vector2, MatchTile> ();
			matchTilesDic [matchTile.position] = matchTile;

			matchTileGridModel.GetMatchTiles ().Returns (matchTilesDic);

			IMatchTileComponent matchTileComponenet = Substitute.For<IMatchTileComponent> ();
			matchTileGridModel.GetMatchTileComponent (Arg.Any<MatchTile>()).Returns (matchTileComponenet);

			hideInvalidTilesCommand.hideType = HideType.ShowAll;
			hideInvalidTilesCommand.Execute ();

			matchTileComponenet.DidNotReceive ().Show ();
		}
	}
}