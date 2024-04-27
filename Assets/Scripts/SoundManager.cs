using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    [Range(0, 1)]
    public float volume = 1f;
    float globalVolume = 1f;

    [Range(-3, 3)]
    public float pitch = 1f;

    [Range(0, 0.5f)]
    public float pitchRandomness = 0f;

    public bool loop = false;

    [Header("OneShot settings")]
    public bool onlyPlaysOneShots = false;
    public int maxSoundsAtSameTime = 10;

    [HideInInspector]
    public AudioSource audioSource;

    double coolDown = 0;
    Queue<double> startTimes = new Queue<double>();
    public Sound()
    {
        name = "";
        clip = null;
        volume = 1;
        pitch = 1;
        pitchRandomness = 0;
        loop = false;
        onlyPlaysOneShots = false;
        maxSoundsAtSameTime = 10;
        coolDown = 0;
        startTimes = new Queue<double>();
    }
    public Sound(string name, AudioClip clip) : this()
    {
        this.name = name;
        this.clip = clip;
        SoundManager.Instance.AddSound(this);
    }

    public static float RandomNum(float nuo, float iki) { return Random.Range(nuo, iki); }

    float GetRandomPitch() { return pitch + RandomNum(-pitchRandomness, pitchRandomness); }

    bool OnlyOneShotCheck()
    {
        if (onlyPlaysOneShots)
        {
            Debug.LogError("Can't play, because onlyPlaysOneShots = true");
            return true;
        }
        return false;
    }

    void UpdateStartTimes()
    {
        while(startTimes.Count != 0 && startTimes.Peek() <= Time.timeAsDouble)
        {
            startTimes.Dequeue();
        }
    }

    public Sound Play()
    {
        if (OnlyOneShotCheck()) return this;
        audioSource.pitch = GetRandomPitch();
        audioSource.Play();
        return this;
    }

    public Sound PlayWithDelay(float delayInSeconds)
    {
        if (OnlyOneShotCheck()) return this;
        audioSource.pitch = GetRandomPitch();
        audioSource.PlayDelayed(delayInSeconds);
        return this;
    }

    public Sound ResetCoolDown()
    {
        coolDown = 0;
        return this;
    }

    public Sound PlayWithCooldown(float coolDown)
    {
        if (this.coolDown <= Time.timeAsDouble)
        {
            this.coolDown = coolDown + Time.timeAsDouble;
            PlayOneShot();
        }
        return this;
    }
    public Sound PlayWithCooldown(float coolDown, Vector3 position)
    {
        if (this.coolDown <= Time.timeAsDouble)
        {
            this.coolDown = coolDown + Time.timeAsDouble;
            PlayOneShot(position);
        }
        return this;
    }

    public Sound PlayIfEnded()
    {
        return PlayWithCooldown(clip.length);
    }
    public Sound PlayIfEnded(Vector3 position)
    {
        return PlayWithCooldown(clip.length, position);
    }

    public bool doCooldownLoop { get; private set; }
    float coolDownLoopCooldown;
    int coolDownLoopIterationCount;
    public Sound StartCooldownLoop(float coolDown, int playCount)
    {
        doCooldownLoop = true;
        coolDownLoopCooldown = coolDown;
        coolDownLoopIterationCount = playCount;
        return this;
    }
    public Sound StopCooldownLoop()
    {
        doCooldownLoop = false;
        return this;
    }

    public Sound UpdateCooldownLoop()
    {
        if (coolDown <= Time.timeAsDouble)
        {
            PlayWithCooldown(coolDownLoopCooldown);
            if (coolDownLoopIterationCount > 0)
            {
                coolDownLoopIterationCount--;
                if (coolDownLoopIterationCount == 0)
                {
                    StopCooldownLoop();
                }
            }
        }
        return this;
    }

    public Sound Stop()
    {
        audioSource.Stop();
        return this;
    }

    public Sound Pause()
    {
        audioSource.Pause();
        return this;
    }

    public Sound UnPause()
    {
        audioSource.UnPause();
        return this;
    }

    public Sound PlayOneShot()
    {
        UpdateStartTimes();

        if (startTimes.Count < maxSoundsAtSameTime)
        {
            startTimes.Enqueue(Time.timeAsDouble + clip.length);
            audioSource.pitch = GetRandomPitch();
            audioSource.PlayOneShot(clip, (onlyPlaysOneShots) ? volume : 1f);
        }
        return this;
    }
    public Sound PlayOneShot(Vector3 position)
    {
        UpdateStartTimes();

        if (startTimes.Count < maxSoundsAtSameTime)
        {
            startTimes.Enqueue(Time.timeAsDouble + clip.length);
            audioSource.pitch = GetRandomPitch();
            AudioSource.PlayClipAtPoint(clip, position, volume);
        }
        return this;
    }

    public Sound ChangePitch(float pitch)
    {
        audioSource.pitch = pitch;
        return this;     
    }

    public Sound GlobalVolumeChanged(float val)
    {
        globalVolume = val;
        ChangeVolume(volume);
        return this;
    }

    public Sound ChangeVolume(float volume)
    { 
        audioSource.volume = volume*globalVolume;
        this.volume = volume;
        return this;
    }

    public Sound ChangeLooping(bool loop)
    {
        audioSource.loop = loop;
        return this;
    }

    public Sound GetPitch(out float pitchOut)
    {
        pitchOut = pitch;
        return this;
    }

    public Sound GetVolume(out float volumeOut)
    {
        volumeOut = volume;
        return this;
    }

    public Sound GetLooping(out bool loopOut)
    {
        loopOut = loop;
        return this;
    }
}

