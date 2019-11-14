using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARWeapon : BaseWeapon
{
    float _nowTime;
    public ARWeapon(GameObject p):base(p)
    {
        _accuracy = 70;
        _weaponName = "AR";
        _bulletNum = 50;
        _recustTime = 0.05f;
        _nowTime = 0f;
        _distance = 15f;
        _Damage = 1;
        _recustSlider.maxValue = _recustTime;
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
            if (hit.collider.gameObject.tag == "Enemy")
            {
                BaseEnemy _enemy = hit.collider.GetComponent<BaseEnemy>();
                _enemy.GetSetHP -= _Damage;
                _manager.ObjectInctance(_particle, hit.point);

            }
            else
            {
                Debug.Log("!!!!!!!!!!!!!!!!!!!!!!" + hit.collider.gameObject.name);
            }
        }
    }

}
