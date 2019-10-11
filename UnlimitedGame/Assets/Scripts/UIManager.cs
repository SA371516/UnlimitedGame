using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    Text _weaponName, _bulletNum;
    public BaseWeapon _weapon;
    // Update is called once per frame
    void Update()
    {
        if (_weapon == null) {
            _weaponName.text = "NotWeapon";
            _bulletNum.text = "0";
            return;
        }
        _weaponName.text = _weapon.GetSetWeaponName;
        _bulletNum.text = _weapon.GetSetBulletNum.ToString();
    }
}
