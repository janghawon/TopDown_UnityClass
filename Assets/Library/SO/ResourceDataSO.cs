using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;

[CreateAssetMenu(menuName ="SO/Item/ResourceData")]
public class ResourceDataSO : ScriptableObject
{
    public float Rate;
    public PoolableMono ItemPrefab;

    public ResourceType ResourceType;
    public int MinAmount = 1, MaxAmount = 5;

    public AudioClip UseSound;
    public Color PopupTextColor;

    public int GetAmount()
    {
        return Random.Range(MinAmount, MaxAmount + 1);
    }
}
