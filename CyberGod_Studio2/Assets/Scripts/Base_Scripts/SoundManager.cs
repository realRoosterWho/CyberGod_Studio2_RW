using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Video;

/*
   添加脚本：
   把 SoundManager 脚本附加到Unity场景中的一个GameObject上。
   
   自动创建音源：
   脚本会自动生成两个AudioSource组件：musicSource（音乐）和sfxSource（音效）。
   
   播放音乐：
   用 PlayMusic(AudioClip clip, float volume = 1f, bool loop = true) 播放背景音乐。
   
   播放音效：
   用 PlaySFX(AudioClip clip, float volume = 1f) 播放一次性音效。
   
   启用/禁用音效：
   EnableSFX(AudioClip clip, AudioMixerGroup mixer = null) 启用音效。
   DisableSFX() 禁用音效播放。
   
   在其他脚本中调用：
   要在别的脚本中调用这些功能，使用 SoundManager.Instance 获取单例实例，然后调用所需的方法，例如：
   SoundManager.Instance.PlayMusic(clip, volume, loop); 播放音乐
   SoundManager.Instance.PlaySFX(clip, volume); 播放音效
   SoundManager.Instance.EnableSFX(clip, mixer); 启用音效
   SoundManager.Instance.DisableSFX(); 禁用音效
   * /
   


public class SoundManager : MonosingletonTemp<SoundManager>
{
    // Start is called before the first frame update
    public AudioSource musicSource;
    public AudioSource sfxSource;
    
    //初始化一个AudioClipList
    public List<AudioClip> AudioClipList = new List<AudioClip>();
    
    public List<AudioClip> MusicClipList = new List<AudioClip>();
    
    public void Init()
    {
        Debug.Log("SoundManager Init");
    }
    public void PlayMusic(AudioClip clip, float volume = 1f, bool loop = true)
    {
		//查看当前正在播放的音乐和要播放的音乐是否相同
		if (musicSource.clip == clip)
        {
            return;
        }
		
        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.volume = volume;
        musicSource.Play();
    }
    
    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        sfxSource.clip = clip;
        sfxSource.volume = volume;
        sfxSource.PlayOneShot(clip);
    }
    
    public void EnableSFX(AudioClip clip, AudioMixerGroup mixer = null)
    {
        sfxSource.clip = clip;
        sfxSource.outputAudioMixerGroup = mixer;
        // 如果在播放，什么都不做，否则播放
        if (sfxSource.isPlaying)
        {
            return;
        }
        sfxSource.Play();
        sfxSource.enabled = true;
    }
    
    public void DisableSFX()
    {
        sfxSource.enabled = false;
    }

    private void Start()
    {
        
        //给自己添加一个AudioSource组件
        musicSource = gameObject.AddComponent<AudioSource>();
        sfxSource = gameObject.AddComponent<AudioSource>();
        
        //播放背景音乐
        PlayMusic(MusicClipList[0]);
    }

    private void Update()
    {

    }
}
