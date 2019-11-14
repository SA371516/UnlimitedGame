﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SRWeapon : BaseWeapon
{
    float _nowTime;
    public SRWeapon(GameObject p):base(p)
    {
        _accuracy = 5;
        _Damage = 5;
        _weaponName = "SR";
        _bulletNum = 10;
        _recustTime = 2f;
        _nowTime = 5f;
        _distance = 40f;
        _recustSlider.maxValue = _recustTime;

    }
    public override void Update()
    {
        if (_bulletNum <= 0) {
            particleScr.StopParticle();
            return;
        }
        if (_nowTime >= _recustTime && Input.GetMouseButtonDown(0)) Attack();//次の弾を撃つラグ
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
        particleScr.PlayParticle();
        //単発銃、射程距離30ｍ
        if(Physics.Raycast(ray, out hit, _distance))
        {
            if (hit.collider.gameObject.tag == "Enemy")
            {
                BaseEnemy _enemy = hit.collider.GetComponent<BaseEnemy>();
                _enemy.GetSetHP -= _Damage;
                _manager.ObjectInctance(_particle,hit.point);
            }
            else
            {
                Debug.Log("!!!!!!!!!!!!!!!!!!!!!!"+hit.collider.gameObject.name);
            }
        }
    }
}
