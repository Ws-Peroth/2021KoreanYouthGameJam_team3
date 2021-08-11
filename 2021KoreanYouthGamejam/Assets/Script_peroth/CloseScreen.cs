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
            MenuTabManager.instance.isPopup = false;
            gameObject.SetActive(false);
        }
    }
}