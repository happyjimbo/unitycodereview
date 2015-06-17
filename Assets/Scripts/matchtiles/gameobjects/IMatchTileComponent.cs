using System;
using UnityEngine;

namespace MatchTileGrid
{
	public interface IMatchTileComponent
	{
		void Tile();
		void HighLight();
		void Show();
		void Hide();
		void Hint(int delay);
	}
}