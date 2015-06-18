using System;
using System.Collections.Generic;
using UnityEngine;
using IoC;
using Command;
using ObjectPool;
using Touched;
using EventDispatcher;

namespace MatchTileGrid
{
	public class MatchTileGridController : IInitialize
	{
		[Inject]
		public ICommandFactory commandFactory { private get; set; }

		[Inject]
		public IEventDispatcher eventDispatcher { private get; set; }

		private MatchTileTouchedCommand matchTileTouchedCommand;
		private CreateNewTileCommand createNewTileCommand;
		private HideInvalidTilesCommand hideInvalidTilesCommand;
		private RemoveMatchTilesCommand removeMatchTilesCommand;

		public void OnInject ()
		{
			BuildCommands ();
		}

		private void BuildCommands()
		{	
			ICommand loadMatchTileGridDataCommand = commandFactory.Build<LoadMatchTileGridDataCommand> ();
			loadMatchTileGridDataCommand.Execute ();

			ICommand createMatchTileGridHolderCommand = commandFactory.Build<CreateMatchTileGridHolderCommand> ();
			eventDispatcher.AddListener (ObjectPoolMessage.OBJECT_POOL_COMPLETE, createMatchTileGridHolderCommand.Execute);

			ICommand createMatchTileGridCommand = commandFactory.Build<CreateMatchTileGridCommand> ();
			eventDispatcher.AddListener (MatchTileGridMessage.GRID_HOLDER_CREATED, createMatchTileGridCommand.Execute);

			matchTileTouchedCommand = commandFactory.Build<MatchTileTouchedCommand> ();
			eventDispatcher.AddListener <TouchedObject> (TouchMessage.OBJECT_TOUCHED_2D, MatchTileTouched);

			ICommand removeMatchTileHightLightCommand = commandFactory.Build<RemoveMatchTileHightLightCommand> ();
			eventDispatcher.AddListener (TouchMessage.TOUCH_ENDED, removeMatchTileHightLightCommand.Execute);

			ICommand calculateTilesToRemove = commandFactory.Build<CalculateTilesToRemoveCommand> ();
			eventDispatcher.AddListener (TouchMessage.TOUCH_ENDED, calculateTilesToRemove.Execute);

			ICommand allowFallingTilesCommand = commandFactory.Build<AllowFallingTilesCommand> ();
			eventDispatcher.AddListener (MatchTileGridMessage.ALLOW_FALLING_TILES, allowFallingTilesCommand.Execute);

			createNewTileCommand = commandFactory.Build<CreateNewTileCommand> ();
			eventDispatcher.AddListener (MatchTileGridMessage.CREATE_NEW_TILE, CreateNewTile);

			ICommand checkMovesRemainingCommand = commandFactory.Build<CheckMovesRemainingCommand> ();
			eventDispatcher.AddListener (MatchTileGridMessage.CHECK_MOVES_REMAINING, checkMovesRemainingCommand.Execute);

			ICommand shuffleGridCommand = commandFactory.Build<ShuffleGridCommand> ();
			eventDispatcher.AddListener (MatchTileGridMessage.SHUFFLE_GRID, shuffleGridCommand.Execute);

			hideInvalidTilesCommand = commandFactory.Build<HideInvalidTilesCommand> ();
			eventDispatcher.AddListener <MatchTileType> (MatchTileGridMessage.HIDE_INVALID_TILES, HideInvalidTiles);
			eventDispatcher.AddListener (TouchMessage.TOUCH_ENDED, MatchTileShowTiles);

			removeMatchTilesCommand = commandFactory.Build<RemoveMatchTilesCommand> ();
			eventDispatcher.AddListener <RemoveTile> (MatchTileGridMessage.REMOVE_TILES, RemoveTiles);

			ICommand positionTilesCommand = commandFactory.Build<PositionTilesCommand> ();
			positionTilesCommand.Execute ();

			ICommand matchTileHintCommand = commandFactory.Build<MatchTileHintCommand> ();
			matchTileHintCommand.Execute ();
		}

		private void MatchTileTouched(TouchedObject touchedObject)
		{
			matchTileTouchedCommand.touchedObject = touchedObject;
			matchTileTouchedCommand.Execute ();
		}

		private void CreateNewTile()
		{
			createNewTileCommand.Execute ();
		}

		private void HideInvalidTiles(MatchTileType type)
		{
			hideInvalidTilesCommand.type = type;
			hideInvalidTilesCommand.hideType = HideType.NotValidTiles;
			hideInvalidTilesCommand.Execute ();
		}

		private void MatchTileShowTiles()
		{
			hideInvalidTilesCommand.hideType = HideType.ShowAll;
			hideInvalidTilesCommand.Execute ();
		}

		private void RemoveTiles(RemoveTile removeTile)
		{
			removeMatchTilesCommand.removeTile = removeTile;
			removeMatchTilesCommand.Execute ();
		}
	}
}