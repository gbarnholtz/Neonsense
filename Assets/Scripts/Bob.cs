using UnityEngine;

namespace DeLeoJack
{
    public class Bob : MonoBehaviour
    {
        [SerializeField] private float bobSpeed = 4;
        [SerializeField] private float bobHeight = 0.2f;

        private Vector3 initialPosition;

        void Start()
        {
            initialPosition = transform.position;
        }

        void Update()
        {
            float y = initialPosition.y + bobHeight * Mathf.Sin(Time.time * bobSpeed);

            transform.position = new Vector3(transform.position.x, y, transform.position.z);
        }
    }
}
