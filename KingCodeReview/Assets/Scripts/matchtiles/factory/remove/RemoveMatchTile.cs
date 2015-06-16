using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IoC;
using DG.Tweening;
using ObjectPool;

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

			Vector3 endPos = new Vector3(4, 4, 0);

			matchTile.tileObject.transform.DOScale (GetTileScale(), 0.3f);
			matchTile.tileObject.transform.DOLocalMove (endPos, 0.5f).OnComplete( () => PoolObject(matchTile) );

			matchTileGridModel.RemoveTile (matchTile.position);
		}

		private void PoolObject(MatchTile matchTile)
		{
			objectPoolModel.PoolObject (matchTile.tileObject);	
			Messenger.Broadcast (MatchTileGridMessage.ALLOW_FALLING_TILES);
		}

		private Vector3 GetTileScale()
		{
			float scale = 0.4f;
			return new Vector3(scale, scale, scale);
		}
	}
}