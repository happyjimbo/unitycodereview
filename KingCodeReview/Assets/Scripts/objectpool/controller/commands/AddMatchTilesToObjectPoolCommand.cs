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