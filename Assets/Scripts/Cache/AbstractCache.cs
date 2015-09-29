using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class AbstractCache<T> : MonoBehaviour where T : MonoBehaviour {

	public int cacheSize;
	public T[] cacheTypes;


	protected Dictionary<string,T[]> _dictionary;

	// Use this for initialization
	void Start () {
        _dictionary = new Dictionary<string, T[]>();
		foreach(T type in cacheTypes){
			T[] arr = new T[cacheSize];
			for (int i=0; i<cacheSize; i++) {
				T instance = Instantiate(type);
				instance.gameObject.SetActive(false);
				instance.transform.parent = transform;
				arr[i] = instance;
			}
			_dictionary.Add (type.GetType().Name, arr);
            Debug.Log(typeof(T).Name + " - " + type.GetType().Name);
		}
	}

	public void Return(T obj)
	{
		var key = obj.GetType ().Name;
        bool cached = false;

        if (_dictionary.ContainsKey(key))
        { 
		    T[] arr = _dictionary [key];
            
		    for (int i=0; i<arr.Length; i++) {
			    if(arr[i] == null)
			    {
                    obj.gameObject.SetActive(false);
				    arr[i] = obj;
                    cached = true;
                    break;
			    }                                                      
		    }
        }
        else
        {
            Debug.Log("Return Has no key - " + key);
        }


        if (!cached)
		    Destroy (obj.gameObject);
	}

	public T Get(int cacheTypePosition)
	{
		T obj = null;
		string key = cacheTypes [cacheTypePosition].name;
        if (_dictionary.ContainsKey(key))
        {
            T[] arr = _dictionary[key];
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] != null)
                {
                    obj = arr[i];
                    arr[i] = null;
                    break;
                }
            }

            if (obj != null)
                return obj;
        }
        else
        {
            Debug.Log("GET Has no key - " + key);
        }


		return Instantiate(cacheTypes[cacheTypePosition]);
	}
}
