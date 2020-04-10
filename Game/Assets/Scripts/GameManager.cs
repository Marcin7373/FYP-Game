using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float volume = 0f, difficulty = 2f;
    public Brain brain;
    public PlayerController player;
    private int cont = 0, deadCount = 0;
    private bool dead = false;

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

        //Audio
        if (AudioListener.volume > 0f)
        {
            if (Input.GetKeyDown("q")) {
                AudioListener.volume -= 0.1f;
            }

            if (Input.GetAxisRaw("DPadY") == -1f)
            {
                AudioListener.volume -= (0.2f * Time.deltaTime);
            }
        }

        if (AudioListener.volume < 1f)
        {
            if (Input.GetKeyDown("w")) {
                AudioListener.volume += 0.1f;
            }

            if (Input.GetAxisRaw("DPadY") == 1f)
            {
                AudioListener.volume += (0.2f * Time.deltaTime);
            }
        }

        if (brain.GetDead() && !dead)
        {
            dead = true;
            deadCount++;
        }
        dead = brain.GetDead();

        if (Input.GetKeyDown("1")) {
            difficulty = 1f;
        }else if (Input.GetKeyDown("2")) {
            difficulty = 2f;
        }else if (Input.GetKeyDown("3")) {
            difficulty = 3f;
        }

        if (difficulty < 2f) //easy
        {
            player.SetDamage(0.06f);
            brain.SetDmgScale(1f);
        }else if ( difficulty == 2f) //medium
        {
            player.SetDamage(0.04f);
            brain.SetDmgScale(1.2f);
        }
        else if (difficulty > 2f) //hard
        {
            player.SetDamage(0.03f);
            brain.SetDmgScale(1.2f + (deadCount * 0.2f));
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
