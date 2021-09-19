using UnityEngine;
using System.Collections;

public abstract class PrefabPool<T> : MonoBehaviour
{
  public GameObject poolPrefab;
  public int poolSize = 10;

  private GameObject[] pool;

  protected virtual void Start()
  {
    pool = new GameObject[poolSize];
    GameObject obj;
    for (int i = 0; i < poolSize; ++i) {
      obj = Instantiate(poolPrefab, transform.position, Quaternion.identity) as GameObject;
      obj.SetActive(false);
      pool[i] = obj;
    }
  }

  public T GetFromPool(Vector3 position)
  {
    GameObject obj = GetFirstActive();

    obj.SetActive(true);
    obj.transform.position = position;

    return obj.GetComponent<T>();
  }

  GameObject GetFirstActive()
  {
    // TODO: Maybe better algo, than brute force ???
    GameObject obj = pool[0];
    for (int i = 0; i < pool.Length; ++i) {
      obj = pool[i];
      if (!obj.activeSelf) {
        break;
      }
    }
    return obj;
  }
}
