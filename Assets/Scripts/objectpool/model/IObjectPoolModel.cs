using System;
using UnityEngine;

namespace ObjectPool
{
	public interface IObjectPoolModel
	{
		void AddObjectPoolEntry(ObjectPoolEntry objectPoolEntry);
		void BuildObjectPool();
		GameObject GetObjectForType(string objectType, bool onlyPooled = false);
		void PoolObject(GameObject obj, string objName = null);
	}
}