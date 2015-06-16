using UnityEngine;
using System.Collections;

/// <summary>
/// Improved copy of Sebastiaan Fehr Coroutiner. This only created one Coroutiner object and uses that
/// throughout the project rather then creating a new Coroutiner object for every StartCoroutine call 
/// which was an expensive operation when a lot of things were happening at the same time. We also don't 
/// destroy the Coroutiner object either.
/// </summary>
public class Coroutiner
{
	private static GameObject routeneHandlerGo;
	private static CoroutinerInstance routeneHandler;

	public static Coroutine StartCoroutine(IEnumerator iterationResult) 
	{
        //Create GameObject with MonoBehaviour to handle task.

		if (routeneHandlerGo == null || routeneHandler == null)
		{
			routeneHandlerGo = new GameObject("Coroutiner");
			routeneHandler = routeneHandlerGo.AddComponent(typeof(CoroutinerInstance)) as CoroutinerInstance;
		}
        
        return routeneHandler.ProcessWork(iterationResult);
    }

	/// <summary>
	/// Destroy the Coroutiner object, only use this in UnitTests.
	/// </summary>
	public static void Destroy()
	{
		if (routeneHandlerGo != null)
		{
			GameObject.DestroyImmediate(routeneHandlerGo);
		}
	}

}

public class CoroutinerInstance : MonoBehaviour 
{
    public Coroutine ProcessWork(IEnumerator iterationResult) 
	{
        return StartCoroutine(Complete(iterationResult));
    }
	   
	public IEnumerator Complete(IEnumerator iterationResult)
	{
        yield return StartCoroutine(iterationResult);
    }
}