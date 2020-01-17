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

    //==========一時的な保存変数===============
    List<WeaponStatus> weaponStatuses = new List<WeaponStatus>();   //すべての武器のステータスが入っている
    int id;                                                                                                     //反映させるときに必要//findは代入時のみ可能なため
    WeaponStatus _changeStatus,_oldStatus;                                     //変えるステータスを入れる//変更する前のステータス保存
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
        this.weaponStatuses = PlayerData._Data._playerStatus.weaponStatuses;
        //===========最初はSR画面を表示させる==============
        _changeStatus = new WeaponStatus(weaponStatuses.Find(Item => Item.WeaponName == "SR"));
        _oldStatus = new WeaponStatus(_changeStatus);
        id = weaponStatuses.FindIndex(Item => Item.WeaponName == _changeStatus.WeaponName);
        //===========加える変数を初期化====================
        ValumeReset();
        //=================================================
        //_Point = 10000;
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
    #region ボタン入力時の処理
    //================加える変数のリセット=============
    public void ValumeReset()
    {
        _Point = PlayerData._Data._playerStatus.Point;
        //武器のリセット
        _changeStatus = new WeaponStatus(weaponStatuses.Find(item => item.WeaponName == _changeStatus.WeaponName));
        _addBulletNum = _changeStatus.BulletNum;
        _addATK = _changeStatus.WeaponATK;
        _addAccuracy = _changeStatus.WeaponAccuracy;
        _LevelCount = _changeStatus.Levelcount;
        _addLimit = _changeStatus.ExceedingLevel;
        //必要なポイントのリセット
        _next[0] = _changeStatus.BulletNum / 10 * 5;
        _next[1] = (int)(_changeStatus.WeaponATK / 1.2) * 5;
        _next[2] = (int)(_changeStatus.WeaponAccuracy / 1.5) * 5;
        _next[3] = _changeStatus.ExceedingLevel * 3000;
    }
    //============OKボタンを入力時=====================
    public void ReflectValume()
    {
        _changeStatus.Levelcount = _LevelCount;
        weaponStatuses[id] = new WeaponStatus(_changeStatus);
        PlayerData._Data._playerStatus.Point = _Point;
        PlayerData._Data._playerStatus.weaponStatuses = this.weaponStatuses;
        _oldStatus = new WeaponStatus(_changeStatus);
    }
    //===========変更する武器を変える時================
    public void WeaponChangeButton(int i)
    {
        switch (i)
        {
            case (int)Weapons.SR:
                _weaponName.text = "SR";
                _changeStatus = new WeaponStatus(weaponStatuses.Find(Item => Item.WeaponName == "SR"));
                _oldStatus= new WeaponStatus(_changeStatus);
                id = weaponStatuses.FindIndex(Item => Item.WeaponName == _changeStatus.WeaponName);
                break;
            case (int)Weapons.AR:
                _weaponName.text = "AR";
                _changeStatus = new WeaponStatus(weaponStatuses.Find(Item => Item.WeaponName == "AR"));
                _oldStatus = new WeaponStatus(_changeStatus);
                id = weaponStatuses.FindIndex(Item => Item.WeaponName == _changeStatus.WeaponName);
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
                if (v.name == "Reduce" && _addBulletNum > _oldStatus.BulletNum)
                {
                    _addBulletNum -= 10;
                    _changeStatus.BulletNum = _addBulletNum;
                    _next[0] = _changeStatus.BulletNum / 10 * 5;
                    _Point += _next[0];
                    _LevelCount--;
                }
                else if (_LevelCount == _addLimit * 10) return;  //レベル10ごとに達した時
                else if (v.name == "Add" && _Point > _next[0])
                {
                    _Point -= _next[0];
                    _addBulletNum += 10;
                    _changeStatus.BulletNum = _addBulletNum;
                    _next[0] = _changeStatus.BulletNum / 10 * 5;
                    _LevelCount++;
                }
                break;
            case 1://攻撃力が1.2倍になっていく
                if (v.name == "Reduce" && _addATK > _oldStatus.WeaponATK)
                {
                    _addATK = _addATK / 1.2f;
                    _changeStatus.WeaponATK = _addATK;
                    _next[1] = (int)(_changeStatus.WeaponATK / 1.2) * 5;
                    _Point += _next[1];
                    _LevelCount--;
                }
                else if (_LevelCount == _addLimit * 10) return;//レベル10ごとに達した時
                else if (v.name == "Add" && _Point > _next[1])
                {
                    _Point -= _next[1];
                    _addATK = _addATK * 1.2f;           //何も強化していないと「0」なため
                    _changeStatus.WeaponATK = _addATK;
                    _next[1] = (int)(_changeStatus.WeaponATK / 1.2) * 5;
                    _LevelCount++;
                }
                break;
            case 2://精度は1.5倍にする
                if (v.name == "Reduce" && _addAccuracy > _oldStatus.WeaponAccuracy)
                {
                    _addAccuracy = _addAccuracy / 1.5f;
                    _changeStatus.WeaponAccuracy = _addAccuracy;
                    _next[2] = (int)(_changeStatus.WeaponAccuracy / 1.5) * 5;
                    _Point += _next[2];
                    _LevelCount--;
                }
                else if (_LevelCount == _addLimit * 10) return;//レベル10ごとに達した時
                else if (v.name == "Add" && _Point > _next[2])
                {
                    _Point -= _next[2];
                    _addAccuracy = _addAccuracy * 1.5f;
                    _changeStatus.WeaponAccuracy = _addAccuracy;
                    _next[2] = (int)(_changeStatus.WeaponAccuracy / 1.5) * 5;
                    _LevelCount++;
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
    }
    //=================================================
    public void SceneMove(int i)
    {
        SceneLoadManager._loadManager.SceneLoadFunction(i);
    }
    #endregion
}
