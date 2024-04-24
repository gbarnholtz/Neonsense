using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{

    public delegate void PauseToggle();
    public static PauseToggle pauseToggle;
    private bool isPaused;
    public GameObject pauseMenuUI;

    [Header("First Selections")]
    [SerializeField] private GameObject pauseMenuFirst;

    // Start is called before the first frame update
    void Start()
    {
        PlayerInputSO.pause.performed += PauseActivation;
        //pauseToggle += PauseActivation;
        isPaused = false; 
    }

    private void OnDestroy()
    {
        PlayerInputSO.pause.performed -= PauseActivation;
        //pauseToggle -= PauseActivation;
    }

    public void PauseActivation(InputAction.CallbackContext obj)
    {
        isPaused = !isPaused;
        if (isPaused)
            Pause();
        else
            Resume();
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0.0f;
        EventSystem.current.SetSelectedGameObject(pauseMenuFirst);
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1.0f;
    }
    
    public void MainMenu()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
