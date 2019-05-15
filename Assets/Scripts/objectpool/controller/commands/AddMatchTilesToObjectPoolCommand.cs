using System;
using UnityEngine;
using IoC;
using Command;
using MatchTileGrid;

namespace ObjectPool
{
	public class AddMatchTilesToObjectPoolCommand : ICommand
	{
		[Inject]
		public IMatchTileGridModel matchTileGridModel { private get; set; }

		[Inject]
		public IObjectPoolModel objectPoolModel { private get; set; }

		public void Execute()
		{
			string dir = matchTileGridModel.matchTileLocation;

			MatchTileType[] matchTileTypes = (MatchTileType[]) Enum.GetValues(typeof(MatchTileType));
			for (int i = 0; i < matchTileTypes.Length; i++)
			{
				Create(matchTileTypes[i].ToString(), dir, 10);	
			}
			
			MatchTileObstacleType[] obstacleType = (MatchTileObstacleType[]) Enum.GetValues(typeof(MatchTileObstacleType));
			for (int i = 0; i < obstacleType.Length; i++)
			{
				Create(obstacleType[i].ToString(), dir, 10);	
			}

			MatchTileSpecialType[] specialType = (MatchTileSpecialType[]) Enum.GetValues(typeof(MatchTileSpecialType));
			for (int i = 0; i < specialType.Length; i++)
			{
				Create(specialType[i].ToString(), dir, 5);	
			}
		}

		private void Create(string type, string dir, int count = 5)
		{
			ObjectPoolEntry objectPoolEntry = new ObjectPoolEntry();
			GameObject obj = Resources.Load(dir + type) as GameObject;

			if (obj != null)
			{
				objectPoolEntry.Prefab = obj;
				objectPoolEntry.Count = count;
				objectPoolEntry.name = obj.name;

				objectPoolModel.AddObjectPoolEntry(objectPoolEntry);
			}
		}
	}
}