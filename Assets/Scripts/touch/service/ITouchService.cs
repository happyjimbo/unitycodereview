using System;
using UnityEngine;

namespace Touched
{
	public interface ITouchService
	{
		Action<Vector3> calculateTouch { get; set; }
		void CheckForTouch();
	}
}