[System.Serializable]
public class Music
{
    public string name;
    public AudioClip clip;
    [Range(0, 1)]
    public float volume = 1f;

    public bool isPlaying
    {
        get
        {
            return sound.audioSource.isPlaying;
        }
    }

    public Sound sound { get; private set; }
    public int soundId { get; private set; }

    public Music(){
        name = "";
        clip = null;
        volume = 1f;
        sound = null;
    }

    public Music(string name, AudioClip clip) : this()
    {
        this.name = name;
        this.clip = clip;
        sound = new Sound(name, this.clip);
        soundId = SoundManager.Instance.GetSoundId(name);
    }

    public Music SetSound(Sound sound)
    {
        this.sound = sound;
        name = sound.name;
        soundId = SoundManager.Instance.GetSoundId(name);
        return this;
    }

    public Music GetSound(out Sound sound)
    {
        sound = this.sound;
        return this;
    }

    public Sound GetSound()
    {
        Sound ans;
        GetSound(out ans);
        return ans;
    }

    public Music GlobalVolumeChanged(float val)
    {
        sound.GlobalVolumeChanged(val);
        return this;
    }

    public Music SetVolume(float volume)
    {
        this.volume = volume;
        sound.ChangeVolume(volume);
        return this;
    }

    public Music GetVolume(out float volumeOut)
    {
        volumeOut = volume;
        return this;
    }

    public void Play()
    {
        sound.Play();
    }

    public void Stop()
    {
        sound.Stop();
    }

    public void Pause()
    {
        sound.Pause();
    }

