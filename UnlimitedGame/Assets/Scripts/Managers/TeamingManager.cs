using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TeamingManager : MonoBehaviour
{
    [SerializeField]
    Text _weaponName, _myPoint, _weaponLevel;
    [SerializeField]
    Text[] _addValume;
    [SerializeField]
    Text[] _nextValume;
    [SerializeField]
    EventSystem eventSystem;

    List<WeaponStatus> weaponStatuses = new List<WeaponStatus>();   //すべての武器のステータスが入っている
    int id;                                                         //反映させるときに必要//findは代入時のみ可能なため
    WeaponStatus _changeStatus;                                     //変えるステータスを入れる
    int _Point;
    int _LevelCount;
    bool _Exceeding_limited;
    //=========表示させるための変数============
    int _addBulletNum;
    float _addATK;
    float _addAccuracy;
    int _addLimit;


    void Start()
    {
        if (PlayerData._Data._debug)
        {
            var v = PlayerData._Data.CreateUserData();
            PlayerData._Data._playerStatus = v;
        }
        _Point = PlayerData._Data._playerStatus.Point;
        weaponStatuses = PlayerData._Data._playerStatus.weaponStatuses;
        //===========最初はSR画面を表示させる==============
        _changeStatus = weaponStatuses.Find(Item => Item.WeaponName == "SR");
        id = weaponStatuses.FindIndex(Item => Item == _changeStatus);
        _LevelCount=_changeStatus.Levelcount;
        //===========加える変数を初期化====================
        ValumeReset();
        //=================================================
    }

    void Update()
    {
        //=================表示の部分==================
        _myPoint.text ="自分のポイント:"+ _Point.ToString();
        _weaponLevel.text = "武器のレベル:" + _LevelCount.ToString();
        _addValume[0].text = _addBulletNum.ToString();
        _addValume[1].text = _addATK.ToString();
        _addValume[2].text = _addAccuracy.ToString();
        _addValume[3].text = _addLimit.ToString();
        //=============================================
    }
    //================加える変数のリセット=============
    void ValumeReset()
    {
        _Point = PlayerData._Data._playerStatus.Point;
        _addBulletNum = _changeStatus.BulletNum;
        _addATK = _changeStatus.WeaponATK;
        _addAccuracy = _changeStatus.WeaponAccuracy;
        _addLimit = _changeStatus.ExceedingLevel;
    }
    #region ボタン入力時の処理
    //============OKボタンを入力時=====================
    public void ReflectValume()
    {
        _changeStatus.Levelcount = _LevelCount;
        weaponStatuses[id] = _changeStatus;
        PlayerData._Data._playerStatus.Point = _Point;
        PlayerData._Data._playerStatus.weaponStatuses = this.weaponStatuses;
    }
    //===========変更する武器を変える時================
    public void WeaponChangeButton(int i)
    {
        switch (i)
        {
            case (int)Weapons.SR:
                _weaponName.text = "SR";
                _changeStatus = weaponStatuses.Find(Item => Item.WeaponName == "SR");
                id = weaponStatuses.FindIndex(Item => Item == _changeStatus);
                break;
            case (int)Weapons.AR:
                _weaponName.text = "AR";
                _changeStatus = weaponStatuses.Find(Item => Item.WeaponName == "AR");
                id = weaponStatuses.FindIndex(Item => Item == _changeStatus);
                break;
        }
        ValumeReset();
    }
    //=============値を変更するとき====================
    //引数(どの値を変更するか)//0=>弾数,1=>ATK,2=>Accuracy,3=>Exceeding the limit
    public void ValumeChangeButton(int i)
    {
        var v = eventSystem.currentSelectedGameObject.gameObject;       //クリックされたものを取得
        switch (i)
        {
            case 0://弾は10発ずつ増えていく
                if (_LevelCount == _addLimit * 10) return;  //レベル10ごとに達した時
                if (v.name == "Add")
                {
                    _addBulletNum += 10;
                    _changeStatus.BulletNum = _addBulletNum;
                    _LevelCount++;
                }
                else if (v.name == "Reduce" && _addBulletNum > 0)
                {
                    _addBulletNum -= 10;
                    _changeStatus.BulletNum = _addBulletNum;
                    _LevelCount--;
                }
                break;
            case 1://攻撃力が1.2倍になっていく
                if (_LevelCount == _addLimit * 10) return;//レベル10ごとに達した時
                if (v.name == "Add")
                {
                    _addATK =  _addATK * 1.2f;//何も強化していないと「0」なため
                    _changeStatus.WeaponATK += _addATK;
                    _LevelCount++;
                }
                else if (v.name == "Reduce" && _addATK > 1)
                {
                    _changeStatus.WeaponATK -= _addATK;
                    _addATK= _addATK / 1.2f;
                    _LevelCount--;
                }
                break;
            case 2://精度は1.5倍にする
                if (_LevelCount == _addLimit * 10) return;//レベル10ごとに達した時
                if (v.name == "Add")
                {
                    _addAccuracy = _addAccuracy * 1.5f;
                    _changeStatus.WeaponAccuracy += _addAccuracy;
                    _LevelCount++;
                }
                else if (v.name == "Reduce" && _addAccuracy > 1)
                {
                    _changeStatus.WeaponAccuracy -= _addAccuracy;
                    _addAccuracy =  _addAccuracy / 1.5f;
                    _LevelCount--;
                }
                break;
            case 3://上限解放の部分は特殊なので注意！！//一度したら戻せない
                if (v.name == "Add"&& _LevelCount == _addLimit * 10)
                {
                    _addLimit++;
                    _changeStatus.ExceedingLevel += _addLimit;
                    _LevelCount++;
                }
                break;
        }
        if (_LevelCount == _addLimit * 10) ;
    }
    //=================================================
    #endregion
}
