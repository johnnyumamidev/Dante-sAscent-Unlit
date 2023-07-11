using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName ="ContainerData")]
public class ContainerData : ScriptableObject
{
    public string containerName;
    public Sprite sprite;

    public List<GameObject> collectables = new List<GameObject>();
}
