namespace Mahjong
{
    /// <summary>
    /// 音频设置服务接口。
    /// </summary>
    public interface IAudioSettingService
    {
        #region 麦克风

        /// <summary>
        /// 获取麦克风开关状态。
        /// </summary>
        /// <returns>是否开启。</returns>
        bool GetMicEnabled();

        /// <summary>
        /// 设置麦克风开关状态。
        /// </summary>
        /// <param name="isEnabled">是否开启。</param>
        void SetMicEnabled(bool isEnabled);

        #endregion

        #region 扬声器

        /// <summary>
        /// 获取扬声器开关状态。
        /// </summary>
        /// <returns>是否开启。</returns>
        bool GetSpeakerEnabled();

        /// <summary>
        /// 设置扬声器开关状态。
        /// </summary>
        /// <param name="isEnabled">是否开启。</param>
        void SetSpeakerEnabled(bool isEnabled);

        #endregion
    }
}
