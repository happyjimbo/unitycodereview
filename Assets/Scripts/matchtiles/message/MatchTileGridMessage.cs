using System;

namespace MatchTileGrid
{
	public class MatchTileGridMessage
	{	
		public const string GRID_HOLDER_CREATED = "GRID_HOLDER_CREATED";
		public const string CREATE_NEW_TILE = "MATCH_TILE_CREATE_NEW";
		public const string CHECK_MOVES_REMAINING = "MATCH_TILE_CHECK_MOVES_REMAINING";
		public const string SHUFFLE_GRID = "MATCH_TILE_SHUFFLE_GRID";
		public const string MATCH_TILE_COLLECTED = "MATCH_TILE_COLLECTED";
		public const string HIDE_INVALID_TILES = "HIDE_INVALID_TILES";

		public const string TILE_SELECTED = "TILE_SELECTED";

		public const string ALLOW_FALLING_TILES = "ALLOW_FALLING_TILES";

		public const string REMOVE_TILES = "REMOVE_TILES";
	}
}