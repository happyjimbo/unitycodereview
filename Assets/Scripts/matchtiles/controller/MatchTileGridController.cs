using System;
using System.Collections.Generic;
using UnityEngine;
using IoC;
using Command;
using ObjectPool;
using Touched;

namespace MatchTileGrid
{
	public class MatchTileGridController : IInitialize
	{
		[Inject]
		public ICommandFactory commandFactory { private get; set; }

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
			Messenger.AddListener (ObjectPoolMessage.OBJECT_POOL_COMPLETE, createMatchTileGridHolderCommand.Execute);

			ICommand createMatchTileGridCommand = commandFactory.Build<CreateMatchTileGridCommand> ();
			Messenger.AddListener (MatchTileGridMessage.GRID_HOLDER_CREATED, createMatchTileGridCommand.Execute);

			matchTileTouchedCommand = commandFactory.Build<MatchTileTouchedCommand> ();
			Messenger.AddListener <TouchedObject> (TouchMessage.OBJECT_TOUCHED_2D, MatchTileTouched);

			ICommand removeMatchTileHightLightCommand = commandFactory.Build<RemoveMatchTileHightLightCommand> ();
			Messenger.AddListener (TouchMessage.TOUCH_ENDED, removeMatchTileHightLightCommand.Execute);

			ICommand calculateTilesToRemove = commandFactory.Build<CalculateTilesToRemoveCommand> ();
			Messenger.AddListener (TouchMessage.TOUCH_ENDED, calculateTilesToRemove.Execute);

			ICommand allowFallingTilesCommand = commandFactory.Build<AllowFallingTilesCommand> ();
			Messenger.AddListener (MatchTileGridMessage.ALLOW_FALLING_TILES, allowFallingTilesCommand.Execute);

			createNewTileCommand = commandFactory.Build<CreateNewTileCommand> ();
			Messenger.AddListener (MatchTileGridMessage.CREATE_NEW_TILE, CreateNewTile);

			ICommand checkMovesRemainingCommand = commandFactory.Build<CheckMovesRemainingCommand> ();
			Messenger.AddListener (MatchTileGridMessage.CHECK_MOVES_REMAINING, checkMovesRemainingCommand.Execute);

			ICommand shuffleGridCommand = commandFactory.Build<ShuffleGridCommand> ();
			Messenger.AddListener (MatchTileGridMessage.SHUFFLE_GRID, shuffleGridCommand.Execute);

			ICommand addScoreForTilesCommand = commandFactory.Build<AddScoreForTilesCommand> ();
			Messenger.AddListener (TouchMessage.TOUCH_ENDED, addScoreForTilesCommand.Execute);

			hideInvalidTilesCommand = commandFactory.Build<HideInvalidTilesCommand> ();
			Messenger.AddListener <MatchTileType> (MatchTileGridMessage.HIDE_INVALID_TILES, HideInvalidTiles);
			Messenger.AddListener (TouchMessage.TOUCH_ENDED, MatchTileShowTiles);

			removeMatchTilesCommand = commandFactory.Build<RemoveMatchTilesCommand> ();
			Messenger.AddListener <RemoveTile> (MatchTileGridMessage.REMOVE_TILES, RemoveTiles);

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