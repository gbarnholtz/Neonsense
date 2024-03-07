using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Modifier<T>
{
    public T Modify(T modifyTarget);
}
