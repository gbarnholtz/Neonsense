using Sirenix.OdinInspector;
using UnityEngine;

public class PooledObject : MonoBehaviour
{
    /*
	For the prefab, next points at the head of the freelist.
	For inactive instances, next points at the next inactive instance.
	For active instances, next points back at the source prefab.
	*/
    [System.NonSerialized] PooledObject next;

    public static T Create<T>(T prefab, Vector3 pos, Quaternion rot) where T : PooledObject
    {
        T result = null;
        if (prefab.next != null)
        {
            /*
			We have a free instance ready to recycle.
			*/
            result = (T)prefab.next;
            prefab.next = result.next;
            result.transform.SetPositionAndRotation(pos, rot);
        }
        else
        {
            /*
			There are no free instances, lets allocate a new one.
			*/
            result = Instantiate<T>(prefab, pos, rot);
            result.gameObject.hideFlags = HideFlags.HideInHierarchy;
        }
        result.next = prefab;
        result.gameObject.SetActive(true);
        return result;
    }

    public void Release()
    {
        if (next == null)
        {
            /*
			This instance wasn't made with Create(), so let's just destroy it.
			*/
            Destroy(gameObject);
        }
        else
        {
            /*
			Retrieve the prefab we were cloned from and add ourselves to its
			free list.
			*/
            var prefab = next;
            gameObject.SetActive(false);
            next = prefab.next;
            prefab.next = this;
        }
    }
}