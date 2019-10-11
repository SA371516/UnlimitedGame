﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARWeapon : BaseWeapon
{
    float _nowTime;
    public ARWeapon()
    {
        _weaponName = "AR";
        _bulletNum = 30;
        _recustTime = 0.25f;
        _nowTime = 0f;
        _distance = 15f;
        _recustSlider.maxValue = _recustTime;
    }
    public override void Update()
    {
        if (_bulletNum <= 0) return;
        if (Input.GetMouseButton(0)&&_nowTime>=_recustTime) Attack();//次の弾を撃つラグ
        _nowTime += Time.deltaTime;
        _recustSlider.value = _nowTime;

    }
    public override void Attack()
    {
        _nowTime = 0f;
        _bulletNum--;
        Ray ray = GetRay();
        //連射銃、射程距離15ｍ
        if (Physics.Raycast(ray, out hit, _distance))
        {
            if (hit.collider.gameObject.tag == "Enemy")
            {
                Debug.Log("あったった");
            }
        }
    }

}
