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
    public BaseWeapon _weapon;
    BasePlayer _playerInfo;
    private void Start()
    {
        _playerInfo = GameObject.FindWithTag("Player").GetComponent<BasePlayer>();
        _hp.maxValue = _playerInfo.GetSetHP;
    }

    void Update()
    {
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
