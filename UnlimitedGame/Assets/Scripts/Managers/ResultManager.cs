using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour
{
    [SerializeField]
    Text _scoreText, _wordText, _enterText;
    [SerializeField]
    Sprite[] _wordBack;
    [SerializeField]
    Image _wordBackImg;
    string[] _wordStr = new string[]
    {
        "もう少し頑張りましょう",
        "さすがの戦果です！！",
        "お、恐ろしい子..."
    };

    int _resultScore;
    float _time;
    float _changeTransparency = 1f;
    float _changeTime;
    float _interval = 1f;

    // Start is called before the first frame update
    void Start()
    {
        _resultScore = SceneLoadManager._loadManager._score;
        //_resultScore = 150000;
        Color c = new Color(1, 0, 0, 0);
        _enterText.color = c;
    }

    // Update is called once per frame
    void Update()
    {
        _scoreText.text = "Score:" + _resultScore.ToString();
        //1========スコアに応じて表示するものを変更する======
        if (_resultScore >= 0 && _resultScore <= 1000)
        {
            _wordBackImg.sprite = _wordBack[0];
            _wordText.text = _wordStr[0];
        }
        else if (_resultScore >= 1100 && _resultScore <= 15000)
        {
            _wordBackImg.sprite = _wordBack[1];
            _wordText.text = _wordStr[1];
        }
        else if (_resultScore >= 15100)
        {
            _wordBackImg.sprite = _wordBack[2];
            _wordText.text = _wordStr[2];
        }
        //==============================================
        if (5.0f < Time.time)
        {
            TextColorChange();
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SceneLoadManager._loadManager.SceneLoadFunction((int)SceneLoadManager.Scenes.Title,true);
        }
    }
    void TextColorChange()//ここでテキストのフェードイン、アウトをしている
    {
        //一定期間で表示・非表示
        _changeTime += Time.deltaTime * _changeTransparency;
        if (_changeTime >= _interval) { _changeTransparency = -1; _changeTime = _interval; }
        if (_changeTime <= 0) { _changeTransparency = 1; _changeTime = 0; }
        Color color = _enterText.color;
        color.a = _changeTime / _interval;
        _enterText.color = color;
    }
}
