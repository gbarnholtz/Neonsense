using Sirenix.OdinInspector;
using UnityEngine;

public class TriggerBarrier : SerializedMonoBehaviour
{
    public GameObject[] Barriers;

    private void Start()
    {
        for (int i = 0; i < Barriers.Length; i++)
        {
            Barriers[i].SetActive(false);   
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        for (int i = 0; i < Barriers.Length; i++)
        {
            Barriers[i].SetActive(true);
        }
    }
}
