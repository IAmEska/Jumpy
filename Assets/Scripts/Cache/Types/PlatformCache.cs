using UnityEngine;
using System.Collections;

public class PlatformCache : AbstractCache<Platform> {
    public override void Return(Platform obj)
    {
        var key = obj.GetType().Name;
        bool cached = false;

        if (_dictionary.ContainsKey(key))
        {
            Platform[] arr = _dictionary[key];

            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] == null)
                {
                    obj.Reset();
                    obj.gameObject.SetActive(false);
                    arr[i] = obj;
                    cached = true;
                    break;
                }
            }
        }

        if (!cached)
            Destroy(obj.gameObject);
    }

}
