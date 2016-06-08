using UnityEngine;
using System.Collections;
using System.IO;

public class CreateSpriteFile : MonoBehaviour {

	void Start () {
        Sprite sprite = (Sprite) Resources.Load("unity_builtin_extra/DropdownArrow");
        byte[] bytes = sprite.texture.EncodeToPNG();
        File.WriteAllBytes("C:/Users/Mathias/Desktop/Sprites/dropdown.png", bytes);
    }
	
	void Update () {
	
	}
}
