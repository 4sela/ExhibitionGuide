using UnityEngine;

public class ModalBookController : MonoBehaviour
{

    [SerializeField] private GameObject modalPanel;
    [SerializeField] private GameObject options;

    private void Awake()
    {
        modalPanel.SetActive(false);

        if(options != null)
        {
            options.SetActive(false);
        }
        
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

        if (options != null) {
            options.SetActive(true);
        }
        
    }

    public void CloseModal()
    {
        modalPanel.SetActive(false);
    }
}
