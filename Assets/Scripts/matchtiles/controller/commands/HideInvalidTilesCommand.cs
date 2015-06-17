using System;
using System.Collections.Generic;
using UnityEngine;
using IoC;
using Command;

namespace MatchTileGrid
{
	public enum HideType
	{
		NotValidTiles,
		ShowAll
	}

	public class HideInvalidTilesCommand : ICommand
	{
		[Inject]
		public IMatchTileGridModel matchTileGridModel { private get; set; }

		public MatchTileType type  { private get; set; }
		public HideType hideType  { private get; set; }

		public void Execute()
		{
			if (hideType == HideType.NotValidTiles)
			{
				HideNotValidTiles ();	
			} 
			else if (hideType == HideType.ShowAll && matchTileGridModel.allowTouch)
			{
				ShowAllTiles ();
			}
		}

		private void HideNotValidTiles()
		{
			List<MatchTile> matchTiles = matchTileGridModel.GetAllMatchTilesNotOfType (type);
			for (int i = 0; i < matchTiles.Count; i++)
			{
				MatchTile tile = matchTiles [i];
				IMatchTileComponent matchTileComponent = matchTileGridModel.GetMatchTileComponent(tile);
				if (matchTileComponent != null)
				{
					matchTileComponent.Hide ();
				}
			}
		}

		private void ShowAllTiles()
		{
			Dictionary<Vector2, MatchTile> matchTiles = matchTileGridModel.GetMatchTiles ();
			foreach(KeyValuePair<Vector2, MatchTile> entry in matchTiles)
			{
				MatchTile tile = entry.Value;
				IMatchTileComponent matchTileComponent = matchTileGridModel.GetMatchTileComponent(tile);
				if (matchTileComponent != null)
				{
					matchTileComponent.Show ();
				}
			}
		}
	}
}