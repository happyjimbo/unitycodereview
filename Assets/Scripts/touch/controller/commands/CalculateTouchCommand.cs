using System;
using UnityEngine;
using Command;
using IoC;

namespace Touched
{
	public class CalculateTouchCommand : ICommand
	{
		public Vector3 touchedPosition { private get; set; }

		public void Execute()
		{
			Camera camera = Camera.main.camera;
			Ray ray = camera.ScreenPointToRay(touchedPosition);
			TouchedObject touched = DetectTouch2D (ray);

			if (!touched.Equals(default(TouchedObject)))
			{
				Messenger.Broadcast(TouchMessage.OBJECT_TOUCHED_2D, touched);
			}	
		}

		private TouchedObject DetectTouch2D(Ray ray)
		{
			TouchedObject touched = new TouchedObject();

			RaycastHit2D hit2d = Physics2D.Raycast(ray.origin, ray.direction);
			if (hit2d.collider != null)
			{			
				touched.objectHit = hit2d.transform.gameObject;
				touched.point = hit2d.point;
			}
			return touched;
		}
	}
}