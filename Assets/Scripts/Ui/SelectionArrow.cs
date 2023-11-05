using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlectionArrow : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private RectTransform[] options;
    [SerializeField] private AudioClip changeSound;// The sound we play when we move the arrow up/down
    [SerializeField] private AudioClip interactSound;// The sound we play when an option is selected
    private RectTransform rect;
    private int currentPosition;

    void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        // Change position of the selection arrow
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            ChangePosition(-1);
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            ChangePosition(1);

        // Interact with options
        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.E))
            Interact();

    }

    private void ChangePosition(int _change)
    {
        currentPosition += _change;

        if (_change != 0)
            SoundManager.instance.Playsound(changeSound);

        if (currentPosition < 0)
            currentPosition = options.Length - 1;
        else if (currentPosition > options.Length - 1)
            currentPosition = 0;

        AssignPosition();
      
    }
    private void AssignPosition()
    {
        // Assign the Y position of the current option to the arrow ( basically moving it up/down )
        rect.position = new Vector3(rect.position.x, options[currentPosition].position.y);
    }
    private void Interact()
    {
        SoundManager.instance.Playsound(interactSound);

        // Access the button component on each option and call it's function
        options[currentPosition].GetComponent<Button>().onClick.Invoke(); 
    }
}
