using System;
using UnityEngine;
using System.Collections;
using IoC;

namespace Touched
{
	public struct TouchedObject
	{
	    public GameObject objectHit;
	    public Vector3 point;
	}

	public class TouchController : ITickable, IInitialize, IDisposable
	{	
		[Inject]
		public ITouchService touchService { private get; set; }

		public void OnInject()
		{
			touchService.calculateTouch += CalculateTouch;
		}

		public void Tick(float delta)
		{					
			touchService.CheckForTouch ();
		}

		private void CalculateTouch(Vector3 pos)
		{
			Camera camera = Camera.main.camera;
			Ray ray = camera.ScreenPointToRay(pos);
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

		public void Dispose()
		{
			touchService.calculateTouch -= CalculateTouch;
		}
	}
}