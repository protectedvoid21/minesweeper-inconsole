namespace MinesweeperProblem;

public class Block {
    public int number;
    public bool isMine;
    public bool uncovered;

    public void Draw() {
        if (!uncovered) {
            Console.Write("■ ");
            return;
        }
        if (isMine) {
            Console.Write("X ");
            return;
        }
        if (number == 0) {
            Console.Write("  ");
            return;
        }
        Console.Write(number + " ");
    }
}