using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Confug : MonoBehaviour
{
    [SerializeField]
    GameObject _menu;
    [SerializeField]
    GameObject _changeObj;
    [SerializeField]
    Text[] _buttonText;
    [SerializeField]
    Slider _mouseSlider;
    [SerializeField]
    Text _sliderValume;

    public static Confug _confug;

    static KeyCode[] _code =new KeyCode[7];//Up,Down,Left,Right,Dush,Getの順番
    bool _select;
    bool _confugActive;
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
            _code[5] = KeyCode.E;
            _mouseMove = 1f;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        _select = false;
        _confugActive = false;
        _menu.SetActive(_confugActive);
        _changeObj.SetActive(false);
        int count = 0;
        foreach(var v in _buttonText)
        {
            v.text = _code[count].ToString();
            count++;
        }
        _mouseSlider.maxValue = 30f;
        _mouseSlider.value = 1f;
    }

    void Update()
    {
        _menu.SetActive(_confugActive);
        _mouseMove = (int)_mouseSlider.value;
        _sliderValume.text = _mouseMove.ToString();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!_confugActive)
            {
                _confugActive = true;
            }
            else if(_confugActive && !_changeObj.activeSelf)//操作ボタンを選択していない状態
            {
                _confugActive = false;
            }
        }
        //操作ボタン変更処理
        if (!_select) return;
        if (Input.anyKeyDown&& !Input.GetKeyDown(KeyCode.Escape))//Escapeキーは固定にするため
        {
            foreach (KeyCode keycode in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(keycode) && !Input.GetKeyDown(KeyCode.Mouse0))//ボタンを変える処理
                {
                    int count = 0;
                    bool _similar = false;
                    foreach (KeyCode code in _code)//かぶったら入れ替える処理
                    {
                        if (keycode == code)
                        {
                            _similar = true;
                            _code[count] = _code[_changeLength];
                            _buttonText[count].text = _code[_changeLength].ToString();
                            _code[_changeLength] = keycode;
                            _buttonText[_changeLength].text = keycode.ToString();
                            break;
                        }
                        count++;
                    }
                    if (!_similar)//かぶらなかったらそのまま
                    {
                        _code[_changeLength] = keycode;
                        _buttonText[_changeLength].text = keycode.ToString();
                    }
                    break;
                }
            }
            _select = false;
            _changeObj.SetActive(false);
        }
    }
    public void ButtonClick(int L)//この関数を呼んで変更する
    {
        _select = true;
        _changeLength = L;
        _changeObj.SetActive(true);
    }
    //返す値をそれぞれに変える
    public T StatusInctance<T>()
    {
        T returnvalume = default;
        if (typeof(T) == typeof(float))
        {
            returnvalume = (T)(object)_mouseMove;
        }
        if (typeof(T) == typeof(KeyCode[]))
        {
            returnvalume=(T)(object) _code;
        }
        if (typeof(T) == typeof(bool))
        {
            returnvalume = (T)(object)_confugActive;
        }
        return returnvalume;

    }
}
