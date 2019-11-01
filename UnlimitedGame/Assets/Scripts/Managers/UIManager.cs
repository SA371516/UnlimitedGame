using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    Text _weaponName, _bulletNum,_levelText;
    [SerializeField]
    Slider  _hp,_levelSlider;
    [SerializeField]
    Text _gameTime,_scoreText;
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
        _stop = false;
    }

    void Update()
    {
        if (_stop) return;
        time = time+ Time.deltaTime;
        float _t = Mathf.Floor(time);
        string str = string.Format("{00}", _t.ToString());
        _gameTime.text = str;
        str = "Level" + _level.ToString();
        _levelText.text = str;
        _levelSlider.value = time;
        _scoreText.text ="Score:"+ _manager.GetSetScore.ToString();
        _hp.value = _playerInfo.GetSetHP;
        if (_levelSlider.value >= _levelSlider.maxValue)
        {
            _levelSlider.minValue = _levelSlider.maxValue;
            _levelSlider.maxValue *= 2;
            _level++;
        }
        //武器アイコン処理
        if (_weapon == null) {
            _weaponName.text = "NotWeapon";
            _bulletNum.text = "0";
            return;
        }
        _weaponName.text = _weapon.GetSetWeaponName;
        _bulletNum.text = _weapon.GetSetBulletNum.ToString();
    }
}
