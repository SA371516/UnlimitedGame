using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    GameObject[] _lispone;
    List<GameObject> _lis = new List<GameObject>();
    [SerializeField]
    GameObject[] _enemys;
    float _time;
    float _nextime;
    void Start()
    {
        _time = 0;
        _nextime = 2f;
        foreach(Transform v in transform)
        {
            _lis.Add(v.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        _time += Time.deltaTime;
        if (_time > _nextime)
        {
            _time = 0f;
            ObjectInctance(_enemys[0], _lis[Random.Range(0, 4)].transform.position);
        }
    }

    public void ObjectInctance(GameObject obj,Vector3 pos)
    {
        Instantiate(obj, pos, Quaternion.identity);
    }
}
