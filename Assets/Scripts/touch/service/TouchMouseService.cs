using System;
using UnityEngine;

namespace Touched
{
	public class TouchMouseService : ITouchService
	{
		public Action<Vector3> calculateTouch { get; set; }

		public void CheckForTouch()
		{
			if (Input.GetKey (KeyCode.Mouse0))
			{
				calculateTouch (Input.mousePosition);
			}

			if (Input.GetMouseButtonUp (0)) 
			{
				Messenger.Broadcast(TouchMessage.TOUCH_ENDED);
			}
		}

	}
}