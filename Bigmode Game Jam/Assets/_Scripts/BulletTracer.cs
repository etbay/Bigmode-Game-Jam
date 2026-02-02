using System.Collections;
using UnityEngine;

public class BulletTracer : MonoBehaviour
{
    [SerializeField] private LineRenderer line;

    public BulletTracer FireTracer(Vector3 start, Vector3 end, float startWidth, float decay)
    {
        line.enabled = true;
        line.widthMultiplier = startWidth;
        line.SetPosition(0, start);
        line.SetPosition(1, end);
        StartCoroutine(BulletDecay(line, decay));
        return this;
    }

    private IEnumerator BulletDecay(LineRenderer line, float time)
    {
        float initialWidth = line.widthMultiplier;
        float elapsedTime = 0f;
        while (elapsedTime < time)
        {
            line.widthMultiplier = Mathf.Lerp(initialWidth, 0f, elapsedTime);
            yield return null;
            elapsedTime += Time.deltaTime;
        }
        line.widthMultiplier = 0f;
        line.gameObject.SetActive(false);
    }
}
