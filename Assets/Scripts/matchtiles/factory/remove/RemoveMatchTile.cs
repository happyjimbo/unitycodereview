using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IoC;
using DG.Tweening;
using ObjectPool;
using EventDispatcher;

namespace MatchTileGrid
{
	public class RemoveMatchTile  
	{
		[Inject]
		public IMatchTileGridModel matchTileGridModel { private get; set; }

		[Inject]
		public IObjectPoolModel objectPoolModel { private get; set; }

		public void Remove(MatchTile matchTile)
		{				
			matchTile.tileObject.transform.DOKill ();
			matchTile.tileObject.transform.DOScale (GetTileScale (), 0.3f).OnComplete (() => PoolObject (matchTile));

			matchTileGridModel.RemoveTile (matchTile.position);
		}

		private void PoolObject(MatchTile matchTile)
		{
			objectPoolModel.PoolObject (matchTile.tileObject);
		}

		private Vector3 GetTileScale()
		{
			float scale = 0.4f;
			return new Vector3(scale, scale, scale);
		}
	}
}