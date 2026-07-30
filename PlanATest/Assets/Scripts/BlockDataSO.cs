using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  This enum is used to check the color and the status of the block for the collect and refill functions
/// </summary>
public enum BlockType
{
    Blue,
    Green,
    Yellow,
    Pink,
    Brown,
    Empty
}


[System.Serializable]
public class BlockData
{
    public BlockType type;
    public Sprite sprite;
}

/*
This Data will provide the needed assets for the BlockObject Prefab
*/
[CreateAssetMenu(fileName = "BlockDataConfig", menuName = "PlanATest/Block Data Config")]
public class BlockDataSO : ScriptableObject
{
    [SerializeField] private List<BlockData> blocks = new List<BlockData>();

    public BlockData GetRandomBlock()
    {
        if (blocks == null || blocks.Count == 0)
        {
            Debug.LogError("No Blocks in list");
            return null;
        }

        int randomIndex = UnityEngine.Random.Range(0, blocks.Count);
        return blocks[randomIndex];
    }
}