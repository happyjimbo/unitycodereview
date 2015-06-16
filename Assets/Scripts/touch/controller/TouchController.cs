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

	public class TouchController : ITickable
	{	
		private TouchedObject tapObject;

		public void Tick(float delta)
		{				
			TouchSelect ();			
						
			#if UNITY_EDITOR
			MouseSelect();
			#endif
		}

		private void TouchSelect()
		{
			if (Input.touchCount > 0)
			{
				Touch touch = Input.GetTouch(0);
				CalculateTouch (touch.position);

				if (touch.phase == TouchPhase.Ended)
				{
					Messenger.Broadcast(TouchMessage.TOUCH_ENDED);
					Messenger.Broadcast(TouchMessage.TAP_OBJECT, tapObject);
				}
			}
		}

		private void MouseSelect()
		{		
			if (Input.GetKey (KeyCode.Mouse0))
			{
				CalculateTouch (Input.mousePosition);
			}

			if (Input.GetMouseButtonUp (0)) 
			{
				Messenger.Broadcast(TouchMessage.TOUCH_ENDED);
				Messenger.Broadcast(TouchMessage.TAP_OBJECT, tapObject);
			}
		}

		private void CalculateTouch(Vector3 pos)
		{
			TouchedObject touched = default(TouchedObject);
			Ray ray;

			// 2d Camera
			//Camera camera = cameraModel.game2DCamera.GetComponent<Camera> ();
			Camera camera = Camera.main.camera;
			ray = camera.ScreenPointToRay(pos);
			touched = DetectTouch2D (ray);
			if (!touched.Equals(default(TouchedObject)))
			{
				tapObject = touched;
				Messenger.Broadcast(TouchMessage.OBJECT_TOUCHED_2D, touched);
				return;
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