using UnityEngine;

public class ChoppableManager : MonoBehaviour
{
    public static ChoppableManager Instance;

    private ChoppableObject[] choppableObjects;

    private void Awake()
    {
        #region Singleton

        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        #endregion

        choppableObjects = FindObjectsByType<ChoppableObject>(FindObjectsSortMode.None);
    }

    private void OnEnable()
    {
        foreach (var choppable in choppableObjects) choppable.OnChopFinish += Despawn;
    }

    private void OnDisable()
    {
        foreach (var choppable in choppableObjects) choppable.OnChopFinish -= Despawn;
    }

    private void Despawn(ChoppableObject choppedObject)
    {
        choppedObject.transform.root.gameObject.SetActive(false);
    }

    public void RespawnAll()
    {
        foreach (var choppable in choppableObjects) choppable.transform.root.gameObject.SetActive(true);
    }
}