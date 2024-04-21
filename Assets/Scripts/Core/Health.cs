using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Health : Progressive, IDamageable
{
    public bool DisplayOverheadHealth;
    private Rigidbody rb;
    public event Action HealthEmpty;
    public Team Team { get => team; set => team = value; }
    private Team team;
    public List<IDamageModifier> DamageModifiers = new List<IDamageModifier>();

    /* Only for mat swap purposes */
    [SerializeField] private Material hurtMat;
    [SerializeField] private GameObject Ch44;
    [SerializeField] private float hurtTime;
    
    [SerializeField] private AudioClip impact;
    private GameObject audioSourceObj;
    private AudioSource audioSource;

    private bool isCouroutineRunning = false;


    public void Start()
    {
        team = gameObject.GetComponent<Teamable>().Team;
        audioSourceObj = GameObject.FindWithTag("player_audio_source");
        audioSource = audioSourceObj.GetComponent<AudioSource>();
    }
    
    public void OnEnable()
    {
        DamageModifiers.Clear();
        rb = transform.GetComponent<Rigidbody>();
    }

    private IEnumerator SwitchMat()
    {
        isCouroutineRunning = true;

        // Get all Renderer components attached to the GameObject
        Renderer renderer = Ch44.GetComponent<Renderer>();
        Material blueMat = renderer.material;
        renderer.material = hurtMat;
        yield return new WaitForSeconds(hurtTime);
        renderer.material = blueMat;

        isCouroutineRunning = false;
    }

    public void TakeDamage(float damage, Team shooterTeam)      
    { 
        if (shooterTeam != team)
        {
            Current -= damage;
            //if (gameObject.CompareTag("drone"))
            //else if (gameObject.CompareTag("normal_enemy"))
            audioSource.PlayOneShot(impact);

            if (team == Team.Enemy)
            {
                UI_Manager.Instance.EnableHitMarker(hurtTime);

                /* Switch mat for normal enemies*/
                if (gameObject.tag == "normal_enemy" && !isCouroutineRunning)
                    StartCoroutine(SwitchMat());
            }

            if (Current <= 0)
            {
                if (team == Team.Enemy) Destroy(gameObject);
                if (team == Team.Ally)
                {
                    Scene thisScene = SceneManager.GetActiveScene();
                    SceneManager.LoadScene(thisScene.name);
                }
            }
        }
    }

    public float GetHealth()
    {
        return Current;
    }

    public void AddToHealth(float value)
    {
        Current += value;
    }
}    
