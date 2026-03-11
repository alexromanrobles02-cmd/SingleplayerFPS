using UnityEngine;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;

    [Header("Level Progression")]
    [Tooltip("Arrastra aquí el cubo 'Portal' u otros objetos que quieras que aparezcan al matar a todos los zombis.")]
    public List<GameObject> objectsToEnableOnClear = new List<GameObject>();

    private int currentEnemyCount = 0;

    private void Awake()
    {
        // Configuramos la instancia para que los enemigos la encuentren fácilmente
        Instance = this;
    }

    private void Start()
    {
        // Por defecto, escondemos todos los portales al iniciar el nivel
        foreach (GameObject obj in objectsToEnableOnClear)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
    }

    // Los zombis llamarán a esta función al nacer
    public void RegisterEnemy()
    {
        currentEnemyCount++;
        Debug.Log("Zombi registrado. Vivos: " + currentEnemyCount);
    }

    // Los zombis llamarán a esta función al morir
    public void UnregisterEnemy()
    {
        currentEnemyCount--;
        Debug.Log("Zombi eliminado. Restantes: " + currentEnemyCount);

        if (currentEnemyCount <= 0)
        {
            OnAllEnemiesDefeated();
        }
    }

    private void OnAllEnemiesDefeated()
    {
        Debug.Log("¡Todos los enemigos han sido eliminados! Activando portales...");
        
        // Hacemos aparecer el cubo/portal
        foreach (GameObject obj in objectsToEnableOnClear)
        {
            if (obj != null)
            {
                obj.SetActive(true);
            }
        }
    }
}
