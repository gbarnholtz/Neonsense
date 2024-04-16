using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ElevatorTriggerNextLevel : MonoBehaviour
{
    [SerializeField] private Animator elevatorDoors;
    [SerializeField] private float waitSeconds = 5;
    private float _secondsUntilMusicStarts = 2.5f;
    [SerializeField] private AudioSource levelMusic;
    
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            elevatorDoors.Play("Elevator close");
        
            StartCoroutine(WaitToLoadLevel());
        }
    }

    IEnumerator WaitToLoadLevel()
    {
        yield return new WaitForSeconds(_secondsUntilMusicStarts);
        levelMusic.Stop();
        
        yield return new WaitForSeconds(waitSeconds);
        
        Scene thisScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(thisScene.buildIndex + 1);
    }
}
