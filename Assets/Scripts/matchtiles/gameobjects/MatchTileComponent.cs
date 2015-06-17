using System;
using UnityEngine;

namespace MatchTileGrid
{
	public class MatchTileComponent : MonoBehaviour, IMatchTileComponent
	{
		public GameObject tile;
		public GameObject highlight;

		public void OnEnable()
		{
			Tile ();
		}

		public void Tile()
		{
			tile.SetActive (true);
			if (highlight != null)
			{
				highlight.SetActive (false);		
			}
		}

		public void HighLight()
		{
			tile.SetActive (false);
			if (highlight != null)
			{
				highlight.SetActive (true);		
			}
		}

		public void Show()
		{
			SpriteRenderer spriteRenderer = tile.GetComponent<SpriteRenderer> ();
			Color currentColor = spriteRenderer.color;
			spriteRenderer.color = new Color (currentColor.r, currentColor.g, currentColor.b, 1);
		}

		public void Hide()
		{
			SpriteRenderer spriteRenderer = tile.GetComponent<SpriteRenderer> ();
			Color currentColor = spriteRenderer.color;
			spriteRenderer.color = new Color (currentColor.r, currentColor.g, currentColor.b, 0.5f);
		}
	}
}