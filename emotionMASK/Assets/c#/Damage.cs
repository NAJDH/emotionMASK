using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    public enum Emotion
{
    Xi,    // 喜
    Nu,    // 怒
    Ai,    // 哀
    Ju     // 惧
}

public static float CalcDamage(float baseDamage, Emotion attacker, Emotion target)
{
    float m = 1f;

    switch (attacker)
    {
        case Emotion.Xi:
            m = 1.2f; // 喜对其他所有都1.2
            break;

        case Emotion.Nu:
            if (target == Emotion.Nu) m = 2f;
            else if (target == Emotion.Ai || target == Emotion.Ju) m = 1.5f;
            // 其他保持1
            break;

        case Emotion.Ai:
            if (target == Emotion.Ju) m = 0.8f; // 哀对惧
            break;

        case Emotion.Ju:
            if (target == Emotion.Ai) m = 0.8f; // 惧对哀
            break;
    }

    return baseDamage * m;
}
}
