using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    public static SoundManager _soundManager;
    AudioSource _bgmSound;
    [SerializeField]
    AudioSource _seSound, _moveSound;
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
        _seSound = p.GetComponent<AudioSource>();
        _moveSound = p.transform.GetChild(1).GetComponent<AudioSource>();
    }

    public void PlayBGMSound(BGM b)
    {
        _bgmSound.clip = BGMClip[(int)b];
        _bgmSound.Play();
    }
    public void PlaySESound(SE s, int id, float delayTime = 0f)
    {
        switch (id)
        {
            case 0:
                if (!_seSound.isPlaying || oldSE != s)
                {
                    oldSE = s;
                    _seSound.clip = SEClip[(int)s];
                    _seSound.Play();
                }
                break;
            case 1:
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
                _seSound.Stop();
                break;
            case 1:
                _moveSound.Stop();
                break;
        }
    }
}
