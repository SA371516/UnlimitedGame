using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TeamingManager : MonoBehaviour
{
    [SerializeField]
    Text _weaponName,_myPoint;
    [SerializeField]
    Text[] _addValume;
    [SerializeField]
    Text[] _nextValume;
    [SerializeField]
    EventSystem eventSystem;

    List<WeaponStatus> weaponStatuses = new List<WeaponStatus>();   //すべての武器のステータスが入っている
    int id;
    WeaponStatus _changeStatus;                                     //変えるステータスを入れる
    int _Point;
    int _LevelCount;
    bool _Exceeding_limited;
    int _addBulletNum;
    int _addATK;
    int _addAccuracy;
    int _addLimit;

    void Start()
    {
        _Point = PlayerData._Data._playerStatus.Point;
        weaponStatuses = PlayerData._Data._playerStatus.weaponStatuses;
        //===========最初はSR画面を表示させる==============
        _changeStatus = weaponStatuses.Find(Item => Item.WeaponName == "SR");
        id = weaponStatuses.FindIndex(Item => Item == _changeStatus);
        _LevelCount=_changeStatus.Levelcount;
        _Exceeding_limited = _changeStatus.Exceeding_limit;
        //===========加える変数を初期化====================
        ValumeReset();
        //=================================================
    }

    void Update()
    {
        //=================表示の部分==================
        _myPoint.text ="自分のポイント:"+ _Point.ToString();
        _addValume[0].text = _addBulletNum.ToString();
        _addValume[1].text = _addATK.ToString();
        _addValume[2].text = _addAccuracy.ToString();
        _addValume[3].text = _addLimit.ToString();
        //=============================================
    }
    //================加える変数のリセット=============
    void ValumeReset()
    {
        _Point = 0;
        _addBulletNum = 0;
        _addATK = 0;
        _addAccuracy = 0;
        _addLimit = 0;
    }

    #region ボタン入力時の処理
    //============OKボタンを入力時=====================
    public void ReflectValume()
    {
        _changeStatus.Exceeding_limit = _Exceeding_limited;
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
            case 0:
                if (_LevelCount % 10 == 0 && _Exceeding_limited) return;
                if (v.name == "Add")
                {
                    float f=_addBulletNum * 1.5f;
                    _addBulletNum = (int)f;
                    _changeStatus.BulletNum += _addBulletNum;
                    _LevelCount++;
                }
                else if (v.name == "Reduce" && _addBulletNum <= 0)
                {
                    _changeStatus.BulletNum -= _addBulletNum;
                    float f = _addBulletNum / 1.5f;
                    _addBulletNum = (int)f;
                    _LevelCount--;
                }
                break;
            case 1:
                if (_LevelCount % 10 == 0 && _Exceeding_limited) return;
                if (v.name == "Add")
                {

                }
                else if (v.name == "Reduce" && _addATK <= 0)
                {

                }
                break;
            case 2:
                if (_LevelCount % 10 == 0 && _Exceeding_limited) return;
                if (v.name == "Add")
                {

                }
                else if (v.name == "Reduce" && _addAccuracy <= 0)
                {

                }
                break;
            case 3:
                if (v.name == "Add")
                {

                }
                else if (v.name == "Reduce" && _addLimit <= 0)
                {

                }
                break;
        }
        if (_LevelCount%10==0)//10レベルごとに達した時
        {
            _Exceeding_limited = true;
        }
    }
    //=================================================
    #endregion
}
