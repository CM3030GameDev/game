using UnityEngine;

public class MinibossAnimationEvent : MonoBehaviour
{
    private Act2Miniboss boss;
    private MinibossBeamAttack beam;

    void Awake()
    {
        boss = GetComponentInParent<Act2Miniboss>();
        beam = GetComponentInParent<MinibossBeamAttack>();
    }

    // Melee for Act2Miniboss
    public void OnMeleeHitEvent() => boss.OnHitEvent();
    public void OnMeleeEndEvent() => boss.OnHitEndEvent();

    // Beam for MinibossBeamAttack
    public void OnBeamShotEvent() => beam.OnBeamShotEvent();

/*    // Death
    public void OnDeathAnimationEnd() => boss.TriggerDestroy();*/
}
