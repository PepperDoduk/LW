using System.Collections.Generic;
using UnityEngine;

public class Area : MonoBehaviour
{
    public bool occupied = false;

    private HashSet<Collider2D> enemyColliders = new HashSet<Collider2D>();
    private HashSet<Collider2D> unitColliders = new HashSet<Collider2D>();

    public float enemyPercent;
    public float ourPercent;
    public float emptyPercent = 100;

    public float occupiedPercent;

    public float emptyUnit = 5;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemyColliders.Add(other);
        }
        else if (other.CompareTag("Unit"))
        {
            unitColliders.Add(other);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemyColliders.Remove(other);
        }
        else if (other.CompareTag("Unit"))
        {
            unitColliders.Remove(other);
        }
    }

    public bool ReturnArea()
    {
        return occupied;
    }

    private void Update()
    {
        if (Time.frameCount % 20 == 0)
        {
            CalculatePercentages();
            PrintCounts();
        }
    }

    private void CalculatePercentages()
    {
        int totalUnits = enemyColliders.Count + unitColliders.Count;

        if (totalUnits > 0)
        {
            enemyPercent = (enemyColliders.Count / (float)totalUnits) * 100;
            ourPercent = (unitColliders.Count / (float)totalUnits) * 100;
        }
        else
        {
            enemyPercent = 0;
            ourPercent = 0;
        }

        emptyPercent = 100 - (enemyPercent + ourPercent);
    }

    private void PrintCounts()
    {
        Debug.Log($"현재 닿아있는 Enemy 개수: {enemyColliders.Count}");
        Debug.Log($"현재 닿아있는 Unit 개수: {unitColliders.Count}");
        Debug.Log($"적 유닛 비율: {enemyPercent:F2}%");
        Debug.Log($"아군 유닛 비율: {ourPercent:F2}%");
        Debug.Log($"빈 공간 비율: {emptyPercent:F2}%");
    }
}
