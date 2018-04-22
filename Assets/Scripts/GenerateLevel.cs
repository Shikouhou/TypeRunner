using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GenerateLevel : MonoBehaviour {

    private List<GameObject> level;

    [Header("Building Pieces")]
    public List<GameObject> flatLiners;
    public GameObject edge;
    public GameObject gap;
    public GameObject quickfireGap;
    public GameObject ledgeUp;
    public GameObject ledgeDown;
    public GameObject quickfireStop;
    public GameObject quickfireEnd;
    
    [Header("Customization")]
    public Vector2 startPos;
    public int startFlatliners;
    public int levelLength;
    public int flatsInARow;

    private int flatCountdown;
    private float currentY;

    private string[] lines;

    public void Start()
    {
        currentY = 0;
        flatCountdown = 0;
        level = new List<GameObject>();
        InitiateLevel();
        SpawnStart();
        SpawnLevel();
    }

    private void InitiateLevel()
    {
        int level = GetComponentInChildren<PointsAdder>().scoreManager.LevelPoints;

        levelLength += 10 * (level - 1);
    }

    private void SpawnStart()
    {
        for (int i = 0; i < startFlatliners; i++)
        {
            GameObject pieceToSpawn = flatLiners[UnityEngine.Random.Range(0, flatLiners.Capacity)];
            GameObject piece = Instantiate(pieceToSpawn, new Vector3(), pieceToSpawn.transform.rotation, transform);
            piece.transform.localPosition = new Vector3(startPos.x + i * Globals.spacing, startPos.y, 0);
            level.Add(piece);
        }
    }

    private void SpawnLevel()
    {
        for (int i = 0; i < levelLength; i++)
        {
            SpawnType type = DecideSpawnType();
            switch (type)
            {
                case SpawnType.Flat:
                    SpawnFlat(i);
                    break;
                case SpawnType.Gap:
                    SpawnGap(i);
                    levelLength++;
                    i++;
                    break;
                case SpawnType.LedgeUp:
                    SpawnLedgeUp(i);
                    break;
                case SpawnType.LedgeDown:
                    SpawnLedgeDown(i);
                    break;
                case SpawnType.QuickfireGap:
                    SpawnQuickfireGap(i);
                    levelLength++;
                    i++;
                    break;
                case SpawnType.QuickfireStop:
                    SpawnQuickfireStop(i);
                    break;
                default:
                    break;
            }
        }

        SpawnEnd(levelLength);
    }

    private void SpawnFlat(int i)
    {
        GameObject pieceToSpawn = flatLiners[UnityEngine.Random.Range(0, flatLiners.Count - 1)];
        GameObject piece = Instantiate(pieceToSpawn, Vector3.zero, pieceToSpawn.transform.rotation, transform);
        piece.transform.localPosition = LevelPiecePosition(i);
        level.Add(piece);
    }

    private void SpawnGap(int i)
    {
        GameObject edge = Instantiate(this.edge, Vector3.zero, this.edge.transform.rotation, transform);
        edge.transform.localPosition = LevelPiecePosition(i);
        level.Add(edge);

        GameObject gap = Instantiate(this.gap, Vector3.zero, this.edge.transform.rotation, transform);
        gap.transform.localPosition = LevelPiecePosition(i + 1);
        level.Add(gap);
    }

    private void SpawnLedgeUp(int i)
    {
        GameObject pieceToSpawn = ledgeUp;
        GameObject piece = Instantiate(pieceToSpawn, Vector3.zero, pieceToSpawn.transform.rotation, transform);
        piece.transform.localPosition = LevelPiecePosition(i);
        level.Add(piece);
        currentY += Globals.yMargin;
    }

    private void SpawnLedgeDown(int i)
    {
        GameObject pieceToSpawn = ledgeDown;
        GameObject piece = Instantiate(pieceToSpawn, Vector3.zero, pieceToSpawn.transform.rotation, transform);
        piece.transform.localPosition = LevelPiecePosition(i);
        level.Add(piece);
        currentY -= Globals.yMargin;
    }

    private void SpawnQuickfireGap(int i)
    {
        GameObject edge = Instantiate(this.edge, Vector3.zero, this.edge.transform.rotation, transform);
        edge.transform.localPosition = LevelPiecePosition(i);
        level.Add(edge);

        GameObject gap = Instantiate(quickfireGap, Vector3.zero, quickfireGap.transform.rotation, transform);
        gap.transform.localPosition = LevelPiecePosition(i + 1);
        level.Add(gap);
    }

    private void SpawnQuickfireStop(int i)
    {
        GameObject pieceToSpawn = quickfireStop;
        GameObject piece = Instantiate(pieceToSpawn, Vector3.zero, pieceToSpawn.transform.rotation, transform);
        piece.transform.localPosition = LevelPiecePosition(i);
        level.Add(piece);
    }

    private void SpawnEnd(int i)
    {
        GameObject pieceToSpawn = quickfireEnd;
        GameObject piece = Instantiate(pieceToSpawn, Vector3.zero, pieceToSpawn.transform.rotation, transform);
        piece.transform.localPosition = LevelPiecePosition(i);
        level.Add(piece);

        for (int iterator = 1; iterator < 2; iterator++)
        {
            SpawnFlat(i + iterator);
        }
    }

    private SpawnType DecideSpawnType()
    {
        SpawnType type = SpawnType.Flat;

        if (flatCountdown == 0)
        {
            type = (SpawnType)UnityEngine.Random.Range(0, Enum.GetValues(typeof(SpawnType)).Length - 1);
            flatCountdown = flatsInARow;
        }
        else
        {
            flatCountdown--;
        }

        return type;
    }

    private Vector3 LevelPiecePosition(int iterator)
    {
        return new Vector3(((startFlatliners * Globals.spacing) + startPos.x) + (iterator * Globals.spacing), currentY, 0);
    }
}

public enum SpawnType
{
    Flat,
    Gap,
    LedgeUp,
    LedgeDown,
    QuickfireGap,
    QuickfireStop,
    QuickfireEnd
}