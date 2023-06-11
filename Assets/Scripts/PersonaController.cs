using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonaController : MonoBehaviour
{
    private Animator personaAnimator;
    private int playerCombo;

    #region Events
    private void OnEnable()
    {
        GameManager.StopGame += StopPersona;
    }
    private void OnDisable()
    {
        GameManager.StopGame -= StopPersona;
    }
    private void StopPersona()
    {
        personaAnimator.enabled = false;
    }
    #endregion

    void Start()
    {
        personaAnimator = GetComponent<Animator>();
        playerCombo = personaAnimator.GetInteger("playerCombo");
    }

    public void PlayPersonaMiss()
    {
        personaAnimator.SetTrigger("playerMissed");
    }

    public void UpdatePersonaCombo(int scoreCombo)
    {
        playerCombo = scoreCombo;
    }
}
