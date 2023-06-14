using System.Collections;
using UnityEngine;

public class DraggingController : MonoBehaviour
{
    private bool isSelected = false;
    private BuildingModel selectedModel;

    private void EventListener()
    {
        
    }
    private void Update()
    {
        if (Input.touchCount < 1) return;
        if(Input.GetTouch(0).phase != TouchPhase.Ended) return;
        
        StopDragging();
    }

    private void StartDragging(BuildingModel building)
    {
        selectedModel = building;
        isSelected = true;

        StartCoroutine(IEDragging());
    }

    private IEnumerator IEDragging()
    {
        yield return null;
    }
    private void StopDragging()
    {
        selectedModel = null;
        isSelected = false;
        StopAllCoroutines();
    }
}