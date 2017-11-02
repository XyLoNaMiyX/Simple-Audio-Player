namespace CSCore.Streams.Effects
{
    /// <summary>
    /// FlangerPhase. See <see cref="DmoFlangerEffect.Phase"/>.
    /// </summary>
    public enum FlangerPhase
    {
        /// <summary>
        /// 180� Phase.
        /// </summary>
        Phase180 = 4,
        /// <summary>
        /// 90� Phase. 
        /// Default value for <see cref="DmoFlangerEffect.Phase"/>. 
        /// </summary>
        Phase90 = 3,
        /// <summary>
        /// 0� Phase.
        /// </summary>
        PhaseZero = 2,
        /// <summary>
        /// -90� Phase.
        /// </summary>
        PhaseNegative90 = 1,
        /// <summary>
        /// -180� Phase.
        /// </summary>
        PhaseNegative180 = 0,
    }
}