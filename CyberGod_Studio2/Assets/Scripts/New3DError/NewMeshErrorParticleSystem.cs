using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewMeshErrorParticleSystem : MonoBehaviour
{
    // 将 ParticleSystem 组件拖到 Inspector 面板中
    private ParticleSystem m_particleSystem;
    private GameObject m_parent;

    // Start is called before the first frame update
    void Start()
    {
        m_parent = transform.parent.gameObject;
        m_particleSystem = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.identity;
    }

    // 调整粒子系统的持续时间
    public void SetDuration(float duration)
    {
        var main = m_particleSystem.main;
        main.duration = duration;
    }

    // 调整粒子系统的开始生命期
    public void SetStartLifetime(float lifetime)
    {
        var main = m_particleSystem.main;
        main.startLifetime = lifetime;
    }

    // 设置粒子系统是否循环播放
    public void SetLooping(bool isLooping)
    {
        var main = m_particleSystem.main;
        main.loop = isLooping;
    }

    // 开始播放粒子系统
    public void PlayParticleSystem()
    {
        // 如果粒子系统已经在播放，那么就不再调用Play方法
        if (!m_particleSystem.isPlaying)
        {
            m_particleSystem.Play();
        }
    }

    // 暂停粒子系统
    public void PauseParticleSystem()
    {
        m_particleSystem.Pause();
    }

    // 停止粒子系统
    public void StopParticleSystem()
    {
        m_particleSystem.Stop();
    }
}