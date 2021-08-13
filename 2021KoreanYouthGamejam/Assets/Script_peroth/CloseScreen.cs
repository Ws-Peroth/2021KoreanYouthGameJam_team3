using UnityEngine;

namespace peroth
{
    public class CloseScreen : MonoBehaviour
    {
        public virtual void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                CloseButtonDown();
        }

        public void CloseButtonDown()
        {
            ChatLogManager.instance.RemoveContentToText();
            MenuTabManager.instance.isPopup = false;
            gameObject.SetActive(false);
            Debug.Log("CloseScreem.cs : CloseButtonDown");
        }
    }
}