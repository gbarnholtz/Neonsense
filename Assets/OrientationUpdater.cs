using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrientationUpdater : SerializedMonoBehaviour
{
    [OdinSerialize] private IInputProvider inputProvider;
    void Update()
    {
        transform.rotation = Quaternion.Euler(inputProvider.GetState().lookEulers);
    }
}
