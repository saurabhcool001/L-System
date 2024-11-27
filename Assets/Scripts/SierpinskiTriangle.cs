using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Sperpin : MonoBehaviour
{
    public string axiom = "F-G-G";
    public int iterations = 4;
    public float angle = 120f;
    public float length = 1f;

    private string currentString;

    public void OnStart()
    {
        currentString = GenerateLSystem(axiom, iterations);
        DrawLSystem(currentString, angle, length);
    }

    string GenerateLSystem(string axiom, int iterations)
    {
        string current = axiom;
        for (int i = 0; i < iterations; i++)
        {
            StringBuilder next = new StringBuilder();
            foreach (char c in current)
            {
                if (c == 'F')
                    next.Append("F-G+F+G-F");
                else if (c == 'G')
                    next.Append("GG");
                else
                    next.Append(c);
            }
            current = next.ToString();
        }
        return current;
    }

    void DrawLSystem(string instructions, float angle, float length)
    {
        Stack<Vector3> positionStack = new Stack<Vector3>();
        Stack<float> rotationStack = new Stack<float>();

        Vector3 position = Vector3.zero;
        float rotation = 0f;

        foreach (char c in instructions)
        {
            if (c == 'F' || c == 'G')
            {
                Vector3 newPosition = position + Quaternion.Euler(0, 0, rotation) * Vector3.right * length;
                Debug.DrawLine(position, newPosition, Color.white, 10000f); // Unity Gizmos for visualization
                position = newPosition;
            }
            else if (c == '+')
            {
                rotation += angle;
            }
            else if (c == '-')
            {
                rotation -= angle;
            }
            else if (c == '[')
            {
                positionStack.Push(position);
                rotationStack.Push(rotation);
            }
            else if (c == ']')
            {
                if (positionStack.Count > 0) position = positionStack.Pop();
                if (rotationStack.Count > 0) rotation = rotationStack.Pop();
            }
        }
    }
}
