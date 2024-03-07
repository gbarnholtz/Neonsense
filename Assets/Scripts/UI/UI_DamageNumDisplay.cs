using TMPro;
using UnityEngine;

public class UI_DamageNumDisplay : PooledObject
{
    Camera cam;
    [SerializeField] private float lifetime;
    private float timer;
    private RectTransform selfTransform;
    [SerializeField] private TextMeshProUGUI label;
    [SerializeField] private static Color color;
    private Vector3 origin;

    private void Awake()
    {
        selfTransform = GetComponent<RectTransform>();
    }

    public void Bind(Transform target, DamageInstance dmg, Camera camera) {
        origin = target.position;
        cam = camera;
        timer = 0;
        label.text = dmg.Amount.ToString();
        UpdateTicker.Subscribe(IncrementDisplay);
    }

    private void IncrementDisplay() {
        timer += Time.deltaTime;
        float prog = Mathf.Clamp01(timer / lifetime);
        
        selfTransform.position = cam.WorldToScreenPoint(origin);

        if (timer >= lifetime)
        {
            UpdateTicker.Unsubscribe(IncrementDisplay);
            Release();
        } 
    }
}