    public void UnPause()
    {
        sound.UnPause();
    }

}

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    List<Sound> sounds = new List<Sound>();

    [Header("Music")]
    [Space(15)]
    [SerializeField] bool playOnAwake = false;
    [SerializeField] bool muteWithMAndChangeSong = false;
    [SerializeField] bool whenStoppedChangeToNext = false;

    [SerializeField]
    List<Music> songs = new List<Music>();

    public event System.Action onSongFinished;

    Dictionary<string, int> soundHash;
    Dictionary<string, int> musicHash;

    public static SoundManager Instance = null;

    AudioSource oneShotAudioSource = null;

    bool musicMuted = false;
    int currentSongIndex = 0;
    Music playingSong = null;

    float globalMusicVolume = 1;
    float globalSoundVolume = 1;

    private void Reset()
    {
        sounds = new List<Sound>()
        {
            new Sound()
        };
        songs = new List<Music>()
        {
            new Music()
        };
    }

    public void ChangeGlobalMusicVolume(float val)
    {
        foreach (Music i in songs)
        {
            i.GlobalVolumeChanged(val);
        }
    }

    public void ChangeGlobalSoundVolume(float val)
    {
        foreach (Sound i in sounds)
        {
            i.GlobalVolumeChanged(val);
        }
    }

    void AddAudioSource(Sound sound)
    {
        if (!sound.onlyPlaysOneShots) { 
            AudioSource source = gameObject.AddComponent<AudioSource>();

            sound.audioSource = source;
            source.pitch = sound.pitch;
            source.loop = sound.loop;
            source.volume = sound.volume;
            source.clip = sound.clip;
            source.playOnAwake = false;
        }
        else
        {
            if (oneShotAudioSource == null)
            {
                oneShotAudioSource = gameObject.AddComponent<AudioSource>();
                oneShotAudioSource.pitch = 1;
                oneShotAudioSource.loop = false;
                oneShotAudioSource.volume = 1;
                oneShotAudioSource.playOnAwake = false;
            }
            sound.audioSource = oneShotAudioSource;
        }
    }

    int soundIndex = 0;
    int musicIndex = 0;
    public void AddSound(Sound sound)
    {
        if (!soundHash.ContainsKey(sound.name))
        {
            AddAudioSource(sound);
            sounds.Add(sound);
            soundHash.Add(sound.name, soundIndex++);
        }
    }

    public void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        Instance = this;

        soundHash = new Dictionary<string, int>(sounds.Count);
        musicHash = new Dictionary<string, int>(songs.Count);
        
        foreach (Sound i in sounds)
        {
            AddAudioSource(i);

            soundHash.Add(i.name,soundIndex++);
        }

        foreach(Music i in songs)
        {
            i.SetSound(new Sound(i.name, i.clip));

            musicHash.Add(i.name, musicIndex++);
        }

        if (playOnAwake)
        {
            PlaySong(currentSongIndex);
        }
    }

    public Music getCurrentSong()
    {
        return playingSong;
    }

    public void StopCurrentSong()
    {
        if (playingSong != null)
        {
            playingSong.Stop();
        }
    }

    public void PauseCurrentSong()
    {
        if (playingSong != null)
        {
            playingSong.Pause();
        }
    }

    public void UnPauseCurrentSong()
    {
        if (playingSong != null)
        {
            playingSong.UnPause();
        }
    }

    public void PlaySong(int songIndex)
    {
        Music music = GetMusic(songIndex);
        if (music != null)
        {
            music.Play();
            currentSongIndex = songIndex;
            playingSong = music;
        }
    }

    public void PlaySongFromIndexList(int[] indices)
    {
        PlaySong(indices[Random.Range(0, indices.Length)]);
    }
    public void PlaySongRandom()
    {
        PlaySong(Random.Range(0, songs.Count));
    }
    

    public int GetSoundId(string name)
    {
        int index;
        if (soundHash.TryGetValue(name, out index))
        {
            return index;
        }
        Debug.LogError("Couldn't find "+name+ " sound");
        return -1;
    }
    
    public Sound GetSound(int id)
    {
        if (id >= 0 && id < sounds.Count)
        {
            return sounds[id];
        }
        Debug.LogError("Couldn't find given sound!");
        return null;
    }

    public List<Sound> GetSoundList()
    {
        return sounds;
    }

    public Sound GetSound(string name)
    {
        return GetSound(GetSoundId(name));
    }

    public int GetMusicId(string name)
    {
        int index;
        if (musicHash.TryGetValue(name, out index))
        {
            return index;
        }
        Debug.LogError("Couldn't find " + name + " song");
        return -1;
    }

    public Music GetMusic(int id)
    {
        if (id >= 0 && id < songs.Count)
        {
            return songs[id];
        }
        Debug.LogError("Couldn't find given song!");
        return null;
    }

    public Music GetMusic(string name)
    {
        return GetMusic(GetMusicId(name));
    }

    public List<Music> GetMusicList()
    {
        return songs;
    }

    private void Update()
    {
        foreach(Sound i in sounds)
        {
            if (i.doCooldownLoop)
            {
                i.UpdateCooldownLoop();
            }
        }

        if (playingSong != null && !playingSong.isPlaying && !musicMuted)
        {
            onSongFinished();
            if (whenStoppedChangeToNext)
            {
                musicIndex = (musicIndex + 1) % songs.Count;
                playingSong = songs[musicIndex];
                PlaySong(musicIndex);
            }
        }

        if (muteWithMAndChangeSong && Input.GetKeyDown(KeyCode.M))
        {
            musicMuted = !musicMuted;
            if (musicMuted)
            {
                playingSong.Stop();
            }
            else
            {
                musicIndex = (musicIndex + 1) % songs.Count;
                playingSong = songs[musicIndex];
                PlaySong(musicIndex);
            }
        }
    }
}