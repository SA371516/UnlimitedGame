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
    Text[] _addValume, nextValume;
    [SerializeField]
    EventSystem eventSystem;

    List<WeaponStatus> weaponStatuses = new List<WeaponStatus>();   //すべての武器のステータスが入っている
    int id;                                                         //反映させるときに必要//findは代入時のみ可能なため
    WeaponStatus _changeStatus;                                     //変えるステータスを入れる
    int _Point;
    int _LevelCount;
    int[] _next = new int[4];
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
        string str;
        //=================表示の部分==================
        _weaponName.text = _changeStatus.WeaponName;
        _myPoint.text ="自分のポイント:"+ _Point.ToString();
        _weaponLevel.text = "武器のレベル:" + _LevelCount.ToString();
        _addValume[0].text = _addBulletNum.ToString();
        str = String.Format("{0:#.##}", _addATK);
        _addValume[1].text = str;
        str = String.Format("{0:#.##}", _addAccuracy);
        _addValume[2].text = str;
        _addValume[3].text = _addLimit.ToString();
        //============次の値のためのポイント計算=======
        nextValume[0].text = "Next:" + _next[0];
        nextValume[1].text = "Next:" + _next[1];
        nextValume[2].text = "Next:" + _next[2];
        nextValume[3].text = "Next:" + _next[3];
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
                if (v.name == "Add" && _Point > _next[0])
                {
                    _Point -= _next[0];
                    _addBulletNum += 10;
                    _changeStatus.BulletNum = _addBulletNum;
                    _next[0] = _changeStatus.BulletNum / 10 * 5;
                    _LevelCount++;
                }
                else if (v.name == "Reduce" && _addBulletNum > 0)
                {
                    _addBulletNum -= 10;
                    _changeStatus.BulletNum = _addBulletNum;
                    _next[0] = _changeStatus.BulletNum / 10 * 5;
                    _Point += _next[0];
                    _LevelCount--;
                }
                break;
            case 1://攻撃力が1.2倍になっていく
                if (_LevelCount == _addLimit * 10) return;//レベル10ごとに達した時
                if (v.name == "Add" && _Point > _next[1])
                {
                    _Point -= _next[1];
                    _addATK = _addATK * 1.2f;//何も強化していないと「0」なため
                    _changeStatus.WeaponATK += _addATK;
                    _next[1] = (int)(_changeStatus.WeaponATK / 1.2) * 5;
                    _LevelCount++;
                }
                else if (v.name == "Reduce" && _addATK > 1)
                {
                    _changeStatus.WeaponATK -= _addATK;
                    _addATK = _addATK / 1.2f;
                    _next[1] = (int)(_changeStatus.WeaponATK / 1.2) * 5;
                    _Point += _next[1];
                    _LevelCount--;
                }
                break;
            case 2://精度は1.5倍にする
                if (_LevelCount == _addLimit * 10) return;//レベル10ごとに達した時
                if (v.name == "Add" && _Point > _next[2])
                {
                    _Point -= _next[2];
                    _addAccuracy = _addAccuracy * 1.5f;
                    _changeStatus.WeaponAccuracy += _addAccuracy;
                    _next[2] = (int)(_changeStatus.WeaponAccuracy / 1.5) * 5;
                    _LevelCount++;
                }
                else if (v.name == "Reduce" && _addAccuracy > 1)
                {
                    _changeStatus.WeaponAccuracy -= _addAccuracy;
                    _addAccuracy = _addAccuracy / 1.5f;
                    _next[2] = (int)(_changeStatus.WeaponATK / 1.2) * 5;
                    _Point += _next[2];
                    _LevelCount--;
                }
                break;
            case 3://上限解放の部分は特殊なので注意！！//一度したら戻せない
                if (v.name == "Add"&& _LevelCount == _addLimit * 10 && _Point > _next[3])
                {
                    _Point -= _next[3];
                    _addLimit++;
                    _changeStatus.ExceedingLevel += _addLimit;
                    _next[3] = _changeStatus.ExceedingLevel * 3000;
                    _LevelCount++;
                }
                break;
        }
        if (_LevelCount == _addLimit * 10) ;
    }
    //=================================================
    public void SceneMove(int i)
    {
        SceneLoadManager._loadManager.SceneLoadFunction(i);
    }
    #endregion
}
