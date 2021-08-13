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
                ChatLogManager.instance.RemoveContentToText();
                gameObject.SetActive(false);
            }
        }
    }
}