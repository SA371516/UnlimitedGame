using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    Text _weaponName, _bulletNum,_levelText;
    [SerializeField]
    Slider _hp, _levelSlider, _dushSlider;
    [SerializeField]
    Text _gameTime,_scoreText;
    [SerializeField]
    Image _damage,_gunImg;
    [SerializeField]
    List<Sprite> _gunSprites = new List<Sprite>();
    BasePlayer _playerInfo;
    GameManager _manager;
    float time = 0;
    int _level = 1;

    public bool _stop;
    public BaseWeapon _weapon;

    private void Start()
    {
        _playerInfo = GameObject.FindWithTag("Player").GetComponent<BasePlayer>();
        _manager = GameObject.Find("Manager").GetComponent<GameManager>();
        _hp.maxValue = _playerInfo.GetSetHP;
        _levelSlider.maxValue = _manager.GetTime;
        _dushSlider.maxValue = _playerInfo.GetSetDush;
        _stop = false;

        Color _color = new Color();
        _color = _damage.color;
        _color.a = 0;
        _damage.color = _color;
    }

    void Update()
    {
        if (_stop) return;
        //経過時間処理
        time = time+ Time.deltaTime;
        float _t = Mathf.Floor(time);
        string str = string.Format("{00}", _t.ToString());
        _gameTime.text = str;

        TextFunction();//テキスト関数
        SliderFunction();//スライダー関数
    }
    //========表示テキスト処理=========
    void TextFunction()
    {
        string str = null;
        str = "Level" + _level.ToString();
        _levelText.text = str;
        _scoreText.text = "Score:" + _manager.GetSetScore.ToString();
        //武器アイコン処理
        if (_weapon == null)
        {
            _weaponName.text = "NoWeapon";
            _bulletNum.text = "0";
            return;
        }
        switch (_weapon.GetSetWeaponName)
        {
            case "SR":
                _gunImg.sprite = _gunSprites.Find(item => item.name == "SRGun");

                break;
            case "AR":
                _gunImg.sprite = _gunSprites.Find(item => item.name == "ARGun");
                break;
        }
        _weaponName.text = _weapon.GetSetWeaponName;
        _bulletNum.text = _weapon.GetSetBulletNum.ToString();
    }
    //=========スライダー処理=========
    void SliderFunction()
    {
        _levelSlider.value = time;
        _dushSlider.value = _playerInfo.GetSetDush;
        _hp.value = _playerInfo.GetSetHP;
        if (_levelSlider.value >= _levelSlider.maxValue)//次のレベルまでのスライダー処理
        {
            _levelSlider.minValue = _levelSlider.maxValue;
            _levelSlider.maxValue *= 2;
            _level++;
        }
    }

     public void DamageFunction()
    {
        Color _color = new Color();
        _color = _damage.color;
        _color.a = 0.5f;
        _damage.color = _color;
        StartCoroutine(Ttransparency());
    }
    IEnumerator Ttransparency()
    {
        float f = 0;
        float t = 1;
        float add = (1.0f / 3.0f) * Time.deltaTime;
        while (t>=0)
        {
            f = Mathf.Lerp(0, 0.5f, t);
            Color _color = new Color();
            _color = _damage.color;
            _color.a = f;
            _damage.color = _color;
            t -= add;//無敵時間
            yield return new WaitForFixedUpdate();
        }
    }
}
