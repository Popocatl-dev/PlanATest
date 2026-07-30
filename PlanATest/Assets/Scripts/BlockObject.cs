using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class BlockObject : MonoBehaviour
{
    public BlockType currentType { get; private set; } 
    public SpriteRenderer spriteRenderer { get; private set; } 
    public Vector2Int gridPosition { get; private set; } 

    /// <summary>
    /// Event for handling when a block is clicked or touched, sending itself to the GridManager 
    /// </summary>
    public delegate void BlockClickedHandler(BlockObject clickedBlock);
    public event BlockClickedHandler OnBlockClicked;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Setup(BlockData data, Vector2Int position)
    {
        currentType = data.type;
        spriteRenderer.sprite = data.sprite;
        gridPosition = position;
    }

    public void UpdateData(BlockType newtype, Sprite newsprite)
    {
        currentType = newtype;
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = true;
            spriteRenderer.sprite = newsprite;
        }
        
    }

    public void SetGridPosition(Vector2Int position)
    {
        gridPosition = position;
    }

    public void ClearBlock()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false;
        }
        currentType = BlockType.Empty;
    }

    private void OnMouseDown()
    {
        OnBlockClicked?.Invoke(this);
    }
}