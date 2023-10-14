using System.Text;
using TMPro;
using UnityEngine;

public class TextFormatter
{
    private int maxCharactersPerLine;
    private int fontSizeOneLine = 20;
    private int fontSizeTwoLine = 18;
    private int fontSizeTreeLine = 16;
    private int lines;
    public int FontSize 
    {
        get
        {
            switch (lines)
            {
                case 1: return fontSizeOneLine;
                case 2: return fontSizeTwoLine;
                default:
                case 3: return fontSizeTreeLine;
            }
        }
    }

    public TextFormatter(int maxCharactersPerLine)
    {
        this.maxCharactersPerLine = maxCharactersPerLine;
    }

    public string FormatText(string inputText)
    {
        string inputedText = inputText.Replace("\n", " ").Replace("\r", " ");
        string[] words = inputedText.Split(' ');
        lines = 1;
        StringBuilder formattedText = new StringBuilder();
        string currentLine = string.Empty;

        foreach (string word in words)
        {
            if ((currentLine + " " + word).Length <= maxCharactersPerLine)
            {
                if (!string.IsNullOrEmpty(currentLine))
                    currentLine += " ";

                currentLine += word;
            }
            else
            {
                lines ++;
                formattedText.AppendLine(currentLine);
                currentLine = word;
            }
        }

        if (!string.IsNullOrEmpty(currentLine))
            formattedText.AppendLine(currentLine);

        return formattedText.ToString();
    }
}
