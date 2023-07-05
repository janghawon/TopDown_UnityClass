using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/EnemyDataSO")]
public class EnemyDataSO : ScriptableObject
{
    public int MaxHP;
    public float MoveSpeed;
    public float RotateSpeed;
    public int AtkDamage;
    public float MotionDelay; //��� ���ݵ�����
    public float AtkCoolTime; //���� ��Ÿ��
}
//ž�ٿ� 