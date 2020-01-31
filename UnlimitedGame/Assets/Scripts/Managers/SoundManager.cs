using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    public static SoundManager _soundManager;
    AudioSource _bgmSound;
    [SerializeField]
    AudioSource  _moveSound;
    AudioSource[] _gunSound=new AudioSource[2];
    public enum BGM
    {
        Title=0,
        Result=1,
        Teaming=2
    }
    public enum SE
    {
        ARShot=0,
        SRShot=1,
        SRReload=2,
        Walk=3,
        Dush=4,
        None=9999
    }

    [SerializeField]
    AudioClip[] BGMClip, SEClip;

    bool activeChack;
    public bool GetSetActive
    {
        get { return activeChack; }
        set { activeChack = value; }
    }
    SE oldSE = SE.None;

    private void Awake()
    {
        if (_soundManager == null)
        {
            _soundManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        _bgmSound = GetComponent<AudioSource>();
    }
    public void GameSetInit()
    {
        GameObject p = GameObject.FindWithTag("Player");
        _gunSound[0] = p.GetComponent<AudioSource>();
        _gunSound[1] = p.transform.GetChild(1).GetComponent<AudioSource>();
        _moveSound = p.transform.GetChild(2).GetComponent<AudioSource>();
    }

    public void PlayBGMSound(BGM b)
    {
        _bgmSound.clip = BGMClip[(int)b];
        _bgmSound.Play();
    }
    public void StopBGM()
    {
        _bgmSound.Stop();
    }
    public void PlaySESound(SE s, int id,float delay=0f)
    {
        switch (id)
        {
            case 0:
                Debug.Log(_gunSound[id].isPlaying);
                if (!_gunSound[id].isPlaying)
                {
                    _gunSound[id].clip = SEClip[(int)s];
                    _gunSound[id].Play();
                }
                break;
            case 1:
                if (!_gunSound[id].isPlaying)
                {
                    _gunSound[id].clip = SEClip[(int)s];
                    _gunSound[id].PlayDelayed(delay);
                }
                break;
            case 2://走る状態と歩く状態があるから
                if (!_moveSound.isPlaying || oldSE != s)
                {
                    oldSE = s;
                    _moveSound.clip = SEClip[(int)s];
                    _moveSound.Play();
                }
                break;
        }
    }

    public void StopSESound(int id)
    {
        switch (id)
        {
            case 0:
                _gunSound[id].Stop();
                break;
            case 1:
                _gunSound[id].Stop();
                break;
            case 2:
                _moveSound.Stop();
                break;
        }
    }
}
