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

    public enum MatchTileObstacleType
    {
        Null = 0,
        MatchToken_Sticker = 1,
        MatchToken_Blocker_1 = 2,
        MatchToken_Blocker_2 = 3,
        MatchToken_Blocker_3 = 4,
        MatchToken_Locker_1 = 5,
        MatchToken_Locker_2 = 6,
        MatchToken_Locker_3 = 7,
        MatchToken_Trapper = 8,
        MatchToken_Regenerator = 9,
        MatchToken_Key = 10
    }

    // We're using the combo value needed to to create the special tile, as it's value.
    // Negative numbers for specials that are not tied to combo's. 
    public enum MatchTileSpecialType
    {
        Null = 0,
        Special_Horizontal = 5,
        Special_Vertical = 6,
        Special_Surrounding = 7,
        Special_Matching = 8,
    }

    public class MatchTile
    {
        public MatchTileType type;
        public Vector2 position;
        public GameObject tileObject;
        public SpecialTile specialTile;
        public bool canMove;
        public bool canTouch;
    }

    public struct SpecialTile
    {
        public MatchTileSpecialType type;
        public GameObject specialObject;
    }

    public class ObstacleTile
    {
        public MatchTileObstacleType type;
        public Vector2 position;
        public GameObject obstacleObject;
        public int counter;
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

    public class SpecialTileCharge
    {
        public MatchTileType type;
        public MatchTileSpecialType specialType;
        public int chargeNeeded;
        public int currentCharge;
    }
}