using System;

namespace EventDispatcher
{
	/// <summary>
	/// This wrapper allow us to inject our IEventDispatcher into our classes
	/// without them knowing about the Messenger implementation (which is a 3rd part library).
	/// Going forwards I'd be interested in looking at using a Signals solution
	/// instead of this, where we're not passing magic strings around the code base and instead
	/// we're using a strongly typed soltion instead..
	/// </summary>
	public class EventDispatcher : IEventDispatcher
	{
		public void AddListener(string eventType, Callback handler) 
		{
			Messenger.AddListener (eventType, handler);
		}

		public void AddListener<T>(string eventType, Callback<T> handler) 
		{
			Messenger.AddListener <T> (eventType, handler);
		}

		public void AddListener<T, U>(string eventType, Callback<T, U> handler) 
		{
			Messenger.AddListener <T, U> (eventType, handler);
		}

		public void AddListener<T, U, V>(string eventType, Callback<T, U, V> handler) 
		{
			Messenger.AddListener <T, U, V> (eventType, handler);
		}

		public void RemoveListener(string eventType, Callback handler) 
		{
			Messenger.RemoveListener (eventType, handler);
		}

		public void RemoveListener<T>(string eventType, Callback<T> handler) 
		{
			Messenger.RemoveListener <T> (eventType, handler);
		}

		public void RemoveListener<T, U>(string eventType, Callback<T, U> handler) 
		{
			Messenger.RemoveListener <T, U> (eventType, handler);
		}

		public void RemoveListener<T, U, V>(string eventType, Callback<T, U, V> handler) 
		{
			Messenger.RemoveListener <T, U, V> (eventType, handler);	
		}

		public void Broadcast(string eventType) 
		{
			Messenger.Broadcast (eventType);
		}

		public void Broadcast<T>(string eventType, T arg1)
		{
			Messenger.Broadcast <T> (eventType, arg1);
		}

		public void Broadcast<T, U>(string eventType, T arg1, U arg2) 
		{
			Messenger.Broadcast <T, U> (eventType, arg1, arg2);
		}

		public void Broadcast<T, U, V>(string eventType, T arg1, U arg2, V arg3)
		{
			Messenger.Broadcast <T, U, V> (eventType, arg1, arg2, arg3);
		}

		public void CleanAndDestroy()
		{
			Messenger.CleanAndDestroy ();
		}
	}
}