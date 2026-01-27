using UnityEngine;

public class PlayerDebugHUD : MonoBehaviour
{
    public player target;
    public bool show = true;

    void OnGUI()
    {
        if (!show || target == null) return;

        var st = target.stateMachine?.currentState?.GetType().Name ?? "null";
        var formIdx = PlayerFormManager.playerForm?.currentFormIndex ?? -1;
        var vel = target.rb != null ? target.rb.velocity : Vector2.zero;

        GUILayout.BeginArea(new Rect(10, 10, 260, 150), GUI.skin.box);
        GUILayout.Label($"State: {st}");
        GUILayout.Label($"Form: {formIdx}");
        GUILayout.Label($"Anim: {(target.anim ? target.anim.GetCurrentAnimatorStateInfo(0).shortNameHash.ToString() : "null")}");
        GUILayout.Label($"Vel: {vel}");
        GUILayout.EndArea();
    }
}