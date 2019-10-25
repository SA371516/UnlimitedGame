using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create Parameter", fileName = "Status")]
public class EnemyStatus : ScriptableObject
{
    public int HP;
    public int Score;
    public string Name;
}
