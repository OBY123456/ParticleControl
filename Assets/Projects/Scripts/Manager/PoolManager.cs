
using MTFrame.MTPool;
using UnityEngine;

/// <remarks>对象池管理类</remarks>
public class PoolManager:MonoBehaviour
{
    public static PoolManager Instance;

    //通用对象池
    public static GenericPropPool genericPropPool;
    [Header("通用池预设")]
    public GameObject GenericPrefabs;

    private void Awake()
    {
        Instance = this;
        Init();
    }

    /// <summary>
    /// 初始化
    /// </summary>
    private void Init()
    {
        genericPropPool = new GenericPropPool();
        genericPropPool.Init();
    }

    /// <summary>
    /// 放回对象池
    /// </summary>
    public void AddPool(PoolType poolType,GameObject go)
    {
        switch (poolType)
        {
            case PoolType.GenericProp:
                genericPropPool.AddPool(go);
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// 获取对象
    /// </summary>
    public GameObject GetPool(PoolType poolType)
    {
        GameObject t;
        switch (poolType)
        {
            case PoolType.GenericProp:
                t= genericPropPool.GetPool();
                if(t == null)
                {
                    t = Instantiate(GenericPrefabs);
                    genericPropPool.UsePool.Add(t);
                }
                break;
            default:
                t = null;
                break;
        }
        return t;
    }

    private void OnDestroy()
    {
        genericPropPool.Clear();
    }
}