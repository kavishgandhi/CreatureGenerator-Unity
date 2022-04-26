using System;
using System.Globalization;
using UnityEngine;


public class QuadOBJParser
{
    private QuadMeshData parsedData;

    private static readonly int KEYWORD = 0;
    private static readonly int X = 1;
    private static readonly int Y = 2;
    private static readonly int Z = 3;

    private string[] lines;
    private int currLine;

    public QuadMeshData Parse(TextAsset OBJFile)
    {
        lines = OBJFile.text.Split('\n');
        currLine = 0;
        parsedData = new QuadMeshData();

        string[] line;
        while ((line = getNextLine()) != null)
        {
            // Parse vertex position
            if (line[KEYWORD] == "v")
            {
                var vertex = new Vector3(float.Parse(line[X], CultureInfo.InvariantCulture),
                                         float.Parse(line[Y], CultureInfo.InvariantCulture),
                                         float.Parse(line[Z], CultureInfo.InvariantCulture));
                parsedData.vertices.Add(vertex);
            }
            // Parse face vertex indices
            else if (line[KEYWORD] == "f")
            {
                int vertexIndex;
                string vertexIndexString;
                Vector4 quad = new Vector4();
                for (int i = 1; i <= 4; i++)
                {
                    vertexIndexString = line[i];
                    if (vertexIndexString.Contains("//"))
                    {
                        vertexIndexString = vertexIndexString.Split(new string[] { "//" }, StringSplitOptions.None)[0];
                    }

                    // Remove 1 from index, in OBJ format indices start at 1 and not 0
                    vertexIndex = int.Parse(vertexIndexString, CultureInfo.InvariantCulture) - 1;
                    quad[i - 1] = vertexIndex;
                    if (vertexIndex > parsedData.vertices.Count)
                    {
                        PrintError("Vertex index out of bounds");
                    }
                }
                parsedData.quads.Add(quad);
            }
            // Ignore comments and other OBJ data
            else
            {
                // continue
            }
        }
        Debug.Log("Finished Parsing OBJ with " + parsedData.vertices.Count + " vertices " + (parsedData.quads.Count / 4) + " quads");
        return parsedData;
    }

    private string[] getNextLine()
    {
        if (currLine < lines.Length)
        {
            // Trim any leading or trailing whitespace, then split into tokens also by whitespace
            var line = lines[currLine].Trim();
            var tokens = line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
            currLine++;
            return tokens.Length > 0 ? tokens : getNextLine();
        }
        return null;
    }

    private void PrintError(string message)
    {
        Debug.LogError("OBJ line " + currLine + ": " + message);
    }
}
