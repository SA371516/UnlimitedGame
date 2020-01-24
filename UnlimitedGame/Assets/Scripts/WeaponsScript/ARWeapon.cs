using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ARWeapon : BaseWeapon
{
    float _nowTime;
    public ARWeapon(GameObject p):base(p)
    {
        //=========基本ステータス=========
        _recustTime = 0.2f;
        _nowTime = 0f;
        _distance = 15f;
        particleScr._stopTime = 0.09f;
        _recustSlider.maxValue = _recustTime;
        //====武器ステータスを反映======
        _status = PlayerData._Data._playerStatus.weaponStatuses.Find(Item => Item.WeaponName == "AR");
        _weaponName = _status.WeaponName;
        _accuracy += _status.WeaponAccuracy;
        _Damage += _status.WeaponATK;
        _bulletNum += _status.BulletNum;
        //=================================
    }
    public override void Update()
    {
        if (_bulletNum <= 0)
        {
            particleScr.StopParticle();
            return;
        }
        if (Input.GetMouseButton(0) && _nowTime >= _recustTime) Attack();//次の弾を撃つラグ
        else if (!Input.GetMouseButton(0))
            particleScr.StopParticle();
        _nowTime += Time.deltaTime;
        _recustSlider.value = _nowTime;

    }
    protected override void Attack()
    {
        _nowTime = 0f;
        _bulletNum--;
        Ray ray = GetBulletItem(_accuracy);
        //if (!_bulletParticle.isPlaying) _bulletParticle.Play();
        particleScr.PlayParticle();
        //連射銃、射程距離15ｍ
        if (Physics.Raycast(ray, out hit, _distance))
        {
            _manager._shotNum++;
            if (hit.collider.gameObject.tag == "Enemy")
            {
                BaseEnemy _enemy = hit.collider.GetComponent<BaseEnemy>();
                _enemy.HitFunction(hit.point, _Damage);
                _manager._hitNum++;
            }
            else
            {
                Debug.Log("!!!!!!!!!!!!!!!!!!!!!!" + hit.collider.gameObject.name);
            }
        }
    }

}
