using UnityEngine;
using System.Collections;
using DG.Tweening;
using IoC;
using Command;
using MatchTileGrid;
using ObjectPool;
using Touched;

public class Main : IContextRoot
{
	public IContainer container { get; private set; }
	private TickEngine tickEngine;
	
	public Main()
	{
		Messenger.Cleanup ();

		Resources.UnloadUnusedAssets();

		DOTween.Init (true, false);
				
		SetupContainer();
		StartGame();
	}
	
	private void SetupContainer()
	{
		container = new UnityContainer();

		// Set up the tick engine
		container.Bind<TickEngine>().AsSingle();
		tickEngine = container.Build<TickEngine>();

		container.Bind<ICommandFactory>().AsSingle<CommandFactory>();
		
		SetUpServices();
		SetUpModels();
		SetUpPresentations();
		SetUpControllers();
		SetUpFactory();
	}
	
	private void SetUpServices()
	{		
		#if UNITY_EDITOR
		container.Bind<ITouchService> ().AsSingle<TouchMouseService>();
		#endif

		#if UNITY_IPHONE || UNITY_ANDROID || UNITY_WP8
		container.Bind<ITouchService> ().AsSingle<TouchScreenService>();
		#endif
	}

	private void SetUpModels()
	{	
		container.Bind<IMatchTileGridModel> ().AsSingle <MatchTileGridModel>();
		container.Bind<IObjectPoolModel>().AsSingle<ObjectPoolModel>();
	}

	private void SetUpPresentations()
	{		
	}

	private void SetUpControllers()
	{
		container.Bind<MatchTileGridController> ().AsSingle ();
		container.Bind<TouchController> ().AsSingle ();
        container.Bind<GameObjectPoolController>().AsSingle();
	}

	private void SetUpFactory()
	{
		container.Bind<IMatchTileFactory> ().AsSingle <MatchTileFactory>();
	}

	private void StartGame()
	{			
		container.Build<ICommandFactory>();

		BuildServices();
		BuildPresentations();
		BuildControllers();
		BuildFactory();
	}
	
	private void BuildServices()
	{
		container.Build<ITouchService> ();
	}

	private void BuildPresentations()
	{	
	}

	private void BuildControllers()
	{			
		container.Build<MatchTileGridController> ();
		container.Build<GameObjectPoolController>();
			
		TouchController touchController = container.Build<TouchController>();
		tickEngine.Add(touchController);
	}

	private void BuildFactory()
	{
		container.Build<IMatchTileFactory> ();
	}

}

//UnityContext must be executed before 
//anything else that uses the container itself.
//In order to achieve this, you can use
//the execution order or the awake/start 
//functions order

public class GameContext : UnityContext<Main>
{
	
}
