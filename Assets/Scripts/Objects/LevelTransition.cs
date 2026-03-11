using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : InteractableObject
{
    [Header("Transition Settings")]
    [Tooltip("The exact name of the Scene you want to load (it must be in Build Settings).")]
    public string nextLevelName;

    [Tooltip("Check this if you want the level to load when walking into the object (Needs a collider to be marked as Trigger).")]
    public bool triggerToLoad = true;

    [Tooltip("Check this if you want the level to load when looking at it and pressing Interact.")]
    public bool interactToLoad = false;

    // This overrides the InteractableObject method so clicking / pressing E works
    public override void Interact()
    {
        if (interactToLoad)
        {
            LoadNextLevel();
        }
    }

    // This runs automatically if the player walks into a Trigger collider
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("LevelTransition Triggered by: " + other.gameObject.name + " (Tag: " + other.gameObject.tag + ")");

        if (triggerToLoad)
        {
            if (other.transform.root.CompareTag("Player"))
            {
                Debug.Log("It IS the Player! Attempting to load...");
                LoadNextLevel();
            }
            else
            {
                Debug.Log("It entered the trigger, but it does NOT have the 'Player' tag.");
            }
        }
        else
        {
            Debug.Log("triggerToLoad is FALSE. Ignoring walk-in.");
        }
    }

    private void LoadNextLevel()
    {
        if (string.IsNullOrEmpty(nextLevelName))
        {
            Debug.LogWarning("Next Level Name is empty! Please set it in the Inspector.");
            return;
        }

        Debug.Log("Loading level: " + nextLevelName);
        SceneManager.LoadScene(nextLevelName);
    }
}

