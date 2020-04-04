using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float volume = 0f;
    public Brain brain;
    public PlayerController player;
    private int cont = 0;

    void Awake()
    {
        AudioListener.volume = volume;

        StartCoroutine(ContScan());       
    }

    void Update()
    {
        //quit game
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }

        //pause game
        if (Input.GetKeyDown("p") || Input.GetButtonDown("Options"))
        {
            Time.timeScale = Time.timeScale == 0 ? 1 : 0;
        }

        if ((Input.GetKeyDown("q") || Input.GetAxisRaw("DPadY") == -1f) && AudioListener.volume > 0f)
        {
            AudioListener.volume -= (0.2f * Time.deltaTime);
        }

        if ((Input.GetKeyDown("w") || Input.GetAxisRaw("DPadY") == 1f) && AudioListener.volume < 1f)
        {
            AudioListener.volume += (0.2f * Time.deltaTime);
        }
    }

    IEnumerator ContScan()
    {
        while (true)
        {
            for (int i = 0; i < Input.GetJoystickNames().Length; i++)
            {
                cont = Input.GetJoystickNames()[i].Length > 0 ? 1 : 0;
                player.SetCont(cont);
            }
            yield return new WaitForSeconds(3);
        }
    }
}
