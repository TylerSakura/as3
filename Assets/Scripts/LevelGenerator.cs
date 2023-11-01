using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public Sprite outsideCornerSprite;
    public Sprite outsideWallSprite;
    public Sprite insideCornerSprite;
    public Sprite insideWallSprite;
    public Sprite standardPelletSprite;
    public Sprite powerPelletSprite;
    public Sprite tJunctionSprite;

    private int[,] levelMap =
    {
        {1,2,2,2,2,2,2,2,2,2,2,2,2,7},
        {2,5,5,5,5,5,5,5,5,5,5,5,5,4},
        {2,5,3,4,4,3,5,3,4,4,4,3,5,4},
        {2,6,4,0,0,4,5,4,0,0,0,4,5,4},
        {2,5,3,4,4,3,5,3,4,4,4,3,5,3},
        {2,5,5,5,5,5,5,5,5,5,5,5,5,5},
        {2,5,3,4,4,3,5,3,3,5,3,4,4,4},
        {2,5,3,4,4,3,5,4,4,5,3,4,4,3},
        {2,5,5,5,5,5,5,4,4,5,5,5,5,4},
        {1,2,2,2,2,1,5,4,3,4,4,3,0,4},
        {0,0,0,0,0,2,5,4,3,4,4,3,0,3},
        {0,0,0,0,0,2,5,4,4,0,0,0,0,0},
        {0,0,0,0,0,2,5,4,4,0,3,4,4,0},
        {2,2,2,2,2,1,5,3,3,0,4,0,0,0},
        {0,0,0,0,0,0,5,0,0,0,4,0,0,0},
    };

    private void Start()
    {
        GenerateLevel();
    }

    private Vector3 cellSize = new Vector3(1.28f, 1.28f, 0f);
    private void GenerateLevel()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        for (int y = 0; y < levelMap.GetLength(0); y++)
        {
            for (int x = 0; x < levelMap.GetLength(1); x++)
            {
                Sprite spriteToUse = null;
                switch (levelMap[y, x])
                {
                    case 1:
                        spriteToUse = outsideCornerSprite;
                        break;
                    case 2:
                        spriteToUse = outsideWallSprite;
                        break;
                    case 3:
                        spriteToUse = insideCornerSprite; 
                        break;
                    case 4:
                        spriteToUse = insideWallSprite;
                        break;
                    case 5:
                        spriteToUse = standardPelletSprite;
                        break;
                    case 6:
                        spriteToUse = powerPelletSprite;
                        break;
                    case 7:
                        spriteToUse = tJunctionSprite;
                        break;
                }

                if (spriteToUse != null)
                {
                    GameObject tile = new GameObject("Tile_" + x + "_" + y);
                    tile.transform.position = new Vector3(x * cellSize.x - 17.28f, -y * cellSize.y + 18.56f, 0); 
                    SpriteRenderer renderer = tile.AddComponent<SpriteRenderer>();
                    renderer.sprite = spriteToUse;
                    tile.transform.SetParent(transform);
                    RotateTile(x, y, tile);
                }
            }
        }
        MirrorLevel();
        AdjustCamera();
    }
    private void MirrorLevel()
    {
        int width = levelMap.GetLength(1);
        int height = levelMap.GetLength(0);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                GameObject originalTile = GameObject.Find("Tile_" + x + "_" + y);
                if (originalTile != null)
                {
                    Vector3 originalPosition = originalTile.transform.position;
                    Vector3 originalRotation = originalTile.transform.eulerAngles;

                    Vector3 horizontalMirrorPosition = new Vector3(-originalPosition.x, originalPosition.y, originalPosition.z);
                    GameObject horizontalMirror = Instantiate(originalTile, horizontalMirrorPosition, Quaternion.identity);
                    horizontalMirror.name = "Tile_H_" + x + "_" + y;
                    horizontalMirror.transform.SetParent(transform);
                    horizontalMirror.transform.localScale = new Vector3(-1, 1, 1);
                    horizontalMirror.transform.eulerAngles = new Vector3(originalRotation.x, originalRotation.y, -originalRotation.z);

                    Vector3 verticalMirrorPosition = new Vector3(originalPosition.x, -originalPosition.y, originalPosition.z);
                    GameObject verticalMirror = Instantiate(originalTile, verticalMirrorPosition, Quaternion.identity);
                    verticalMirror.name = "Tile_V_" + x + "_" + y;
                    verticalMirror.transform.SetParent(transform);
                    verticalMirror.transform.localScale = new Vector3(1, -1, 1);
                    verticalMirror.transform.eulerAngles = new Vector3(originalRotation.x, originalRotation.y, -originalRotation.z);

                    Vector3 bothMirrorPosition = new Vector3(-originalPosition.x, -originalPosition.y, originalPosition.z);
                    GameObject bothMirror = Instantiate(originalTile, bothMirrorPosition, Quaternion.identity);
                    bothMirror.name = "Tile_HV_" + x + "_" + y;
                    bothMirror.transform.SetParent(transform);
                    bothMirror.transform.localScale = new Vector3(-1, -1, 1);
                    bothMirror.transform.eulerAngles = new Vector3(originalRotation.x, originalRotation.y, originalRotation.z);
                }
            }
        }
    }

    private void RotateTile(int x, int y, GameObject tile)
    {
        int top = y > 0 ? levelMap[y - 1, x] : -1;
        int bottom = y < levelMap.GetLength(0) - 1 ? levelMap[y + 1, x] : -1;
        int left = x > 0 ? levelMap[y, x - 1] : -1;
        int right = x < levelMap.GetLength(1) - 1 ? levelMap[y, x + 1] : -1;

        if (levelMap[y, x] == 3) 
        {
            if (top == 4 && right == 4 && bottom == 3) tile.transform.Rotate(0, 0, 90);
            else if (bottom == 4 && right == 4 && top == 3) tile.transform.Rotate(0, 0, 0);
            else if (bottom == 4 && left == 4 && top == 4) tile.transform.Rotate(0, 0, 270);
            else if ((top == 4 || top == 3) && (right == 4 || right == 3)) tile.transform.Rotate(0, 0, 90);
            else if ((top == 4 || top == 3) && (left != 4 && left != 3)) tile.transform.Rotate(0, 0, 90);
            else if ((top == 4 || top == 3) && (left == 4 || left == 3)) tile.transform.Rotate(0, 0, 180);
            else if ((bottom == 4 || bottom == 3) && (left == 4 || left == 3)) tile.transform.Rotate(0, 0, 270);
        }
        else if (levelMap[y, x] == 4) 
        {
            if ((left == 4 || left == 3) && (right == 4 || right == 3)) tile.transform.Rotate(0, 0, 90);
            else if ((top == 4 || top == 3) && (bottom == 4 || bottom == 3)) tile.transform.Rotate(0, 0, 0);
            else if ((left != 4 && left !=3 && right == 4) || (left == 4 && right != 4 && right != 3)) tile.transform.Rotate(0, 0, 90);
        }
        else if (levelMap[y, x] == 1) 
        {
            if ((top == 2 || top == 1) && (right == 2 || right == 1)) tile.transform.Rotate(0, 0, 90);
            else if ((top == 2 || top == 1) && (left == 2 || left == 1)) tile.transform.Rotate(0, 0, 180);
            else if ((bottom == 2 || bottom == 1) && (left == 2 || left == 1)) tile.transform.Rotate(0, 0, 270);
        }
        else if (levelMap[y, x] == 2)
        {
            if (left == 2 || right == 2) tile.transform.Rotate(0, 0, 90);
        }
        else if (levelMap[y, x] == 7)
        { 
            if (right ==2) tile.transform.Rotate(0, 0, 90);
            else if (top == 2) tile.transform.Rotate(0, 0, 180);
            else if (left == 2) tile.transform.Rotate(0, 0, 270);
        }
    }

    private void AdjustCamera()
    {
        Camera.main.orthographicSize = (levelMap.GetLength(0) * cellSize.y + cellSize.x);
    }
}
