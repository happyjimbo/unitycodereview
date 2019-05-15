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
		private CreateObstacle createObstacle;
		private CreateSpecialTile createSpecialTile;

		private RemoveMatchTile removeMatchTile;
		private RemoveObstacle removeObstacle;

		public void OnInject()
		{	
			container.Bind<CreateMatchTile>().AsSingle();
			createMatchTile = container.Build<CreateMatchTile>();

			container.Bind<CreateRandomMatchTile> ().AsSingle ();
			createRandomMatchTile = container.Build<CreateRandomMatchTile> ();

			container.Bind<CreateObstacle> ().AsSingle ();
			createObstacle = container.Build<CreateObstacle> ();

			container.Bind<CreateSpecialTile> ().AsSingle ();
			createSpecialTile = container.Build<CreateSpecialTile> ();				

			container.Bind<RemoveMatchTile> ().AsSingle ();
			removeMatchTile = container.Build<RemoveMatchTile> ();

			container.Bind<RemoveObstacle> ().AsSingle ();
			removeObstacle = container.Build<RemoveObstacle> ();
		}

		public MatchTile CreateMatchTile(MatchTileType matchTile, MatchTileObstacleType obstacleTile, Vector2 position)
		{	
			CreateObstacle (obstacleTile, position);
			
			if (obstacleTile != MatchTileObstacleType.Null && matchTile == MatchTileType.Null)
			{
				switch (obstacleTile)
				{
					case MatchTileObstacleType.MatchToken_Sticker:
					case MatchTileObstacleType.MatchToken_Blocker_1:
					case MatchTileObstacleType.MatchToken_Blocker_2:
					case MatchTileObstacleType.MatchToken_Blocker_3:
					case MatchTileObstacleType.MatchToken_Locker_1:
					case MatchTileObstacleType.MatchToken_Locker_2:
					case MatchTileObstacleType.MatchToken_Locker_3:
					case MatchTileObstacleType.MatchToken_Trapper:
					case MatchTileObstacleType.MatchToken_Regenerator:
					case MatchTileObstacleType.MatchToken_Key:
						return CreateRandomMatchTile (position, obstacleTile);
				}
			}

			return createMatchTile.Create (matchTile, position);	
		}

		public MatchTile CreateRandomMatchTile(Vector2 position, MatchTileObstacleType obstacleType)
		{		
			return createRandomMatchTile.Create (position, obstacleType);
		}

		public ObstacleTile CreateObstacle(MatchTileObstacleType obstacle, Vector2 position)
		{
			return createObstacle.Create (obstacle, position);
		}

		public void RemoveMatchTile(MatchTile matchTile)
		{
			removeMatchTile.Remove (matchTile);
		}

		public void RemoveObstacle(ObstacleTile obstacleTile)
		{
			removeObstacle.Remove (obstacleTile);	
		}

		public void CreateSpecialTile(Vector2 position, MatchTileSpecialType specialType)
		{
			createSpecialTile.Create (position, specialType);
		}
	}
}