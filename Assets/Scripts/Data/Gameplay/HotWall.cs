using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotWall : MonoBehaviour
{
    private BoxCollider damageTriggerBox;

    private Timer damageTimer;

    private void Awake()
    {
        damageTimer = gameObject.AddComponent<Timer>();


        damageTriggerBox = gameObject.AddComponent<BoxCollider>();
        damageTriggerBox.isTrigger = true;
        damageTriggerBox.size = Vector3.one * 1.25f;


        damageTimer.SetTimer(0.5f, 1, () => { Player.main.TakeDamage(1); Debug.Log(Player.main.Health); });
        damageTimer.SetLoop(true);
        damageTimer.PauseTimer();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == Player.main.Name)
        {
            damageTimer.ResumeTimer();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == Player.main.Name)
        {
            damageTimer.PauseTimer();
        }
    }
}
