using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    public static T instance
    {
        get
        {
            return _instance;
        }
    }

    public virtual void Awake()
    {
        Debug.Log($"Singleton Awake called for {typeof(T).Name}");

        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(this.gameObject);
            Debug.Log($"First instance of {typeof(T).Name} - keeping this one");
        }
        else
        {
            Debug.Log($"Duplicate {typeof(T).Name} found - destroying this one");
            Destroy(gameObject);
        }
    }
}