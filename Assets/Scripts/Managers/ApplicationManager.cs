using UnityEngine;

public class ApplicationManager : MonoBehaviour
{
    public void QuitApplication()
    {
        Quit();
    }

    private void Quit()
    {
        Application.Quit();
    }
}
