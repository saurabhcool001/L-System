using System.Collections.Generic;
using UnityEngine;

public class SingleRuleLSystemGenerator : MonoBehaviour
{
    // Variables
    public int iterations = 5;
    public float angle = 25.7f;
    public float length = 1f;

    public LineRenderer leaf;

    private LSystemConfig currentConfig;

    // Rule for 'F' and 'G'
    private string rules;     
    private string ruleG;

    private string rulesFilePath = "Rules/rules";
    private string currentString;

    public void AutoStartGenerate(int index)
    {
        LoadRules(index);
        currentString = currentConfig.axiom;
        iterations = currentConfig.iterations;
        angle = currentConfig.angle;
        length = currentConfig.length;
        SystemManager.Instance.UpdateSingleRuleSystemSliders(iterations, angle, length);
        OnStart();
    }

    public void OnStart()
    {
        if (currentConfig == null)
        {
            Debug.LogError("No L-System configuration loaded.");
            return;
        }

        currentString = currentConfig.axiom;

        // Generate the L-System string
        for (int i = 0; i < iterations; i++)
        {
            currentString = Generate(currentString);
        }

        // Configure the LineRenderer
        leaf.positionCount = 0;
        leaf.startWidth = 0.05f;
        leaf.endWidth = 0.05f;
        leaf.numCornerVertices = 5;
        leaf.numCapVertices = 5;

        DrawLSystem();
    }

    private string Generate(string input)
    {
        string result = "";

        foreach (char c in input)
        {
            if (c == 'F')
            {
                result += currentConfig.rule_F;
            }
            else if (c == 'G' && currentConfig.rule_G != null)
            {
                result += currentConfig.rule_G;
            }
            else
            {
                result += c.ToString();
            }
        }

        return result;
    }

    private void DrawLSystem()
    {
        Stack<TransformInfo> transformStack = new Stack<TransformInfo>();
        Vector3 position = Vector3.zero;
        Quaternion rotation = Quaternion.identity;

        List<Vector3> positions = new List<Vector3>();
        positions.Add(position); // Add the starting point

        foreach (char c in currentString)
        {
            switch (c)
            {
                case 'F':
                // Both F and G move forward and draw a line
                case 'G':
                    Vector3 startPosition = position;
                    position += rotation * Vector3.up * length;

                    // Avoid duplicating the last position
                    if (positions[positions.Count - 1] != startPosition)
                    {
                        positions.Add(startPosition);
                    }
                    positions.Add(position);
                    break;

                // Rotate left
                case '+': 
                    rotation *= Quaternion.Euler(0, 0, angle);
                    break;
                // Rotate right
                case '-': 
                    rotation *= Quaternion.Euler(0, 0, -angle);
                    break;
                // Save position and rotation
                case '[': 
                    transformStack.Push(new TransformInfo { Position = position, Rotation = rotation });
                    break;
                // Restore position and rotation
                case ']': 
                    if (transformStack.Count > 0)
                    {
                        TransformInfo ti = transformStack.Pop();
                        position = ti.Position;
                        rotation = ti.Rotation;

                        // Avoid duplicating the restored position
                        if (positions[positions.Count - 1] != position)
                        {
                            positions.Add(position);
                        }
                    }
                    break;
            }
        }

        // Update the LineRenderer
        leaf.positionCount = positions.Count;
        leaf.SetPositions(positions.ToArray());
    }

    private void LoadRules(int index)
    {
        TextAsset jsonFile = Resources.Load<TextAsset>(rulesFilePath);

        if (jsonFile != null)
        {
            LSystemsCollection collection = JsonUtility.FromJson<LSystemsCollection>(jsonFile.text);

            if (collection.LSystems.Count > index)
            {
                currentConfig = collection.LSystems[index];
                rules = currentConfig.rule_F;
                ruleG = currentConfig.rule_G;
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

    private struct TransformInfo
    {
        public Vector3 Position;
        public Quaternion Rotation;
    }
}

[System.Serializable]
public class LSystemsCollection
{
    public List<LSystemConfig> LSystems;
}

[System.Serializable]
public class LSystemConfig
{
    public int index;
    public string axiom;
    public int iterations;
    public float angle;
    public float length;
    public string rule_F;
    public string rule_X;
    public string rule_G;
}
