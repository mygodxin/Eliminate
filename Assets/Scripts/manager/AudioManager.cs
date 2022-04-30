using UnityEngine;
using FairyGUI;
using System.Collections.Generic;
public class AudioManager : MonoBehaviour
{
    public static AudioManager inst;

    public AudioSource _musicAudio;
    public AudioSource _effectAudio;
    Dictionary<string, AudioClip> musicMap;
    void Awake()
    {
        if (inst == null)
            inst = new AudioManager();
        if(this.musicMap == null)
            this.musicMap = new Dictionary<string, AudioClip>();
    }
    public void initAudio(AudioSource audio)
    {
        // this._audio = audio;
        // cc.audioEngine.setEffectsVolume(0.6);
        // cc.audioEngine.setMusicVolume(0.4);
        int kg = this.getAudioSwitch(2);
        GRoot.inst.soundVolume = kg > 0 ? 1 : 0;

        this.preload();
    }

    /**预加载音频 */
    void preload()
    {
    }

    bool isExist(string name)
    {
        return !!this.musicMap[name];
    }

    /**播放背景音乐 */
    public void playMusic(string name, bool loop = true)
    {
        var musicState = this.getAudioSwitch(1);
        if (musicState == 0) return;

        AudioClip clip = Resources.Load<AudioClip>(name);
        this._musicAudio.clip = clip;
        this._musicAudio.Play();
    }
    public void stopMusic()
    {
        if (this._musicAudio.isPlaying)
        {
            this._musicAudio.Stop();
        }
    }

    /**播放音效*/
    public void playEffect(string name)
    {
        var effectState = this.getAudioSwitch(2);
        if (effectState == 0) return;
        AudioClip clip = Resources.Load<AudioClip>(name);
        this._effectAudio.clip = clip;
        this._effectAudio.PlayOneShot(clip);
    }

    /**停止音效 */
    public void stopEffect()
    {
        GRoot.inst.soundVolume = 0;
        this._effectAudio.Stop();
    }

    /**恢复音效 */
    public void resumeEffect()
    {
        GRoot.inst.soundVolume = 1;
    }

    public void resumeMusic()
    {

    }

    void saveAudioSetting(int state, int type)
    {
        var data = Storage.GetIntArray(DataDef.AUDIO);
        if (data == null)
            data = new int[] { 0, 1, 1 };
        if (type == 0)
        {
            data[1] = data[2] = state;
        }
        else
        {
            if (data[type] != state)
            {
                data[type] = state;
            }
        }
        Storage.SetIntArray(DataDef.AUDIO, data);

        if (type == 0)
        {
            if (state > 0)
            {
                this.resumeMusic();
                this.resumeEffect();
            }
            else
            {
                this.stopMusic();
                this.stopEffect();
            }
        }
        else
        {
            if (state > 0)
            {
                this.resumeMusic();
                this.resumeEffect();
            }
            else
            {
                if (type == 1)
                    this.stopMusic();
                else if (type == 2)
                    this.stopEffect();
            }
        }
    }

    /**获取音频设置(1音乐 2音效 不填则为总开关)*/
    public int getAudioSwitch(int type)
    {
        var data = Storage.GetIntArray(DataDef.AUDIO);
        if (data == null)
            data = new int[] { 0, 1, 1 };
        return data[type];
    }
}