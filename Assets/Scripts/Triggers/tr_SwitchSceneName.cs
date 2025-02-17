using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Switches to the scene with the name in SceneName
public class tr_SwitchSceneName : MonoBehaviour
{
    [SerializeField] private string SceneName;
    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene(SceneName);
        }
    }
}
