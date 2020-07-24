using UnityEngine;

public class enterDialog : MonoBehaviour
{
    public GameObject ED;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            ED.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            ED.SetActive(false);
        }
    }
}
