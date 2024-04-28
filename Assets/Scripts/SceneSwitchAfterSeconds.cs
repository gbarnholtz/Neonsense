using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitchAfterSeconds : MonoBehaviour
{
    public float delayInSeconds = 11f; // Adjust this value to set the delay before scene switch

    void Start()
    {
        // Invoke the SwitchScene method after delayInSeconds seconds
        Invoke("SwitchScene", delayInSeconds);
    }

    void SwitchScene()
    {
        // Load the first scene in the build index
        SceneManager.LoadScene(0);
    }
}
