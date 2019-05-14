using System;
using UnityEngine;
using Command;
using IoC;
using EventDispatcher;

namespace Touched
{
	public class CalculateTouchCommand : ICommand
	{
		[Inject]
		public IEventDispatcher eventDispatcher { private get; set; }

		public Vector3 touchedPosition { private get; set; }

		public void Execute()
		{
			Camera camera = Camera.main.GetComponent<Camera>();
			Ray ray = camera.ScreenPointToRay(touchedPosition);
			TouchedObject touched = DetectTouch2D (ray);

			if (!touched.Equals(default(TouchedObject)))
			{
				eventDispatcher.Broadcast(TouchMessage.OBJECT_TOUCHED_2D, touched);
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