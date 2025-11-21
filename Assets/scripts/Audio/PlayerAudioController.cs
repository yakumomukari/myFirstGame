using UnityEngine;

namespace Game.Audio
{
    /// <summary>
    /// 管理挂在玩家身上的短音效（PlayOneShot）和循环音效（如滑铲）.
    /// 使用方式：添加到玩家 GameObject 并在 Inspector 中设置 AudioClips。
    /// 在移动代码/动画事件中调用 PlayFootstep(), PlaySlide(true/false), PlayHurt().
    /// </summary>
    public class PlayerAudioController : MonoBehaviour
    {
        [Header("Audio Sources (optional)")]
        [Tooltip("如果为空，会自动在运行时创建并作为临时 SFX source 使用")]
        [SerializeField] private AudioSource sfxSource;

        [Tooltip("用于循环播放（例如滑铲声音）。若留空会自动创建")]
        [SerializeField] private AudioSource loopSource;

        [Header("Audio Clips")]
        [Tooltip("可填入多个脚步音，播放时会随机选取")]
        public AudioClip[] footstepClips;

        [Tooltip("滑铲循环音效（可为空）")]
        public AudioClip slideClip;

        [Tooltip("受伤音效（一次性）")]
        public AudioClip hurtClip;

        [Header("Settings")]
        [Tooltip("脚步声最小间隔，避免连续触发时音效堆叠")]
        public float footstepCooldown = 0.28f;

        [Tooltip("脚步音随机音高范围 (min)")]
        public float footstepPitchMin = 0.95f;
        [Tooltip("脚步音随机音高范围 (max)")]
        public float footstepPitchMax = 1.05f;

        [Range(0f, 1f)]
        public float sfxVolume = 1f;

        private float footstepTimer;

        void Reset()
        {
            // 在编辑器中为方便，尝试自动获取现存的 AudioSource
            if (sfxSource == null) sfxSource = GetComponent<AudioSource>();
        }

        void Update()
        {
            if (footstepTimer > 0f) footstepTimer -= Time.deltaTime;
        }

        // 播放一个脚步声（会根据 cooldown 限制频率）
        public void PlayFootstep()
        {
            if (footstepTimer > 0f) return;
            if (footstepClips == null || footstepClips.Length == 0) return;

            EnsureSfxSource();

            var clip = footstepClips[Random.Range(0, footstepClips.Length)];
            sfxSource.pitch = Random.Range(footstepPitchMin, footstepPitchMax);
            sfxSource.PlayOneShot(clip, sfxVolume);

            footstepTimer = footstepCooldown;
        }

        // 一次性播放滑铲音（PlayOneShot），用于只需播放一次的场景
        public void PlaySlide()
        {
            if (slideClip == null) return;
            EnsureSfxSource();
            sfxSource.pitch = 1f;
            sfxSource.PlayOneShot(slideClip, sfxVolume);
        }

        // 受伤音，立刻播放一次
        public void PlayHurt()
        {
            if (hurtClip == null) return;
            EnsureSfxSource();
            sfxSource.pitch = 1f;
            sfxSource.PlayOneShot(hurtClip, sfxVolume);
        }

        private void EnsureSfxSource()
        {
            if (sfxSource == null)
            {
                sfxSource = gameObject.AddComponent<AudioSource>();
                sfxSource.playOnAwake = false;
            }
        }

        private void EnsureLoopSource()
        {
            if (loopSource == null)
            {
                loopSource = gameObject.AddComponent<AudioSource>();
                loopSource.playOnAwake = false;
                loopSource.loop = true;
            }
        }
    }
}
