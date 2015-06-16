using UnityEngine;
using IoC;

namespace MatchTileGrid
{
	public class MatchTileFactory : IMatchTileFactory, IInitialize
	{
		[Inject]
		public IContainer container { private get; set; }

		private CreateMatchTile createMatchTile;
		private CreateRandomMatchTile createRandomMatchTile;

		private RemoveMatchTile removeMatchTile;

		public void OnInject()
		{	
			container.Bind<CreateMatchTile>().AsSingle();
			createMatchTile = container.Build<CreateMatchTile>();

			container.Bind<CreateRandomMatchTile> ().AsSingle ();
			createRandomMatchTile = container.Build<CreateRandomMatchTile> ();

			container.Bind<RemoveMatchTile> ().AsSingle ();
			removeMatchTile = container.Build<RemoveMatchTile> ();
		}

		public MatchTile CreateMatchTile(MatchTileType type, Vector2 position)
		{	
			return createMatchTile.Create (type, position);
		}

		public MatchTile CreateRandomMatchTile(Vector2 position)
		{		
			return createRandomMatchTile.Create (position);
		}

		public void RemoveMatchTile(MatchTile matchTile)
		{
			removeMatchTile.Remove (matchTile);
		}
	}
}