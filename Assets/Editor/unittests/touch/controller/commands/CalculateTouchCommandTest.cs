using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using NSubstitute;
using UnityEngine;
using Touched;
using EventDispatcher;

// Using the Given When Then approach:
// http://martinfowler.com/bliki/GivenWhenThen.html
namespace Touched
{
	[TestFixture]
	internal class CalculateTouchCommandTest
	{
		private CalculateTouchCommand calculateTouchCommand;
		private IEventDispatcher eventDispatcher;

		[SetUp]
		public void SetUp()
		{
			eventDispatcher = Substitute.For<IEventDispatcher> ();

			calculateTouchCommand = new CalculateTouchCommand ();
			calculateTouchCommand.eventDispatcher = eventDispatcher;

			eventDispatcher.CleanAndDestroy();
		}

		[Test]
		public void GivenGameObjectWithColliderOnScreen_WhenExecute_ThenOBjectTouched()
		{
			GameObject gameObject = new GameObject ();
			gameObject.transform.localScale = new Vector3 (2, 2, 2);

			BoxCollider2D collider = gameObject.AddComponent<BoxCollider2D> ();
			collider.center = Vector3.zero;
			collider.size = new Vector2 (2, 2);

			calculateTouchCommand.touchedPosition = Vector3.zero;
			calculateTouchCommand.Execute ();

			eventDispatcher.Received ().Broadcast (TouchMessage.OBJECT_TOUCHED_2D, Arg.Any<TouchedObject> ());
			GameObject.DestroyImmediate(gameObject);
		}

		[Test]
		public void GivenNoObjectOnScreen_WhenExecute_ThenNoActionTaken()
		{
			calculateTouchCommand.touchedPosition = Vector3.zero;
			calculateTouchCommand.Execute ();

			eventDispatcher.DidNotReceive ().Broadcast (TouchMessage.OBJECT_TOUCHED_2D, Arg.Any<TouchedObject> ());
		}
	}
}