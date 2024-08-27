using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;

namespace MainManagers
{
    [RequireComponent(typeof(AudioSource))]
    public class MainAudioManager : MonoBehaviour
    {
        [SerializeField] private GameObject spawnableAudio;
        public static MainAudioManager instance;

        public AudioClip fieldAmbience;
        public AudioCue normalMusic;
        public AudioSource ambienceSource;
        public AudioSource musicSource;
        public bool playMusic;
        public bool playAmbience;
    
        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }
            
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            LogFile.Enable();
            
            ambienceSource.clip = fieldAmbience;
            ambienceSource.Play();
        }

        private float _desiredVolume;
        private float _desiredVolumeMax;

        public void Update()
        {
            ambienceSource.volume = playAmbience ? 1 : 0;

            musicSource.volume = Mathf.Lerp(musicSource.volume, _desiredVolume, .5f * Time.deltaTime);

            if (!playMusic)
            {
                FadeOut();
                return;
            }
            
            if(musicSource.volume == 0)
                FadeIn();

            _desiredVolume = normalMusic.volume;

            if (musicSource.isPlaying) return;

            AudioClip c;
            do
            {
                c = normalMusic.GetSound();
            } while (c == musicSource.clip);

            _desiredVolumeMax = _desiredVolume;
            musicSource.clip = c;
            musicSource.Play();
            Invoke(nameof(FadeOut), c.length - 2);
        }

        private void FadeIn()
        {
            _desiredVolume = _desiredVolumeMax;
        }
        
        private void FadeOut()
        {
            _desiredVolume = 0;
        }

        public void SpawnAudio(AudioClip clip, Vector3 pos, float volume = 1f, float maxDistance = 150, bool loop = false, bool spatial = true)
        {
            GameObject g = Instantiate(spawnableAudio, pos, quaternion.identity);
            g.GetComponent<AudioSource>().clip = clip;
            g.GetComponent<AudioSource>().volume = volume;
            g.GetComponent<AudioSource>().maxDistance = maxDistance;
            g.GetComponent<AudioSource>().loop = loop;
            g.GetComponent<AudioSource>().spatialBlend = spatial ? 1 : 0;
            g.GetComponent<AudioSource>().Play();
            Destroy(g, clip.length + 1);
        }
    }
}
