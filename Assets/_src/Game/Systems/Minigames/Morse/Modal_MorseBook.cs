using UnityEngine;

public class Modal_MorseBook : MonoBehaviour
{

    [SerializeField] private GameObject modalPanel;

    private void Awake()
    {
        modalPanel.SetActive(false);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenModal()
    {
        modalPanel.SetActive(true);
    }

    public void CloseModal() 
    { 
        modalPanel.SetActive(false);
    }
}
