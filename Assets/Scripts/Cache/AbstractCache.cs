using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class AbstractCache<T> : MonoBehaviour where T : MonoBehaviour {

	public int cacheSize;
	public T[] cacheTypes;


	protected Dictionary<string,List<T>> _dictionary;

	// Use this for initialization
	void Start () {
        _dictionary = new Dictionary<string, List<T>>();
		foreach(T type in cacheTypes){
			List<T> arr = new List<T>();
			for (int i=0; i<cacheSize; i++) {
				T instance = Instantiate(type);
				instance.transform.position = new Vector3(-100, -100, 0);
				instance.transform.SetParent(transform);
                instance.gameObject.SetActive(false);
                arr.Add(instance);
                AdditionStart(instance);
			}
			_dictionary.Add (type.GetType().Name, arr);
		}
	}

    protected virtual void AdditionStart(T item)
    {

    }

	public virtual void Return(T obj)
	{                 

		var key = obj.GetType ().Name;
        bool cached = false;

        if (_dictionary.ContainsKey(key))
        { 
		    List<T> arr = _dictionary [key];
            if(arr.Count < cacheSize)
            {       
                obj.transform.position = new Vector3(-100, -100, 0);
                obj.transform.rotation = Quaternion.identity;
                obj.transform.SetParent(transform);
                obj.gameObject.SetActive(false);
                arr.Add(obj);
                cached = true;
            }
        }

        if (!cached)
            Destroy(obj.gameObject);
	}

	public T Get(int cacheTypePosition)
	{
		T obj = null;
		string key = cacheTypes [cacheTypePosition].name;
        if (_dictionary.ContainsKey(key))
        {
            List<T> arr = _dictionary[key];
            if(arr.Count > 0)
            {
                obj = arr[0];
                arr.RemoveAt(0);
            }

            if (obj != null)
                return obj;
        }

		return Instantiate(cacheTypes[cacheTypePosition]);
	}
}
