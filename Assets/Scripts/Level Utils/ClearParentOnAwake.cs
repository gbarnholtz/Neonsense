using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearParentOnAwake : MonoBehaviour
{
    void Awake()
    {
        transform.SetParent(null);
    }
}
