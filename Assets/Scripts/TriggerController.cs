using System.Security.Permissions;
using UnityEngine;
using UnityEngine.Events;

public class TriggerController : MonoBehaviour
{
    [SerializeField] private bool isTriggerActive = true;

    [Header("Connected Triggers")]
    [SerializeField] private TriggerController[] connectedTriggers;

    [Header("Disconnected Triggers")]
    [SerializeField] private TriggerController[] disconnectedTriggers;

    [Header("Animation")]
    [SerializeField] private Animator targetAnimator;
    [SerializeField] private string enterAnimationTrigger = "";
    [SerializeField] private string exitAnimationTrigger = "";

    [Header("Za svaki slucaj")]
    public UnityEvent onPlayerEnter;
    public UnityEvent onPlayerExit;

    private string playerTag = "Player";

    private bool isPlayerInside = false;

    public void ActivateTrigger() { isTriggerActive = true; }
    public void DeactivateTrigger() { isTriggerActive = false; }

    private void OnTriggerEnter(Collider other)
    {
        if (!isTriggerActive || !other.CompareTag(playerTag)) return;

        isPlayerInside = true;

        foreach (TriggerController trigger in connectedTriggers)
        {
            if (trigger != null)
            {
                trigger.ActivateTrigger();
            }
        }

        foreach (TriggerController trigger in disconnectedTriggers)
        {
            if (trigger != null)
            {
                trigger.DeactivateTrigger();
            }
        }

        PlayAnimation(enterAnimationTrigger);
        onPlayerEnter.Invoke();


    }

    private void OnTriggerExit(Collider other)
    {
        if (!isPlayerInside) return;
        if (!other.CompareTag(playerTag)) return;

        isPlayerInside = false;

        PlayAnimation(exitAnimationTrigger);
        onPlayerExit.Invoke();
    }

    private void PlayAnimation(string triggerName)
    {
        if (targetAnimator == null || string.IsNullOrEmpty(triggerName)) return;

        targetAnimator.SetTrigger(triggerName);
    }
}

