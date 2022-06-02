using System.Drawing;

namespace MinesweeperProblem;

public class GameManager {
    private const int Top = 1;
    private const int Bottom = -1;
    private const int Left = -1;
    private const int Right = 1;

    private readonly int width;
    private readonly int height;

    private readonly Block[,] blocks;

    public GameManager(int width, int height, int bombs) {
        blocks = new Block[width, height];

        this.width = width;
        this.height = height;

        for(int x = 0; x < width; x++) {
            for(int y = 0; y < height; y++) {
                blocks[x, y] = new Block();
            }
        }

        int bombsLeft = bombs;
        Random random = new();
        while(bombsLeft > 0) {
            int randomX = random.Next(width - 1);
            int randomY = random.Next(height - 1);

            if(!blocks[randomX, randomY].isMine) {
                blocks[randomX, randomY].isMine = true;
                bombsLeft--;
            }
        }

        for(int x = 0; x < width; x++) {
            for(int y = 0; y < height; y++) {
                if(!blocks[x, y].isMine) {
                    continue;
                }

                if(x + Right != width && y + Top != height) { // Upper right
                    if(blocks[x + Right, y + Top].isMine == false) {
                        blocks[x + Right, y + Top].number++;
                    }
                }
                if(x + Left >= 0 && y + Top != height) { // Upper left
                    if(blocks[x + Left, y + Top].isMine == false) {
                        blocks[x + Left, y + Top].number++;
                    }
                }
                if(x + Left >= 0 && y + Bottom >= 0) { // Lower left
                    if(blocks[x + Left, y + Bottom].isMine == false) {
                        blocks[x + Left, y + Bottom].number++;
                    }
                }
                if(x + Right != width && y + Bottom >= 0) { // Lower right
                    if(blocks[x + Right, y + Bottom].isMine == false) {
                        blocks[x + Right, y + Bottom].number++;
                    }
                }

                if(y + Top != height) {
                    if(blocks[x, y + Top].isMine == false) {
                        blocks[x, y + Top].number++;
                    }
                }
                if(y + Bottom >= 0) {
                    if(blocks[x, y + Bottom].isMine == false) {
                        blocks[x, y + Bottom].number++;
                    }
                }
                if(x + Right != width) {
                    if(blocks[x + Right, y].isMine == false) {
                        blocks[x + Right, y].number++;
                    }
                }
                if(x + Left >= 0) {
                    if(blocks[x + Left, y].isMine == false) {
                        blocks[x + Left, y].number++;
                    }
                }
            }
        }
    }

    public void PrintMap() {
        for(int y = 0; y < blocks.GetLength(0); y++) {
            for(int x = 0; x < blocks.GetLength(1); x++) {
                blocks[x, y].Draw();
            }
            Console.WriteLine();
        }
    }

    public void GameOver() {
        Console.ForegroundColor = ConsoleColor.Red;
    }

    public void Reveal(int x, int y) {
        if(blocks[x, y].uncovered) {
            return;
        }
        blocks[x, y].uncovered = true;
        if (blocks[x, y].isMine) {
            GameOver();
            return;
        }
        if(blocks[x, y].number == 0) {
            RevealClearRegion(x, y);
        }
    }

    private void RevealClearRegion(int x, int y) {
        if(x + 1 < width && y + 1 < height) { // Upper right
            Reveal(x + Right, y + Top);
        }
        if(x > 0 && y + 1 < height) { // Upper left
            Reveal(x + Left, y + Top);
        }
        if(x > 0 && y > 0) { // Lower left
            Reveal(x + Left, y + Bottom);
        }
        if(x + 1 < width && y > 0) { // Lower right
            Reveal(x + Right, y + Bottom);
        }
        if(y + 1 < height) {
            Reveal(x, y + Top);
        }
        if(y > 0) {
            Reveal(x, y + Bottom);
        }
        if(x + 1 < width) {
            Reveal(x + Right, y);
        }
        if(x > 0) {
            Reveal(x + Left, y);
        }
    }

    public void MarkHint() {
        List<int[]> blocksToReveal = new();
        for(int x = 0; x < width; x++) {
            for(int y = 0; y < height; y++) {
                if (!blocks[x, y].isMine && blocks[x, y].number > 0) {
                    if (GetNeighborUnclickedBlockCount(x, y, out List<int[]> neighborBlocks) == blocks[x, y].number) {
                        blocksToReveal.AddRange(neighborBlocks);
                    }
                }
            }
        }

        foreach (var block in blocksToReveal) {
            Reveal(block[0], block[1]);
        }
    }

    private int GetNeighborUnclickedBlockCount(int x, int y, out List<int[]> neighborBlocks) {
        int blockCount = 0;
        neighborBlocks = new List<int[]>();

        if(x > 0) {
            if(!blocks[x + Left, y].uncovered) {
                neighborBlocks.Add(new int[] { x + Left, y });
                blockCount++;
            }

            if(y > 0) {
                if(!blocks[x + Left, y + Bottom].uncovered) {
                    neighborBlocks.Add(new int[] { x + Left, y + Bottom });
                    blockCount++;
                }
            }
            if(y + 1 < height) {
                if(!blocks[x + Left, y + Top].uncovered) {
                    neighborBlocks.Add(new int[] { x + Left, y + Top });
                    blockCount++;
                }
            }
        }
        if(x + 1 < width) {
            if(!blocks[x + Right, y].uncovered) {
                neighborBlocks.Add(new int[] { x + Right, y });
                blockCount++;
            }

            if(y > 0) {
                if(!blocks[x + Right, y + Bottom].uncovered) {
                    neighborBlocks.Add(new int[] { x + Right, y + Bottom });
                    blockCount++;
                }
            }
            if(y + 1 < height) {
                if(!blocks[x + Right, y + Top].uncovered) {
                    neighborBlocks.Add(new int[] { x + Right, y + Top });
                    blockCount++;
                }
            }
        }

        if(y > 0) {
            if(!blocks[x, y + Bottom].uncovered) {
                neighborBlocks.Add(new int[] { x, y + Bottom });
                blockCount++;
            }
        }
        if(y + 1 < height) {
            if(!blocks[x, y + Top].uncovered) {
                neighborBlocks.Add(new int[] { x, y + Top });
                blockCount++;
            }
        }
        return blockCount;
    }
}