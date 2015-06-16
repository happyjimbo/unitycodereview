using UnityEngine;
using System.Collections;
using IoC;

namespace MatchTileGrid
{
	public class MatchTileGridMediator : MonoBehaviour 
	{	
		[Inject]
		public IMatchTileGridModel model {private get; set;}

		public GameObject gridHolder;

		public void Start () 
		{ 
			this.Inject();

			model.gridHolder = gridHolder;

			GameObject.Destroy(this);
		}
	}
}