using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public delegate void SonarPing(Vector3 sonarSource);
    public static SonarPing sonarPing;
}
