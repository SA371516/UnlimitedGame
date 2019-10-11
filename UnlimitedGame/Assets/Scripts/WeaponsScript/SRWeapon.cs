using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SRWeapon : BaseWeapon
{
    float _nowTime;
    public SRWeapon()
    {
        _recustTime = 5f;
        _nowTime = 5f;
        _distance = 30f;
        _recustSlider.maxValue = _recustTime;
    }
    public override void Update()
    {
        if(_nowTime>=_recustTime) Attack();//次の弾を撃つラグ

        _nowTime += Time.deltaTime;
        _recustSlider.value = _nowTime;
    }

    public override void Attack()
    {
        Ray ray = GetRay();
        //単発銃、射程距離30ｍ
        if (Input.GetMouseButtonDown(0)&&Physics.Raycast(ray,out hit,_distance))
        {
            if (hit.collider.gameObject.tag == "Enemy")
            {
                Debug.Log("あったった");
            }
            _nowTime = 0f;

        }
    }
}
