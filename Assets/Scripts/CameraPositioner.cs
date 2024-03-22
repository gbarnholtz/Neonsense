using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPositioner : MonoBehaviour
{
    [SerializeField] private int index;
    private Camera cam;
    private void Awake()
    {
        cam = GetComponent<Camera>();
        cam.rect = new Rect(0, 0, 0.25f, 0.25f);
        gameObject.SetActive(true);
    }
}
