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

        public virtual void CloseButtonDown()
        {
            gameObject.SetActive(false);
            Debug.Log("CloseScreem.cs : CloseButtonDown");
        }
    }
}