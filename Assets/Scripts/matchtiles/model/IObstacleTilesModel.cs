using System.Collections.Generic;
using UnityEngine;

namespace MatchTileGrid
{
    public interface IObstacleTilesModel
    {
        bool regeneratorObstacleRemoved { get; set; }
        void AddObstacleTile(ObstacleTile obstacleTile);
        void RemoveObstacleTile(Vector2 key);
        ObstacleTile GetMatchObstacle(Vector2 pos);
        bool ObstacleAlreadyRemoved(ObstacleTile obstacle);
        void RemovedObstacle(ObstacleTile obstacle);
        void ClearRemovedObstacles();
        List<ObstacleTile> GetAllObstaclesOfType(MatchTileObstacleType type);
        bool ObstacleRemoveByAdjacentTile(MatchTileObstacleType type);
        void SwapObstacles(Vector2 firstPos, Vector2 secondPos);
        MatchTileObstacleType GetMatchTileObstacleType(string str);
        bool CanMove(Vector2 pos);
        bool CanMove(ObstacleTile obstacleTile);
        bool CanTouchTile(Vector2 pos);

        void SetObstaclesNeeded(MatchTileObstacleType type, int amount);

        void AddObstaclesCollected(MatchTileObstacleType type);

        int GetObstaclesNeeded(MatchTileObstacleType type);

        void AddObstacleOriginalType(ObstacleTile obstacle);

        MatchTileObstacleType ObstacleOriginalType(ObstacleTile obstacle);
    }
}