using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class AbstractCache<T> : MonoBehaviour where T : MonoBehaviour {

	public int cacheSize;
	public T[] cacheTypes;


	protected Dictionary<string,T[]> _dictionary;

	// Use this for initialization
	void Start () {
		foreach(T type in cacheTypes){
			T[] arr = new T[cacheSize];
			for (int i=0; i<cacheSize; i++) {
				T instance = Instantiate(type);
				instance.gameObject.SetActive(false);
				instance.transform.parent = transform;
				arr[i] = instance;
			}
			_dictionary.Add (type.GetType().Name, arr);
		}
	}

	public void Return(T obj)
	{
		var key = obj.GetType ().Name;
		T[] arr = _dictionary [key];
		for (int i=0; i<arr.Length; i++) {
			if(arr[i] == null)
			{
				arr[i] = obj;
				return;
			}
		}

		Destroy (obj.gameObject);
	}

	public T Get(int cacheTypePosition)
	{
		T obj = null;
		string key = cacheTypes [cacheTypePosition].name;
		T[] arr = _dictionary [key];
		for(int i=0; i<arr.Length; i++)
		{
			if(arr[i] != null)
			{
				obj = arr[i];
				arr[i] = null;
				return obj;
			}
		}

		return Instantiate(cacheTypes[cacheTypePosition]);
	}
}
