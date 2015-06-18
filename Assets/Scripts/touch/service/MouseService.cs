using System;
using UnityEngine;
using IoC;
using EventDispatcher;

namespace Touched
{
	public class MouseService : ITouchService
	{
		[Inject]
		public IEventDispatcher eventDispatcher { private get; set; }

		public void CheckForTouch()
		{
			if (Input.GetKey (KeyCode.Mouse0))
			{
				eventDispatcher.Broadcast(TouchMessage.CALCULATE_TOUCH, Input.mousePosition);
			}

			if (Input.GetMouseButtonUp (0)) 
			{
				eventDispatcher.Broadcast(TouchMessage.TOUCH_ENDED);
			}
		}

	}
}