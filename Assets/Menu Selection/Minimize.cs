using UnityEngine;

public class MenuToggle : MonoBehaviour
{
    public GameObject weatherMenuPanel;   // Full menu panel (with minimize button inside)
    public GameObject maximizedIcon;      // Maximize button or small icon shown when menu is hidden

    // Called when minimize button is clicked

    public void Start()
    {
        maximizedIcon.SetActive(false);
    }
    public void MinimizeMenu()
    {
        weatherMenuPanel.SetActive(false);
        maximizedIcon.SetActive(true);
    }

    // Called when maximize button (minimized icon) is clicked
    public void MaximizeMenu()
    {
        weatherMenuPanel.SetActive(true);
        maximizedIcon.SetActive(false);
    }
}
