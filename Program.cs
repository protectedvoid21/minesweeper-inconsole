using System.Runtime.InteropServices.ComTypes;

namespace MinesweeperProblem; 

public class Program {
    static void Main() {
        int width = 10;
        int height = 10;

        GameManager gameManager = new GameManager(10, 10, 12);
        Console.Title = "Mineswepr in Console";

        bool isGameOngoing = true;
        while (isGameOngoing) {
            int x, y;
            while (true) {
                Console.Clear();
                gameManager.PrintMap();
                Console.Write("Type your pick to uncover : ");
                string? inputText = Console.ReadLine();
                if (inputText == null) {
                    continue;
                }

                if (inputText is "h" or "H") {
                    gameManager.MarkHint();
                    continue;
                }

                string[] splitText = inputText.Split(' ', StringSplitOptions.TrimEntries)
                    .Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();
                if (splitText.Length != 2) {
                    continue;
                }

                if (string.IsNullOrWhiteSpace(splitText[0]) || string.IsNullOrWhiteSpace(splitText[1])) {
                    continue;
                }

                bool xParsed = int.TryParse(splitText[0], out int xResult);
                bool yParsed = int.TryParse(splitText[1], out int yResult);
                if (!(xParsed && yParsed)) {
                    continue;
                }

                x = xResult;
                y = yResult;

                if (x < 1 || x > width || y < 1 || y > height) {
                    continue;
                }
                break;
            }
            gameManager.Reveal(x - 1, y - 1);
        }
    }
}