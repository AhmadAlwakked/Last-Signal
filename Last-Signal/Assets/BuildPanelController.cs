using UnityEngine;

public class BuildPanelController : MonoBehaviour
{
    public GameObject buildPanel;
    private Animator animator;

    void Start()
    {
        animator = buildPanel.GetComponent<Animator>();
        buildPanel.SetActive(false); // Begin onzichtbaar
    }

    public void ToggleBuildPanel()
    {
        if (!buildPanel.activeSelf)
        {
            buildPanel.SetActive(true); // Activeer eerst
            animator.SetTrigger("OpenPanel"); // Trigger de animatie
        }
        else
        {
            buildPanel.SetActive(false); // Optioneel: verbergen zonder animatie
        }
    }
}
