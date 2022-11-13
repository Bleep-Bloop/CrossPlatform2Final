using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Component
{

    private static T instance;

    public static T Instance
    {
        get
        {
            if (!instance)
                instance = FindObjectOfType<T>();

            if (!instance)
            {
                GameObject obj = new GameObject();
                obj.name = typeof(T).Name;
                instance = obj.AddComponent<T>();
            }

            return instance;

        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        if (!instance)
        {
            instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            Debug.LogWarning("Addtional instance of " + gameObject.name + " singleton found and deleted");
        }
    }

}
