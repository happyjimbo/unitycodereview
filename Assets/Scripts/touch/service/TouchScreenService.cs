using System;
using UnityEngine;
using EventDispatcher;
using IoC;

namespace Touched
{
	public class TouchScreenService : ITouchService
	{
		[Inject]
		public IEventDispatcher eventDispatcher { private get; set; }

		public void CheckForTouch()
		{
			if (Input.touchCount > 0)
			{
				Touch touch = Input.GetTouch(0);
				eventDispatcher.Broadcast(TouchMessage.CALCULATE_TOUCH, touch.position);

				if (touch.phase == TouchPhase.Ended)
				{
					eventDispatcher.Broadcast(TouchMessage.TOUCH_ENDED);
				}
			}
		}

	}
}