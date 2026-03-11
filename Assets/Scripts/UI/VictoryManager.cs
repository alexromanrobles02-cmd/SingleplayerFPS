using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class VictoryManager : MonoBehaviour
{
    [Header("UI Panels")]
    [Tooltip("Drag the Victory Panel here")]
    public GameObject victoryPanel;
    
    [Tooltip("Drag the Credits Panel here")]
    public GameObject creditsPanel;

    [Header("Timings")]
    [Tooltip("How many seconds the Victory screen stays visible")]
    public float victoryDuration = 4f;
    
    [Tooltip("How many seconds the Credits screen stays visible before quitting")]
    public float creditsDuration = 6f;

    [Header("Scrolling Credits")]
    [Tooltip("How fast the credits will scroll upwards")]
    public float scrollSpeed = 50f;

    private void Update()
    {
        // Si el panel de créditos está activado (ha terminado la victoria), lo movemos hacia arriba poco a poco
        if (creditsPanel != null && creditsPanel.activeSelf)
        {
            creditsPanel.transform.Translate(Vector3.up * scrollSpeed * Time.deltaTime);
        }
    }

    private void Start()
    {
        // Make sure the cursor is visible if the player wants to click around (optional)
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (victoryPanel == null || creditsPanel == null)
        {
            Debug.LogError("Faltan asignar los paneles en el VictoryManager");
            return;
        }

        // Start the sequence
        StartCoroutine(VictorySequence());
    }

    private IEnumerator VictorySequence()
    {
        // 1. Show Victory, Hide Credits
        victoryPanel.SetActive(true);
        creditsPanel.SetActive(false);

        // Wait...
        yield return new WaitForSeconds(victoryDuration);

        // 2. Hide Victory, Show Credits
        victoryPanel.SetActive(false);
        creditsPanel.SetActive(true);

        // Wait...
        yield return new WaitForSeconds(creditsDuration);

        // 3. Finish Game
        Debug.Log("Juego Completado. Cerrando aplicación...");
        Application.Quit();

        // If playing in the Unity Editor, stop the play mode
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
