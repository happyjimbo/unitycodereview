using System;
using UnityEngine;

namespace Touched
{
	public class TouchScreenService : ITouchService
	{
		public Action<Vector3> calculateTouch { get; set; }

		public void CheckForTouch()
		{
			if (Input.touchCount > 0)
			{
				Touch touch = Input.GetTouch(0);
				calculateTouch (touch.position);

				if (touch.phase == TouchPhase.Ended)
				{
					Messenger.Broadcast(TouchMessage.TOUCH_ENDED);
				}
			}
		}

	}
}