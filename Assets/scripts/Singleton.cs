using UnityEngine;

// Родитель синглтонов.
public class Singleton<T>:MonoBehaviour where T : class {

	public static T Instance {
		get;
		protected set;
	}

	public Singleton() {
		Instance = this as T;
	}

}
