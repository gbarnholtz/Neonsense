using UnityEngine;

public class LowerVolumeOnTrigger : MonoBehaviour
{
    [SerializeField] float volumeDecrease = 0.1f; // The amount by which volume will be decreased
    [SerializeField] float transitionTime = 1f; // The time it takes for the volume to decrease

    [SerializeField] private AudioSource audioSource;
    private float _originalVolume;

    void Start()
    {
        // Store the original volume
        _originalVolume = audioSource.volume;
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the collider entering the trigger zone has a tag "Player"
        if (other.CompareTag("Player"))
        {
            // Lower the volume over transitionTime seconds
            StartCoroutine(LowerVolumeOverTime());
        }
    }

    System.Collections.IEnumerator LowerVolumeOverTime()
    {
        // Calculate the target volume
        float targetVolume = _originalVolume - volumeDecrease;

        // Gradually decrease the volume over transitionTime seconds
        float elapsedTime = 0f;
        while (elapsedTime < transitionTime)
        {
            // Interpolate between originalVolume and targetVolume
            audioSource.volume = Mathf.Lerp(_originalVolume, targetVolume, elapsedTime / transitionTime);

            // Increment the elapsed time
            elapsedTime += Time.deltaTime;

            // Wait for the next frame
            yield return null;
        }

        // Ensure the volume is set to the targetVolume
        audioSource.volume = targetVolume;
    }
}
