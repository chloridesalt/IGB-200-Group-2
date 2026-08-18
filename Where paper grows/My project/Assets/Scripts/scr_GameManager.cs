using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager s_Instance { get; private set; }
    public Camera MainCamera;
    public GameObject Sun;

    private void Awake()
    {
        if (s_Instance != null && s_Instance != this)
        {
            Destroy(gameObject); 
            return;
        }

        s_Instance = this;

        DontDestroyOnLoad(gameObject); 
    }


}