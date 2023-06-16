using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonaController : MonoBehaviour
{
    private Animator personaAnimator;

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
        personaAnimator.SetBool("playerIdle", true);
    }
    #endregion

    void Start()
    {
        personaAnimator = GetComponent<Animator>();
    }

    public void PlayPersonaMiss()
    {
        personaAnimator.SetTrigger("playerMissed");
    }

    public void UpdatePersonaCombo(int scoreCombo)
    {
        personaAnimator.SetInteger("playerCombo", scoreCombo);
    }
}
