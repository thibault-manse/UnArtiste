using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject completionPanel;

    public static GameManager Instance { get; private set; }

    private bool finished;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        if (completionPanel != null)
        {
            completionPanel.SetActive(false);
        }
    }

    public void GameOver()
    {
        finished = true;
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }

    public void CompletePrototype()
    {
        if (finished)
        {
            return;
        }

        finished = true;
        if (completionPanel != null)
        {
            completionPanel.SetActive(true);
        }

        PlayerController controller = FindAnyObjectByType<PlayerController>();
        controller?.LockControls(true);
    }

    public void OnRestart(InputAction.CallbackContext context)
    {
        if (context.performed && finished)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
