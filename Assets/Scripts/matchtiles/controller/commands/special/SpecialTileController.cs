using UnityEngine;
using System.Collections.Generic;
using IoC;
using Command;
using EventDispatcher;
using Touched;

namespace MatchTileGrid
{
	public class SpecialTileController : IInitialize
	{
		[Inject] public ICommandFactory commandFactory { private get; set; }
		
		[Inject] public IEventDispatcher eventDispatcher { private get; set; }

		private SpecialHorizontalTileCommand specialHorizontalTile;
		private SpecialVerticleTileCommand specialVerticleTile;
		private SpecialMatchingTIleCommand specialMatchingTIle;
		private SpecialSurroundingTileCommand specialSurroundingTile;

		public void OnInject()
		{
			BuildCommands ();

			eventDispatcher.AddListener <List<MatchTileSpecialType>, MatchTile> (MatchTileGridMessage.SPECIAL_TILE, SpecialTiles);
		}

		private void BuildCommands()
		{
			ICommand setUpSpecialTileChargeCommand = commandFactory.Build<SetUpSpecialTileChargeCommand> ();
			setUpSpecialTileChargeCommand.Execute ();

			ICommand calculateSpecialTileChargeCommand = commandFactory.Build<CalculateSpecialTileChargeCommand> ();
			eventDispatcher.AddListener (TouchMessage.TOUCH_ENDED, calculateSpecialTileChargeCommand.Execute);

			specialHorizontalTile = commandFactory.Build<SpecialHorizontalTileCommand> ();
			specialVerticleTile = commandFactory.Build<SpecialVerticleTileCommand> ();
			specialMatchingTIle = commandFactory.Build<SpecialMatchingTIleCommand> ();
			specialSurroundingTile = commandFactory.Build<SpecialSurroundingTileCommand> ();
		}

		private void SpecialTiles(List<MatchTileSpecialType> tiles, MatchTile matchTile)
		{
			for (int i = 0; i < tiles.Count; i++)
			{
				SpecialTile (tiles [i], matchTile);
			}
		}

		private void SpecialTile(MatchTileSpecialType type, MatchTile matchTile)
		{
			switch (type)
			{
				case MatchTileSpecialType.Special_Horizontal:
					specialHorizontalTile.matchTile = matchTile;
					specialHorizontalTile.Execute ();
					break;

				case MatchTileSpecialType.Special_Vertical:
					specialVerticleTile.matchTile = matchTile;
					specialVerticleTile.Execute ();
					break;

				case MatchTileSpecialType.Special_Matching:
					specialMatchingTIle.matchTile = matchTile;
					specialMatchingTIle.Execute ();
					break;

				case MatchTileSpecialType.Special_Surrounding:
					specialSurroundingTile.matchTile = matchTile;
					specialSurroundingTile.Execute ();
					break;
			}
		}

	}
}