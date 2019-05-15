using System;

namespace MatchTileGrid
{
	public class MatchTileGridMessage
	{
		public const string GRID_HOLDER_CREATED = "GRID_HOLDER_CREATED";
		public const string GRID_CREATED = "MATCH_TILE_GRID_CREATED";
		public const string CREATE_NEW_TILE = "MATCH_TILE_CREATE_NEW";
		public const string CHECK_MOVES_REMAINING = "MATCH_TILE_CHECK_MOVES_REMAINING";
		public const string SHUFFLE_GRID = "MATCH_TILE_SHUFFLE_GRID";
		public const string MATCH_TILE_COLLECTED = "MATCH_TILE_COLLECTED";
		public const string OBSTACLE_COLLECTED = "OBSTACLE_COLLECTED";
		public const string HIDE_INVALID_TILES = "HIDE_INVALID_TILES";
		public const string MATCH_SCORE_ADDED = "MATCH_SCORE_ADDED";
		public const string SPECIAL_TILE = "SPECIAL_TILE";

		public const string TILE_SELECTED = "TILE_SELECTED";

		public const string ALLOW_FALLING_TILES = "ALLOW_FALLING_TILES";

		public const string REMOVE_TILES = "REMOVE_TILES";
		public const string REMOVE_OBSTACLE = "REMOVE_OBSTACLE";

		public const string CREATE_OBSTACLE_RAN_POS = "CREATE_OBSTACLE_RAN_POS";

		public const string REMOVE_TILES_COMPLETE = "REMOVE_TILES_COMPLETE";

		public const string UPDATE_TRAPPER = "UPDATE_TRAPPER";
	}
}