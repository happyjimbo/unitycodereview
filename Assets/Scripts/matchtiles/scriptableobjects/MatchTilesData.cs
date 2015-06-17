using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace MatchTileGrid
{
	[Serializable]
	public class MatchTilesData : ScriptableObject
	{
		public Vector2 gridSize;
		public List<MatchTileData> matchTilesData;

		// Dictionary data is not serialised by unity, so we have to create this ourselves.
		private Dictionary<Vector2, MatchTileData> _matchTiles;
		public Dictionary<Vector2, MatchTileData> matchTiles 
		{ 
			get 
			{
				if (_matchTiles == null)
				{
					_matchTiles = new Dictionary<Vector2, MatchTileData> ();

					for (int i = 0; i < matchTilesData.Count; i++)
					{
						MatchTileData data = matchTilesData [i];
						_matchTiles [data.position] = data;
					}
				}

				return _matchTiles;
			}
		}
	}

	[Serializable]
	public struct MatchTileData
	{
		public Vector2 position;
		public GameObject gameObject;
	}
}