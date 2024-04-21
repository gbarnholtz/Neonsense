using UnityEngine;

public class Crate : MonoBehaviour
{
    [SerializeField] public int AddToAmmo;
    [SerializeField] public float AddToHealth;
    
    [Header("Sounds")]
    private AudioSource soundAttachedToPlayer;
    [SerializeField] private AudioClip healthSound;
    [SerializeField] private AudioClip ammoSound;
    //[SerializeField] [Range(0, 1)] private float soundVolume = 1f;

    void Start()
    {
        GameObject audioSourceObj = GameObject.FindWithTag("player_audio_source");
        soundAttachedToPlayer = audioSourceObj.GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (gameObject.tag == "ammo_pickup")
            {
                ((RangedWeapon)ArsenalController.activeWeapon).AmmoPool += AddToAmmo;
                
                if (soundAttachedToPlayer != null && ammoSound != null)
                {
                    //soundAttachedToPlayer.PlayOneShot(ammoSound, soundVolume);
                    soundAttachedToPlayer.PlayOneShot(ammoSound);
                }
                
                Destroy(gameObject);    
            }
            if (gameObject.tag == "health_pickup")
            {
                other.gameObject.GetComponent<Health>().AddToHealth(AddToHealth);
                
                if (soundAttachedToPlayer != null && healthSound != null)
                {
                    //soundAttachedToPlayer.PlayOneShot(healthSound, soundVolume);
                    soundAttachedToPlayer.PlayOneShot(healthSound);
                }
                
                Destroy(gameObject);
            }
        }
    }
}
