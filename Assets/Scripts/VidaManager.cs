using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VidaManager : MonoBehaviour
{
    public int vidas = 3;

    public void PerderVida() {
        vidas--;
        Debug.Log("Vida perdida! Vidas restantes: " + vidas);
        if (vidas <= 0) {
            GameOver();
        }
    }

    void GameOver() {
        Debug.Log("Game Over");
        // Chamar UI, resetar, etc.
    }
}
