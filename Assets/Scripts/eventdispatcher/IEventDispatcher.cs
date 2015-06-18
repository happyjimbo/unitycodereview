using System;

namespace EventDispatcher
{
	public interface IEventDispatcher
	{
		void AddListener(string eventType, Callback handler);
		void AddListener<T>(string eventType, Callback<T> handler);
		void AddListener<T, U>(string eventType, Callback<T, U> handler);
		void AddListener<T, U, V>(string eventType, Callback<T, U, V> handler);

		void RemoveListener(string eventType, Callback handler);
		void RemoveListener<T>(string eventType, Callback<T> handler);
		void RemoveListener<T, U>(string eventType, Callback<T, U> handler);
		void RemoveListener<T, U, V>(string eventType, Callback<T, U, V> handler);

		void Broadcast(string eventType);
		void Broadcast<T>(string eventType, T arg1);
		void Broadcast<T, U>(string eventType, T arg1, U arg2);
		void Broadcast<T, U, V>(string eventType, T arg1, U arg2, V arg3);

		void CleanAndDestroy();
	}
}