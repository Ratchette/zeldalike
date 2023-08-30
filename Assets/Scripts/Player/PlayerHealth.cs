using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health {
    [SerializeField] private FloatValue playerHealth;
    [SerializeField] private SignalSender playerHealthSignal;

    void Start() {
        health = playerHealth.runtimeValue;  
    }

    public override void Heal(float heal_ammount) {
        base.Heal(heal_ammount);
        playerHealth.runtimeValue = health;
        playerHealthSignal.Raise();
    }

    public override bool Damage(float damage_ammount) {
        bool isDead = base.Damage(damage_ammount);
        playerHealth.runtimeValue = health;
        playerHealthSignal.Raise();
        return isDead;
    }
}
