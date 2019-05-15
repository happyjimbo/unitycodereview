using System;
using UnityEngine;
using MatchTileGrid;
using UnityEngine.UIElements;

public class TrapperComponent : MatchTileComponent
{
	public GameObject counter;

	private Label counterLabel;
	private ObstacleTile obstacleTile;

	public void Start()
	{
		//counterLabel = counter.GetComponent<Label> ();
	}

	public void SetObstacleTile(ObstacleTile obstacleTile)
	{
		this.obstacleTile = obstacleTile;

		UpdateCounter ();
	}

	public void UpdateCounter()
	{
		int amount = obstacleTile.counter;
		amount = amount < 0 ? 0 : amount;
		//counterLabel.text = amount.ToString ();
	}
}