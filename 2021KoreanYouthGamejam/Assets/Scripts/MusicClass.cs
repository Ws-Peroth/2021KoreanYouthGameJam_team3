using UnityEngine;

namespace peroth
{
    public class MusicClass : SingletonDontDestroy<MusicClass>
    {
        private AudioSource _audioSource;
        [SerializeField] private AudioClip title;
        [SerializeField] private AudioClip stage;

        protected override void Awake()
        {
            title = Resources.Load("StageBgm") as AudioClip;
            title = Resources.Load("TitleBGM") as AudioClip;

            _audioSource = gameObject.AddComponent<AudioSource>();
            _audioSource.playOnAwake = true;
            _audioSource.loop = true;

            base.Awake();
        }

        public void Start()
        {

        }

        public void PlayMusic()
        {
            if (_audioSource.isPlaying) return;
            _audioSource.Play();
        }

        public void StopMusic()
        {
            _audioSource.Stop();
        }

        public void SetTitleSong()
        {
            _audioSource.clip = title;
        }

        public void TitleSongOn()
        {
            _audioSource.clip = title;

            if (_audioSource.isPlaying) return;
            _audioSource.Play();
        }

        public void SetStageSong()
        {
            _audioSource.clip = title;
        }

        public void StageSongOn()
        {
            _audioSource.clip = title;

            if (_audioSource.isPlaying) return;
            _audioSource.Play();
        }
    }
}