using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {
    [SerializeField] private FloatValue maxHealth;
    protected float health;

    void Start() {
        Reset();
    }

    public void Reset() {
        health = maxHealth.runtimeValue;
    }

    public virtual void Heal(float heal_ammount) {
        health += heal_ammount;

        if(health > maxHealth.runtimeValue) {
            health = maxHealth.runtimeValue;
        }
    }

    // Return values
    //      true = this character is still alive (health > 0)
    //      false = the characther has died (health <= 0)
    public virtual bool Damage(float damage_ammount) {
        Debug.LogFormat("[{0}][Damage] health={1}, damage={2}, newHealth={3}", 
            this.name, health, damage_ammount, (health - damage_ammount));

        health -= damage_ammount;

        return health > 0;
    }
}
