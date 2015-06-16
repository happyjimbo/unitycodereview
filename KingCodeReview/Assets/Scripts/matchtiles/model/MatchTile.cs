using System;
using System.Collections.Generic;
using UnityEngine;

namespace MatchTileGrid
{
	public enum MatchTileType
	{
		Null = 0,
		MatchToken_A = 1,
		MatchToken_B = 2,
		MatchToken_C = 3,
		MatchToken_D = 4,
		MatchToken_E = 5,
		MatchToken_F = 6,
		MatchToken_G = 7,
		Blank = 8,
		RemainingClearance = 9
	}

	public class MatchTile
	{
		public MatchTileType type;
		public Vector2 position;
		public GameObject tileObject;
		public bool canMove;
		public bool canTouch;
	}

	public struct RemoveTile
	{
		// This is a list so that we can remove multiple tiles at the same time.
		public List<MatchTile> matchTiles;
		public int tilesChained;
		public bool lastInChain;
		public bool special;
		public bool touched;
	}
}