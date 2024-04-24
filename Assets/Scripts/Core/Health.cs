using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Random = System.Random;

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
    
    [SerializeField] private AudioClip[] hitSound;
    //[SerializeField] private AudioClip impactDrone;
    private GameObject audioSourceObj;
    private AudioSource audioSource;

    private bool isCouroutineRunning = false;

    [SerializeField] private ArenaManager _arenaManager;


    public void Start()
    {
        team = gameObject.GetComponent<Teamable>().Team;
        
        // UNCOMMENT THESE AND THE OTHER BLOCK TO RESTORE AUDIO
        //audioSourceObj = GameObject.FindWithTag("player_audio_source");
        //audioSource = audioSourceObj.GetComponent<AudioSource>();
        
        //_arenaManager = GetComponent<ArenaManager>();
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
            
            // this has been causing the audio to go haywire
            //audioSource.PlayOneShot(hitSound[UnityEngine.Random.Range(0, hitSound.Length)]);
            
            if (team == Team.Enemy)
            {
                UI_Manager.Instance.EnableHitMarker(hurtTime);

                /* Switch mat for normal enemies*/
                if (gameObject.tag == "normal_enemy" && !isCouroutineRunning)
                    StartCoroutine(SwitchMat());
            }

            if (Current <= 0)
            {
                if (team == Team.Enemy)
                {
                    if (_arenaManager != null)
                    {
                        _arenaManager.CheckIfEnemiesDefeated();
                        //Debug.Log("CheckIfEnemiesDefeated() called by " + gameObject.name);
                    }
                    
                    Destroy(gameObject);
                }
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
