using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ItemDropper : MonoBehaviour
{
    [SerializeField] private ItemDropTableSO _dropTable;
    private float[] _itemWeights;

    [SerializeField][Range(0, 1)] private float _dropChance;

    private void Start()
    {
        _itemWeights = _dropTable.DropList.Select(item => item.Rate).ToArray();
    }

    public void DropItem()
    {
        float ratio = Random.value;

        if(ratio < _dropChance)
        {
            int idx = GetRamdomWeightsIndex();
            PoolableMono resource = PoolManager.Instance.Pop(_dropTable.DropList[idx].ItemPrefab.name);
            resource.transform.position = transform.position;
        }
    }

    private int GetRamdomWeightsIndex()
    {
        float sum = 0;
        for(int i = 0; i < _itemWeights.Length; i++)
        {
            sum += _itemWeights[i];
        }

        float randValue = Random.Range(0, sum);
        float temsum = 0;

        for(int i = 0; i < _itemWeights.Length; i++)
        {
            if(randValue >= temsum && randValue < temsum + _itemWeights[i])
            {
                return i;
            }
            else
            {
                temsum += _itemWeights[i];
            }
        }

        return 0;
    }
}
