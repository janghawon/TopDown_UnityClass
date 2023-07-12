using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressableTest : MonoBehaviour
{
    [SerializeField] private AssetReference _ref;
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            LoadEnemy();
        }
        if(Input.GetKeyDown(KeyCode.D))
        {
            DestroyEnemy();
        }
    }

    private AsyncOperationHandle _handle;
    private GameObject _enemy;

    private void LoadEnemy()
    {
        _ref.InstantiateAsync(Vector3.zero, Quaternion.identity).Completed += obj =>
        {
            _handle = obj;
            _enemy = obj.Result;
        };
    }

    private void DestroyEnemy()
    {
        Destroy(_enemy);
        Addressables.Release(_handle);
    }
}
