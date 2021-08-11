using UnityEngine;

namespace peroth
{
    public class PopupScreen : CloseScreen
    {
        public override void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                MenuTabManager.instance.isPopup = false;
                gameObject.SetActive(false);
                Debug.Log("PopupScreen.cs : Update");
            }
        }
    }
}