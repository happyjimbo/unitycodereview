using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using NSubstitute;
using UnityEngine;
using Touched;

// Using the Given When Then approach:
// http://martinfowler.com/bliki/GivenWhenThen.html
namespace MatchTileGrid
{
	[TestFixture]
	internal class CalculateTouchCommandTest
	{
		private CalculateTouchCommand calculateTouchCommand;

		[SetUp]
		public void SetUp()
		{
			calculateTouchCommand = new CalculateTouchCommand ();

			Messenger.CleanAndDestroy();
		}

		[Test]
		public void GivenGameObjectWithColliderOnScreen_WhenExecute_ThenOBjectTouched()
		{
			GameObject gameObject = new GameObject ();
			gameObject.transform.localScale = new Vector3 (2, 2, 2);

			BoxCollider2D collider = gameObject.AddComponent<BoxCollider2D> ();
			collider.center = Vector3.zero;
			collider.size = new Vector2 (2, 2);

			Messenger.AddListener <TouchedObject> (TouchMessage.OBJECT_TOUCHED_2D, (TouchedObject touched) =>
			{
				GameObject.DestroyImmediate(gameObject);
				Assert.Pass();
			});

			calculateTouchCommand.touchedPosition = Vector3.zero;
			calculateTouchCommand.Execute ();

			Assert.Fail ();
		}

		[Test]
		public void GivenNoObjectOnScreen_WhenExecute_ThenNoActionTaken()
		{
			Messenger.AddListener <TouchedObject> (TouchMessage.OBJECT_TOUCHED_2D, (TouchedObject touched) =>
			{
				Assert.Fail();
			});

			calculateTouchCommand.touchedPosition = Vector3.zero;
			calculateTouchCommand.Execute ();

			Assert.Pass ();
		}
	}
}