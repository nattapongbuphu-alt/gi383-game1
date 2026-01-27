using UnityEngine;

public class CoroutineRunner : MonoBehaviour
{
    static CoroutineRunner instance;
    public static CoroutineRunner Instance
    {
        get
        {
            if (instance == null)
            {
                var go = new GameObject("CoroutineRunner");
                instance = go.AddComponent<CoroutineRunner>();
                DontDestroyOnLoad(go);
            }
            return instance;
        }
    }
}
