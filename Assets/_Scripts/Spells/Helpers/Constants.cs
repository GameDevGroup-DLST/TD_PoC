using System;

[Serializable]
public enum MovementType {
    None,
    Linear,
    Sinusoidal,
    Custom,
}

[Serializable]
public enum TargetType {
    None,
    Self,
    Point,
    Targeted
}

[Serializable]
public enum OnKillEffect {
    None,
}

[Serializable]
public enum OnHitEffect {
    None,
}