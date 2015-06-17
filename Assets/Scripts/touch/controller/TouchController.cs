using System;
using UnityEngine;
using System.Collections;
using IoC;
using Command;

namespace Touched
{
	public struct TouchedObject
	{
	    public GameObject objectHit;
	    public Vector3 point;
	}

	public class TouchController : IInitialize, ITickable
	{	
		[Inject]
		public ICommandFactory commandFactory { private get; set; }

		[Inject]
		public ITouchService touchService { private get; set; }

		private CalculateTouchCommand calculateTouchCommand;

		public void OnInject()
		{			
			calculateTouchCommand = commandFactory.Build<CalculateTouchCommand> ();
			Messenger.AddListener <Vector3> (TouchMessage.CALCULATE_TOUCH, CalculateTouch);
		}

		private void CalculateTouch(Vector3 touchedPosition)
		{
			calculateTouchCommand.touchedPosition = touchedPosition;
			calculateTouchCommand.Execute ();
		}

		public void Tick(float delta)
		{					
			touchService.CheckForTouch ();
		}
	}
}