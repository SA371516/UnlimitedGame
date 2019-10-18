using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Confug : MonoBehaviour
{
    public static Confug _confug;
    static KeyCode[] _code =new KeyCode[6];//Up,Down,Left,Right,Dushの順番
    bool _select;
    int _changeLength;
    static float _soundValume;
    static float _mouseMove;

    private void Awake()
    {
        if (_confug == null)
        {
            _confug = this;
            _code[0] = KeyCode.W;
            _code[1] = KeyCode.S;
            _code[2] = KeyCode.A;
            _code[3] = KeyCode.D;
            _code[4] = KeyCode.LeftShift;
            _mouseMove = 1f;
            DontDestroyOnLoad(gameObject);
        }
        else
        {

        }
    }
    void Start()
    {
        _select = false;
    }

    void Update()
    {
        //操作ボタン変更処理
        if (!_select) return;
        if (Input.anyKeyDown)
        {
            foreach (KeyCode keycode in Enum.GetValues(typeof(KeyCode))) 
            {
                if (Input.GetKeyDown(keycode))//ボタンを変える処理
                {
                    _code[_changeLength] = keycode;
                }
            }
            _select = true;
        }
    }

    public void ButtonClick(int L)
    {
        _select = true;
        _changeLength = L;
    }
    
    //返す値をそれぞれに変える
    public T StatusInctance<T>(string str="",int L=9999)
    {
        T returnvalume = default;
        if (typeof(T) == typeof(float))
        {
            returnvalume = (T)(object)_mouseMove;
        }
        if (typeof(T) == typeof(KeyCode))
        {
            returnvalume=(T)(object) _code[L];
        }
        return returnvalume;
    }

}
