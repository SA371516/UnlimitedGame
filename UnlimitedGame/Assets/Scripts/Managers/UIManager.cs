using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    Text _weaponName, _bulletNum;
    [SerializeField]
    Slider  _hp;
    [SerializeField]
    Text _gameTime,_scoreText;
    public BaseWeapon _weapon;
    BasePlayer _playerInfo;
    GameManager _manager;
    float time = 0;

    public bool _stop;
    private void Start()
    {
        _playerInfo = GameObject.FindWithTag("Player").GetComponent<BasePlayer>();
        _manager = GameObject.Find("Manager").GetComponent<GameManager>();
        _hp.maxValue = _playerInfo.GetSetHP;
        _stop = false;
    }

    void Update()
    {
        if (_stop) return;
        time = time+ Time.deltaTime;
        float _t = Mathf.Floor(time);
        string str = string.Format("{00}", _t.ToString());
        _gameTime.text = str;
        _scoreText.text ="Score:"+ _manager.GetSetScore.ToString();
        _hp.value = _playerInfo.GetSetHP;
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
