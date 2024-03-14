using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;

    private Vector3 _originalPos;
    private float _timeAtCurrentFrame;
    private float _timeAtLastFrame;
    private float _fakeDelta;



    public float amplitudeX;
    public float amplitudeY;
    public float DefaultSpeed;
    Transform zeroPosition;
    float stepBuffer = 0;
    public float returnTime;
    void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        zeroPosition = gameObject.transform;
    }
    void SwingMove()
    {
/*        Vector3 position;
        position.x = stepbuffer/amplitudeX
        gameObject.transform.position = */

    }
    void MoveBack()
    {

    }
    void Update()
    {
        // Calculate a fake delta time, so we can Shake while game is paused.
        stepBuffer += Time.deltaTime;
        _timeAtCurrentFrame = Time.realtimeSinceStartup;
        _fakeDelta = _timeAtCurrentFrame - _timeAtLastFrame;
        _timeAtLastFrame = _timeAtCurrentFrame;
    }
    public static void DefaultShake()
    {
        Shake(0.7f, 0.2f);
    }
    public static void Shake(float duration, float amount)
    {
        instance._originalPos = instance.gameObject.transform.localPosition;
        instance.StopAllCoroutines();
        instance.StartCoroutine(instance.cShake(duration, amount));
    }

    public IEnumerator cShake(float duration, float amount)
    {

        float endTime = Time.time + duration;

        while (duration > 0)
        {
            transform.localPosition = _originalPos + Random.insideUnitSphere * amount;

            duration -= _fakeDelta;

            yield return null;
        }

        transform.localPosition = _originalPos;
    }
}
