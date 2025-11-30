using System;
using Mono.Cecil.Cil;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class WaveMovement : MonoBehaviour
{
    public Rigidbody2D body;

    public BoxCollider2D bottomCollider;

    public TextMeshProUGUI textOutput;

    public float ySpeed = 0.5f;

    float time;

    float startY;

    float TIMER_LENGTH = 2 * Mathf.PI;
    float targetTime;
    int waveState = 0;

    void Start()
    {
        // startY = transform.localPosition.y;
        targetTime = TIMER_LENGTH;
    }

    void Update()
    {
        Debug.Log("test: ");
        switch (waveState)
        {
            default:
            case 0:
                {
                    WaveIdle();
                    break;
                }
            case 1:
                {
                    WaveCycle(1);
                    break;
                }
            case 2:
                {
                    WaveCycle(3);
                    break;
                }

        }

        Debug.Log("targetTime: " + targetTime);
    }

    void UpdateTime()
    {
        time += Time.deltaTime; //* ySpeed;

        if (time > 2 * Mathf.PI)
        {
            // time -= 2 * Mathf.PI;
            nextState();
        }
    }

    void WaveIdle()
    {
        Timer();
        // transform.localPosition = new Vector2(body.position.x, startY);
    }

    void WaveCycle(float amplitude)
    {
        UpdateTime();

        //+1 to change range from [-1,1] to [0,2] 
        float yPos = (Mathf.Cos(time + Mathf.PI) + 1) * -1;

        // transform.localPosition = new Vector2(body.position.x, yPos * amplitude + startY);
    }

    void nextState()
    {
        waveState = (waveState + 1) % 3;
        time = 0;
    }

    void Timer()
    {
        targetTime -= Time.deltaTime;

        if (targetTime <= 0f)
        {
            nextState();
            targetTime = TIMER_LENGTH;
        }
    }

}