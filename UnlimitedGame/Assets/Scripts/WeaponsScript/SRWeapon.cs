using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SRWeapon : BaseWeapon
{
    float _nowTime;
    public SRWeapon(GameObject p):base(p)
    {
        //================武器の初期化==================
        _recustTime = 2f;
        _nowTime = 5f;
        _distance = 40f;
        particleScr._stopTime = 0.18f;
        _recustSlider.maxValue = _recustTime;
        //===============武器のステータス反映===========
        _status = PlayerData._Data._playerStatus.weaponStatuses.Find(Item => Item.WeaponName == "SR");
        _accuracy = _status.WeaponAccuracy;
        _Damage = _status.WeaponATK;
        _weaponName = _status.WeaponName;
        _bulletNum = _status.BulletNum;
        //==============================================

    }
    public override void Update()
    {
        if (_bulletNum <= 0) {
            particleScr.StopParticle();
            return;
        }
        if (_nowTime >= _recustTime && Input.GetMouseButtonDown(0)) Attack();//次の弾を撃つラグ
        else if(_nowTime<=_recustTime)SoundManager._soundManager.PlaySESound(SoundManager.SE.SRReload,0,2f);
        _nowTime += Time.deltaTime;
        _recustSlider.value = _nowTime;
    }
    protected override void Attack()
    {
        _nowTime = 0f;
        _bulletNum--;
        Ray ray = GetBulletItem(_accuracy);
        particleScr.PlayParticle();
        //単発銃、射程距離30ｍ
        if(Physics.Raycast(ray, out hit, _distance))
        {
            _manager._shotNum++;
            SoundManager._soundManager.PlaySESound(SoundManager.SE.SRShot,0);
            if (hit.collider.gameObject.tag == "Enemy")
            {
                BaseEnemy _enemy = hit.collider.GetComponent<BaseEnemy>();
                _enemy.HitFunction(hit.point, _Damage);
                _manager._hitNum++;
            }
            else
            {
                Debug.Log("!!!!!!!!!!!!!!!!!!!!!!"+hit.collider.gameObject.name);
            }
        }
    }
}
