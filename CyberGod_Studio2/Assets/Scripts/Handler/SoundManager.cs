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
   */
   


public class SoundManager : MonosingletonTemp<SoundManager>
{
    // Start is called before the first frame update
    public AudioSource musicSource;
    public AudioSource sfxSource;
    
    // 添加一个新的字段来存储所有的 AudioSource
    [SerializeField]
    public List<AudioSource> audioSources;
    
    //初始化一个AudioClipList
    public List<AudioClip> AudioClipList = new List<AudioClip>();
    
    public List<AudioClip> MusicClipList = new List<AudioClip>();
    
    private bool isAudioFadeActive = false; // 新增一个布尔变量来控制音频是否应该持续播放
    private int currentAudioClipIndex = -1; // 当前播放的音频剪辑的索引
    private Coroutine audioFadeTransitionCoroutine; // 用于存储渐变音量的协程
    private int activeRequests = 0; // 新增一个计数器来跟踪在一帧中有多少个函数调用ControlAudioFadeTransition并传入true

    
    
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
    
    public void PlaySFX(int index, float volume = 1f)
    {
        AudioClip clip = AudioClipList[(int)index];
        sfxSource.clip = clip;
        sfxSource.volume = volume;
        sfxSource.PlayOneShot(clip);
    }
    
    public void PlaySFXOnce(int index, float volume = 1f)
    {
        //如果是相同音频在播放，那么什么都不做
        if (sfxSource.clip == AudioClipList[(int)index])
        {
            return;
        }
        AudioClip clip = AudioClipList[(int)index];
        sfxSource.clip = clip;
        sfxSource.volume = volume;
        sfxSource.Play();
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
    
    public void ControlAudioFadeTransition(bool shouldPlay, int clipIndex, float volume, float fadeTime)
    {
        if (shouldPlay)
        {
            activeRequests++;
            isAudioFadeActive = true;
            currentAudioClipIndex = clipIndex;
            if (audioFadeTransitionCoroutine != null)
            {
                StopCoroutine(audioFadeTransitionCoroutine);
            }
            audioFadeTransitionCoroutine = StartCoroutine(PlayAudioWithFadeTransitionCoroutine(AudioClipList[clipIndex], volume, fadeTime));
        }
        else
        {
            activeRequests--;
            if (activeRequests <= 0)
            {
                isAudioFadeActive = false;
                if (audioFadeTransitionCoroutine != null)
                {
                    StopCoroutine(audioFadeTransitionCoroutine);
                }
                audioFadeTransitionCoroutine = StartCoroutine(FadeOutAudioTransitionCoroutine(fadeTime, volume));
            }
        }
    }

    private IEnumerator PlayAudioWithFadeTransitionCoroutine(AudioClip clip, float volume, float fadeTime)
    {
        while (isAudioFadeActive && AudioClipList[currentAudioClipIndex] == clip)
        {
            sfxSource.clip = clip;
            sfxSource.loop = true;
            sfxSource.volume = 0f;
            sfxSource.Play();

            // Fade in
            float startTime = Time.time;
            while (Time.time < startTime + fadeTime)
            {
                sfxSource.volume = (Time.time - startTime) / fadeTime * volume;
                yield return null;
            }
            sfxSource.volume = volume;

            // Wait for the clip to finish playing
            yield return new WaitForSeconds(clip.length - fadeTime);
        }
    }

    private IEnumerator FadeOutAudioTransitionCoroutine(float fadeTime, float volume)
    {
        // Fade out
        float startTime = Time.time;
        while (Time.time < startTime + fadeTime)
        {
            sfxSource.volume = volume - ((Time.time - startTime) / fadeTime * volume);
            yield return null;
        }
        sfxSource.volume = 0f;
        sfxSource.Stop();
    }
    
    
    // 添加一个新的方法来开启指定的 AudioSource
    public void EnableAudioSource(int index, bool enable)
    {
        if (index >= 0 && index < audioSources.Count)
        {
            audioSources[index].enabled = enable;
        }
        else
        {
            Debug.LogError("Index out of range. Please make sure the index is within the range of the audioSources list.");
        }
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
        // 在每一帧的开始，重置计数器
        activeRequests = 0;
    }
}
