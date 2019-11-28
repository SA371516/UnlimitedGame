using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Save", fileName = "Save")]
public class SaveScript : ScriptableObject
{
    public string UserName;
    public string PassWord;
    public int Point;
    public int SRLevel;
    public int ARLevel;
}
