using UnityEngine;
using UnityEngine.UI;

public class ImageChanger : MonoBehaviour
{
    [SerializeField]
    private Image image;

    [SerializeField]
    private Sprite sprite1;

    [SerializeField]
    private Sprite sprite2;

    [SerializeField]
    private Sprite sprite3;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) == true)
        {
            image.sprite = sprite1;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) == true)
        {
            image.sprite = sprite2;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) == true)
        {
            image.sprite = sprite3;
        }
    }
}
