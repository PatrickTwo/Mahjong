using UnityEngine;

namespace Mahjong
{
    /// <summary>
    /// 大厅音频设置服务。
    /// </summary>
    public class LobbyAudioSettingService : IAudioSettingService
    {
        #region 常量

        private const string MicEnabledKey = "Lobby.Audio.MicEnabled";
        private const string SpeakerEnabledKey = "Lobby.Audio.SpeakerEnabled";

        #endregion

        #region 麦克风

        /// <summary>
        /// 获取麦克风开关状态。
        /// </summary>
        /// <returns>是否开启。</returns>
        public bool GetMicEnabled()
        {
            return PlayerPrefs.GetInt(MicEnabledKey, 1) == 1;
        }

        /// <summary>
        /// 设置麦克风开关状态。
        /// </summary>
        /// <param name="isEnabled">是否开启。</param>
        public void SetMicEnabled(bool isEnabled)
        {
            PlayerPrefs.SetInt(MicEnabledKey, isEnabled ? 1 : 0);
            PlayerPrefs.Save();
        }

        #endregion

        #region 扬声器

        /// <summary>
        /// 获取扬声器开关状态。
        /// </summary>
        /// <returns>是否开启。</returns>
        public bool GetSpeakerEnabled()
        {
            return PlayerPrefs.GetInt(SpeakerEnabledKey, 1) == 1;
        }

        /// <summary>
        /// 设置扬声器开关状态。
        /// </summary>
        /// <param name="isEnabled">是否开启。</param>
        public void SetSpeakerEnabled(bool isEnabled)
        {
            PlayerPrefs.SetInt(SpeakerEnabledKey, isEnabled ? 1 : 0);
            PlayerPrefs.Save();
        }

        #endregion
    }
}
