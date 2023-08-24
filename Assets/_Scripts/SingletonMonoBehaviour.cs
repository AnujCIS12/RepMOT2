using System;
using UnityEngine;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{

    protected static T instance =  null;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                Type t = typeof(T);

                instance = (T)FindObjectOfType(t);
                if (instance == null)
                {
                    Debug.LogError(t + " unable to create object");
                }
            }

            return instance;
        }
    }

	public static void Create( string Name, Transform Parent = null )
	{
		if( instance != null )
		{
			return;
		}

		GameObject Obj = new GameObject( Name );
		Obj.transform.SetParent(Parent, false);
		instance = Obj.AddComponent<T>();
	}

	public static void Create(GameObject go)
	{
		if(instance!=null)
		{
			return;
		}
		instance = go.AddComponent<T>();
	}

    virtual protected void Awake()
    {
        if (this != Instance)
        {
            Destroy(this);
            Debug.LogError(
                typeof(T) +
                "Has already been attached to another GameObject, so I discarded the component." +
                " The attached GameObject is " + Instance.gameObject.name + "is.");
            return;
        }
    }

    virtual protected void OnDestroy()
    {
        if( instance == this )
        {
            instance = null;
        }
    }
    protected void SetDontDestroy()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    protected virtual void OnCreate()
    {
        
    }
}
