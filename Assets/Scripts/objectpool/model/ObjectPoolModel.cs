using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IoC;
using EventDispatcher;


namespace ObjectPool
{
	/// <summary>
	/// Please note that this was originally built from a 3rd party library,
	/// but has been customised
	/// </summary>
	public class ObjectPoolModel : IObjectPoolModel
	{
		[Inject]
		public IEventDispatcher eventDispatcher { private get; set; }

		/// <summary>
		/// The object prefabs which the pool can handle	
		/// by The amount of objects of each type to buffer.
		/// </summary>	
		//public ObjectPoolEntry[] Entries;
		private List<ObjectPoolEntry> Entries = new List<ObjectPoolEntry>();

		/// <summary>	
		/// The pooled objects currently available.
		/// Indexed by the index of the objectPrefabs
		/// </summary>
		private List<GameObject>[] Pool;

		/// <summary>	
		/// The container object that we will keep unused pooled objects so we dont clog up the editor with objects.	
		/// </summary>	
		private GameObject ContainerObject;

		public void AddObjectPoolEntry(ObjectPoolEntry objectPoolEntry)
		{
			Entries.Add(objectPoolEntry);
		}

		// Use this for initialization	
		public void BuildObjectPool()
		{
			ContainerObject = new GameObject("ObjectPoolContainer");

			//Loop through the object prefabs and make a new list for each one.		
			//We do this because the pool can only support prefabs set to it in the editor,		
			//so we can assume the lists of pooled objects are in the same order as object prefabs in the array		
			Pool = new List<GameObject>[Entries.Count];

			for (int i = 0; i < Entries.Count; i++)			
			{	
				Build(i);	
			}

			Coroutiner.StartCoroutine (CompleteObjectPool ());
		}

		private void Build(int i)
		{
			var objectPrefab = Entries[i];

			//create the repository			
			Pool[i] = new List<GameObject>();  
			
			GameObject[] objects = new GameObject[objectPrefab.Count];

			//fill it			
			int n = 0;
			for (n = 0; n < objectPrefab.Count; n++)				
			{
				var newObj = GameObject.Instantiate(objectPrefab.Prefab) as GameObject;
				newObj.name = objectPrefab.name;
				objects[n] = newObj;
				PoolObject(newObj, objectPrefab.name);
			}	
					
			Coroutiner.StartCoroutine ( PreloadObjectPoolTextures(objects) );
		}
		
		private IEnumerator PreloadObjectPoolTextures(GameObject[] objects)
		{			
			for (int i = 0; i < objects.Length; i++)
			{		
				GameObject newObj = objects[i];			
			
				if (newObj.GetComponent<AudioSource>() == null) 
				{
					// need to keep the original y pos.
					newObj.transform.position = new Vector3(0, newObj.transform.position.y, 100);
					newObj.SetActive(true);
				}
			}

			// wait a frame
			yield return null;
			
			for (int j = 0; j < objects.Length; j++)
			{		
				GameObject newObj = objects[j];
			
				if (newObj.GetComponent<AudioSource>() == null) 
				{
					newObj.SetActive(false);
				}
			}
		}

		private IEnumerator CompleteObjectPool()
		{
			yield return null;
			yield return null;

			eventDispatcher.Broadcast ( ObjectPoolMessage.OBJECT_POOL_COMPLETE );
		}

		/// <summary>	
		/// Gets a new object for the name type provided.  If no object type exists or if onlypooled is true and there is no objects of that type in the pool	
		/// then null will be returned.	
		/// </summary>	
		/// <returns>	
		/// The object for type.	
		/// </returns>	
		/// <param name='objectType'>	
		/// Object type.	
		/// </param>	
		/// <param name='onlyPooled'>	
		/// If true, it will only return an object if there is one currently pooled.	
		/// </param>	
		public GameObject GetObjectForType(string objectType, bool onlyPooled = false)		
		{
			for (int i = 0; i < Entries.Count; i++)			
			{
				if (Entries[i].name != objectType)				
					continue;

				if (Pool[i].Count > 0)				
				{
					GameObject pooledObject = Pool[i][0];

					Pool[i].RemoveAt(0);

					pooledObject.transform.parent = null;
					//pooledObject.SetActiveRecursively(true);
					pooledObject.SetActive(true);
					return pooledObject;
				}

				/*if (!onlyPooled)
				{
					return Instantiate(Entries[i].Prefab) as GameObject;
				}*/

				if( !onlyPooled )				
				{			
					//Debug.Log("only pooled");

					GameObject newObj = GameObject.Instantiate( Entries[ i ].Prefab ) as GameObject;				
					newObj.name = Entries[ i ].name;				
					return newObj;				
				}
			}

			//If we have gotten here either there was no object of the specified type or non were left in the pool with onlyPooled set to true		
			return null;		
		}
			
		/// <summary>	
		/// Pools the object specified.  Will not be pooled if there is no prefab of that type.	
		/// </summary>	
		/// <param name='obj'>	
		/// Object to be pooled.	
		/// </param>	
		public void PoolObject(GameObject obj, string objName = null)		
		{
			if (obj != null && ContainerObject != null)
			{
				for (int i = 0; i < Entries.Count; i++)			
				{			
					// Better for the GC if we pass the name instead of getting it from the GameObject
					if (objName != null)
					{
						if (Entries[i].name != objName) 				
							continue;
					}
					else
					{
						if (Entries[i].name != obj.name) 				
							continue;				
					}			
					

					//obj.SetActiveRecursively(false);			
					obj.SetActive(false);
					obj.transform.parent = ContainerObject.transform;

					Pool[i].Add(obj);
					return;			
				}	
			}
		}
	}

	/// <summary>	
	/// Member class for a prefab entered into the object pool	
	/// </summary>	
	public struct ObjectPoolEntry 
	{		
		/// <summary>		
		/// the object to pre-instantiate		
		/// </summary>			
		public GameObject Prefab;
		
		public string name;

		/// <summary>		
		/// quantity of object to pre-instantiate		
		/// </summary>			
		public int Count;		
	}
}