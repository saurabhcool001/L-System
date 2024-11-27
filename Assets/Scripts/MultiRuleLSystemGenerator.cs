using System.Collections.Generic;
using UnityEngine;

public class MultiRuleLSystemGenerator : MonoBehaviour
{
    public int iterations = 7; // Number of iterations
    public float angle = 20f; // Angle in degrees
    public float length = 0.3f; // Length of each segment
    public float lengthReductionFactor = 0.7f; // Reduce length with each iteration
    public Material lineMaterial; // Material for LineRenderer
    public float lineWidth = 0.01f;
    public LineRenderer root;

    private LSystemConfig currentConfig;

    private string rule_F;
    private string rule_X;

    private string rulesFilePath = "Rules/rules";

    private string currentString;

    private string axiom = "X";

    private List<Vector3> positions = new List<Vector3>();

     public void AutoStartGenerate(int index)
    {
        LoadRules(index);
        currentString = currentConfig.axiom;
        iterations = currentConfig.iterations;
        angle = currentConfig.angle;
        length = currentConfig.length;
        SystemManager.Instance.UpdateMultieRuleSystemSliders(iterations, angle, length);
        OnStart();
    }

    public void OnStart()
    {
        currentString = axiom;

        // Generate the L-System string
        for (int i = 0; i < iterations; i++)
        {
            currentString = Generate(currentString);
        }

        // Draw the L-System
        DrawLSystem();
    }

    private string Generate(string input)
    {
        string result = "";

        foreach (char c in input)
        {
            if (c == 'X') 
                result += rule_X;
            else if (c == 'F') 
                result += rule_F;
            else
                result += c.ToString();
        }

        return result;
    }

    private void DrawLSystem()
    {
        Stack<TransformInfo> transformStack = new Stack<TransformInfo>();
        Vector3 position = Vector3.zero;
        Quaternion rotation = Quaternion.identity;

        float currentLength = length;

        positions.Clear();
        // Starting position
        positions.Add(position);

        foreach (char c in currentString)
        {
            switch (c)
            {
                // Move forward and draw a line
                case 'F': 
                    Vector3 startPosition = position;
                    position += rotation * Vector3.up * currentLength;
                    // Add new position to the list
                    positions.Add(position);
                    break;
                // Turn right
                case '+':
                    rotation *= Quaternion.Euler(0, 0, angle);
                    break;
                // Turn left
                case '-': 
                    rotation *= Quaternion.Euler(0, 0, -angle);
                    break;
                // Push current state to the stack
                case '[': 
                    transformStack.Push(new TransformInfo { Position = position, Rotation = rotation, Length = currentLength });
                    break;
                // Pop the state from the stack
                case ']': 
                    if (transformStack.Count > 0)
                    {
                        TransformInfo ti = transformStack.Pop();
                        position = ti.Position;
                        rotation = ti.Rotation;
                        currentLength = ti.Length;
                        // Add position for discontinuity
                        positions.Add(position); 
                    }
                    break;
            }
        }

        // Render the lines function
        RenderLines(root);
    }

    private void RenderLines(LineRenderer root)
    {
        LineRenderer lineRenderer = root;
        lineRenderer.positionCount = positions.Count;
        lineRenderer.SetPositions(positions.ToArray());
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.material = lineMaterial;
        lineRenderer.useWorldSpace = true;
    }

    private struct TransformInfo
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public float Length;
    }
     private void LoadRules(int index)
    {
        // Load the JSON file from Resources folder
        TextAsset jsonFile = Resources.Load<TextAsset>(rulesFilePath);

        if (jsonFile != null)
        {
            // Parse the JSON into a collection of LSystemConfig objects
            LSystemsCollection collection = JsonUtility.FromJson<LSystemsCollection>(jsonFile.text);

            if (collection.LSystems.Count > index)
            {
                currentConfig = collection.LSystems[index];
                rule_F = currentConfig.rule_F;
                rule_X = currentConfig.rule_X;
            }
            else
            {
                Debug.LogError("Invalid index for L-System configuration.");
            }
        }
        else
        {
            Debug.LogError("Rules file not found!");
        }
    }
}