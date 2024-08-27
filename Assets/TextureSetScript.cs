using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class TextureSetScript : MonoBehaviour
{
    public Texture2D tex;

    private bool _shouldApply;
    
    private void Start()
    {
        for (int i = 0; i < tex.width; i++)
        {
            for (int j = 0; j < tex.height; j++)
            {
                tex.SetPixel(i, j, Color.black);
                
            }
        }
        tex.Apply();
        
        Camera.main.orthographicSize = 80;

        InvokeRepeating(nameof(ApplyTex), 0, .5f);
        
        
    }

    private void ApplyTex()
    {
        if(!_shouldApply) return;
        tex.Apply();
    }
    
    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 pos = Input.mousePosition; // Mouse position
            RaycastHit2D[] hits;
            Camera _cam = Camera.main; // Camera to use for raycasting
            Vector2 ray = _cam.ScreenToWorldPoint(pos);
            hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            //Color c;
            foreach (RaycastHit2D hit in hits)
            {
                if (!hit.collider.gameObject.CompareTag("Ground")) continue;
                Vector2 shotPos = hit.point - new Vector2(hit.collider.transform.position.x-100,hit.collider.transform.position.y-100);
                //Debug.Log(shotPos/200);
                for (int i = 0; i < 50; i++)
                {
                    for (int j = 0; j < 50; j++)
                    {
                        tex.SetPixel((int)(shotPos.x * tex.width) / 200 + i, (int)(shotPos.y * tex.height) / 200 + j,
                            new Color(255, 0, 0, 50));
                    }
                }

                _shouldApply = true;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            _shouldApply = false;
            SaveTexture(tex);
        }
    }
    
    protected void SaveTexture(Texture2D texture)
    {
        //string path =  Application.dataPath + "/../" + AssetDatabase.GetAssetPath( tex );
        //File.WriteAllBytes(path, texture.EncodeToPNG());
    }
	
    protected Texture2D RetriveTexture(Texture2D texture)
    {
		
        Texture2D newTexture = new Texture2D(texture.width, texture.height);

        string path = Application.persistentDataPath + "/" + texture.name;
        if (File.Exists(path)){
            newTexture.LoadImage(File.ReadAllBytes(path));
        } else {
            newTexture = texture;
        }

        return newTexture;
    }
}
