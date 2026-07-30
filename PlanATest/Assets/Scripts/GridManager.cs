using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("Grid Dimensions")]
    [SerializeField] private Transform origin;
    [SerializeField] private int columns = 5;
    [SerializeField] private int rows = 6;
    [SerializeField] private Vector2 cellSize = new Vector2(1.22f, 1.1f);
    [SerializeField] private float processDelay = 1.0f;

    [Header("References")]
    [SerializeField] private BlockObject blockPrefab;
    [SerializeField] private BlockDataSO blockConfig;


    public delegate bool CollectedHandler(int totalBlocksCollected);
    public event CollectedHandler OnComboCollected;
    private bool isCollecting = false;
    private bool isRunning = true;
    private BlockObject[,] grid;

    private readonly Vector2Int[] adjacentDirections = new Vector2Int[]
    {
        new Vector2Int(-1, 0), // Left
        new Vector2Int(1, 0),  // Right
        new Vector2Int(0, -1), // Down
        new Vector2Int(0, 1)   // up
    };

    public void GenerateGrid()
    {

        grid = new BlockObject[columns, rows];

        Vector3 originPosition = origin.position;
        Transform parent = transform;

        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                Vector3 gridPos = originPosition + new Vector3(x * cellSize.x, y * cellSize.y, 0f);
                BlockData randomData = blockConfig.GetRandomBlock();
                Vector2Int gridIndex = new Vector2Int(x, y);
                BlockObject newBlock = Instantiate(blockPrefab, gridPos, Quaternion.identity, parent);
                newBlock.Setup(randomData, gridIndex);
                newBlock.OnBlockClicked += HandleBlockClicked;
                grid[x, y] = newBlock;
            }
        }
        isRunning = true;
    }

    public void ResetGrid(){

        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                BlockData randomData = blockConfig.GetRandomBlock();
                grid[x, y].UpdateData(randomData.type, randomData.sprite);
            }
        }
        isRunning = true;
        isCollecting = false;
    }

    /// <summary>
    /// A Queue is used to store the empty blocks, when a Color Block is finded on the same column,
    /// it's data is sended to the empty block on the queue. The Color Block is cleared and enqueued.
    /// If at the final of the column loop the queue is not empty, the empty block receives random data.
    /// </summary>
    private void Refill()
    {
        for (int x = 0; x < columns; x++)
        {
            Queue<BlockObject> emptyBlocksQueue = new Queue<BlockObject>();

            for (int y = 0; y < rows; y++)
            {
                BlockObject currentBlock = grid[x, y];

                if (currentBlock.currentType == BlockType.Empty)
                {
                    emptyBlocksQueue.Enqueue(currentBlock);
                }
                else if (emptyBlocksQueue.Count > 0)
                {
                    BlockObject emptyTarget = emptyBlocksQueue.Dequeue();

                    emptyTarget.UpdateData(currentBlock.currentType, currentBlock.spriteRenderer.sprite);

                    currentBlock.ClearBlock();
                    emptyBlocksQueue.Enqueue(currentBlock);
                }
            }

            while (emptyBlocksQueue.Count > 0)
            {
                BlockObject emptyBlock = emptyBlocksQueue.Dequeue();
                BlockData randomData = blockConfig.GetRandomBlock();

                emptyBlock.UpdateData(randomData.type, randomData.sprite);
            }
        }
    }

    public void ClearGrid()
    {
        if (grid == null) return;

        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                if (grid[x, y] != null)
                {
                    Destroy(grid[x, y].gameObject);
                    grid[x, y] = null;
                }
            }
        }
    }

    private void HandleBlockClicked(BlockObject clickedBlock)
    {
        if (isCollecting || !isRunning || clickedBlock == null) return;
        ProcessBlockCollect(clickedBlock);
    }

    /// <summary>
    /// A Queue stores the same color adjacent blocks, this way we can loop through on the queue until it's empty,
    /// add the blocks to a hashmap to avoid storing already visited blocks,
    /// and in a list to count the score and clear the blocks.
    /// </summary>
    private async Task ProcessBlockCollect(BlockObject startBlock){
        isCollecting = true;
        Vector2Int pos = startBlock.gridPosition;
        BlockType targetType = startBlock.currentType;

        Debug.Log($"Block touched ({pos.x}, {pos.y}) type: {targetType}");
    
        if (targetType == BlockType.Empty) 
            return;

        
        Queue<BlockObject> processQueue = new Queue<BlockObject>();
        HashSet<BlockObject> visitedBlocks = new HashSet<BlockObject>();
        List<BlockObject> connectedBlocks = new List<BlockObject>();
        processQueue.Enqueue(startBlock);
        visitedBlocks.Add(startBlock);

        while (processQueue.Count > 0)
        {
            BlockObject currentBlock = processQueue.Dequeue();
            connectedBlocks.Add(currentBlock);
            Vector2Int currentPos = currentBlock.gridPosition;

            foreach (Vector2Int dir in adjacentDirections)
            {
                int neighborX = currentPos.x + dir.x;
                int neighborY = currentPos.y + dir.y;

                if (IsValidGridPosition(neighborX, neighborY))
                {
                    BlockObject neighborBlock = grid[neighborX, neighborY];

                    if (neighborBlock != null && neighborBlock.currentType == targetType &&  !visitedBlocks.Contains(neighborBlock))
                    {
                        visitedBlocks.Add(neighborBlock);
                        processQueue.Enqueue(neighborBlock);
                    }
                }
            }
        }

        
        if (connectedBlocks.Count >= 3)
        {
            foreach (BlockObject blockToClear in connectedBlocks)
            {
                blockToClear.ClearBlock();
            }
            if(OnComboCollected.Invoke(connectedBlocks.Count)) isRunning = false;

            int millisecondsDelay = Mathf.RoundToInt(processDelay * 1000f);
            await Task.Delay(millisecondsDelay);
            Refill();
            
        }
        isCollecting = false;
    }

    private bool IsValidGridPosition(int x, int y)
    {
        return x >= 0 && x < columns && y >= 0 && y < rows;
    }

    

}