using UnityEngine;
using System.Collections;
using DG.Tweening;
using IoC;
using Command;
using MatchTileGrid;
using ObjectPool;
using Touched;
using EventDispatcher;

public class Main : IContextRoot
{
	public IContainer container { get; private set; }
	private TickEngine tickEngine;
	
	public Main()
	{
		Resources.UnloadUnusedAssets();

		DOTween.Init (true, false);
				
		SetupContainer();
		SetUpEventDispatcher ();
		CreateBindings ();
		Build();
	}
	
	private void SetupContainer()
	{
		container = new UnityContainer();

		container.Bind<TickEngine>().AsSingle();
		tickEngine = container.Build<TickEngine>();

		container.Bind<ICommandFactory>().AsSingle<CommandFactory>();
	}

	private void SetUpEventDispatcher()
	{
		container.Bind<IEventDispatcher>().AsSingle<EventDispatcher.EventDispatcher>();
		container.Build<IEventDispatcher> ();
	}

	private void CreateBindings()
	{
		BindServices();
		BindModels();
		//BindPresentations();
		BindControllers();
		BindFactories();
	}

	private void BindServices()
	{	
		#if UNITY_EDITOR
			container.Bind<ITouchService> ().AsSingle<MouseService>();
		#elif UNITY_IPHONE || UNITY_ANDROID || UNITY_WP8
			container.Bind<ITouchService> ().AsSingle<TouchScreenService>();
		#endif
	}

	private void BindModels()
	{	
		container.Bind<IObstacleTilesModel>().AsSingle<ObstacleTilesModel>();
		container.Bind<ISpecialTilesModel> ().AsSingle <SpecialTilesModel>();
		container.Bind<IMatchTileGridModel> ().AsSingle <MatchTileGridModel>();
		container.Bind<IObjectPoolModel>().AsSingle<ObjectPoolModel>();
	}

	/*private void BindPresentations()
	{		
	}*/

	private void BindControllers()
	{
		container.Bind<MatchTileGridController> ().AsSingle ();
		container.Bind<SpecialTileController> ().AsSingle ();
		container.Bind<TouchController> ().AsSingle ();
        container.Bind<GameObjectPoolController>().AsSingle();
	}

	private void BindFactories()
	{
		container.Bind<IMatchTileFactory> ().AsSingle <MatchTileFactory>();
	}

	private void Build()
	{			
		container.Build<ICommandFactory>();

		BuildServices();
		//BuildPresentations();
		BuildControllers();
		BuildFactory();
	}
	
	private void BuildServices()
	{
		container.Build<ITouchService> ();
	}

	/*private void BuildPresentations()
	{	
	}*/

	private void BuildControllers()
	{			
		container.Build<MatchTileGridController> ();
		container.Build<SpecialTileController> ();
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
