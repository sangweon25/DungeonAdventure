using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.OnScreen;

public class ObjectPoolManager : MonoBehaviour
{
    //풀에 넣을 프리팹들
    public GameObject[] prefabs;
    //미리 생성하고 꺼내쓸 풀
    private Dictionary<int, Queue<GameObject>> _pools = new Dictionary<int, Queue<GameObject>>();

    public static ObjectPoolManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        // 풀에 넣을 프리팹의 갯수에 따라 큐 공간에 자리를 만들어둠(해당 큐에는 프리팹이 여러개 들어감 같은종류)
        for (int i = 0; i < prefabs.Length; i++)
        {
            _pools[i] = new Queue<GameObject>();
        }
    }

    public GameObject GetObject(int prefabIndex, Vector3 position, Quaternion rotation)
    {
        if (!_pools.ContainsKey(prefabIndex))
        {
            Debug.LogWarning($"ObjectPoolManager Can't find {prefabIndex} key");
            return null;
        }
        GameObject obj;
        //풀에 남는 오브젝트가 있는경우
        if (_pools[prefabIndex].Count > 0)
        {
            obj = _pools[prefabIndex].Dequeue();
        }
        else //풀에 오브젝트가 없는 경우
        {
            // 프리팹 배열에서 생성
            obj = Instantiate(prefabs[prefabIndex]);
            // Pool로 관리될 오브젝트( IPoolable을 가지고 있다면) Return 델리게이트 구독
            obj.GetComponent<IPoolable>()?.Initialize(o => ReturnObject(prefabIndex, o));
        }

        obj.transform.SetPositionAndRotation(position, rotation);
        obj.SetActive(true);
        obj.GetComponent<IPoolable>()?.OnSpawn();
        return obj;
    }

    public void ReturnObject(int index, GameObject obj)
    {
        //풀에 등록된 오브젝트가 아니라면 삭제
        if (!_pools.ContainsKey(index))
        {
            Destroy(obj);
            return;
        }

        //풀에 들어가기전 비활성화
        obj.SetActive(false);
        //풀에 다시 집어넣기
        _pools[index].Enqueue(obj);
    }
}
