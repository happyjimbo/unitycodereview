using System;
using IoC;
using Command;

namespace ObjectPool
{
	public class GameObjectPoolController : IInitialize
	{
		[Inject]
		public ICommandFactory commandFactory {private get; set;}

		[Inject]
		public IObjectPoolModel objectPoolModel {private get; set;}

		public void OnInject()
		{
			BuildCommands();
		}

		private void BuildCommands()
		{
			ICommand addMatchTilesToObjectPoolCommand = commandFactory.Build<AddMatchTilesToObjectPoolCommand>();
			addMatchTilesToObjectPoolCommand.Execute ();

			objectPoolModel.BuildObjectPool();
		}
	}	
}