using System;
using UnityEngine;

namespace Touched
{
	public class MouseService : ITouchService
	{
		public void CheckForTouch()
		{
			if (Input.GetKey (KeyCode.Mouse0))
			{
				Messenger.Broadcast(TouchMessage.CALCULATE_TOUCH, Input.mousePosition);
			}

			if (Input.GetMouseButtonUp (0)) 
			{
				Messenger.Broadcast(TouchMessage.TOUCH_ENDED);
			}
		}

	}
}