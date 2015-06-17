using System;
using UnityEngine;

namespace Touched
{
	public class TouchScreenService : ITouchService
	{
		public void CheckForTouch()
		{
			if (Input.touchCount > 0)
			{
				Touch touch = Input.GetTouch(0);
				Messenger.Broadcast(TouchMessage.CALCULATE_TOUCH, touch.position);

				if (touch.phase == TouchPhase.Ended)
				{
					Messenger.Broadcast(TouchMessage.TOUCH_ENDED);
				}
			}
		}

	}
}