using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateUVBySpray : MonoBehaviour
{ 
    //todo: this behaviour is functional but needs work, the mapping to UV coordinates is incorrect

    //[SerializeField] Texture2D UVImageBackup;
    Color[] mapPixels;
    Color[,] mapPixelsMatrix;
    MeshRenderer thisRenderer;
    [SerializeField] Texture2D UVImage;
    [SerializeField] Texture2D alphaBaseImage;
    [SerializeField] Texture2D newImageFormatted;

    int imgSize; //limitation: assumes square image 
    float thisBoundX = 0;
    float thisBoundZ = 0; //limitation: assumes mesh is on ground

    // Use this for initialization
    void Start ()
    {
        thisRenderer = this.GetComponent<MeshRenderer>();
        if (!thisRenderer)
        {
            Debug.LogError("Error: mesh renderer not found for paint object");
        }
        else
        {
            UVImage = thisRenderer.material.mainTexture as Texture2D;
            if (!UVImage)
            {
                Debug.LogError("Error: texture not found for paint object");
            }
            else
            {
                newImageFormatted = UVImage;
                //UVImage = UVImageBackup;
            }
        }

        int paintMapRows = UVImage.height;
        int paintMapColumns = UVImage.width;
        mapPixels = UVImage.GetPixels();
        mapPixelsMatrix = new Color[paintMapRows, paintMapColumns];
        for (int n = 0; n < paintMapColumns; n++)
        {
            for (int m = 0; m < paintMapRows; m++)
            {
                mapPixelsMatrix[n, m] = mapPixels[n + m + n * (paintMapColumns - 1)];
            }
        }

        imgSize = UVImage.height;
        thisBoundX = this.transform.position.x - this.GetComponent<MeshCollider>().bounds.size.x;
        thisBoundZ = this.transform.position.z - this.GetComponent<MeshCollider>().bounds.size.z;

        SetAlphaBase();
        UpdateUV();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void SetAlphaBase()
    {
        int paintMapRows = UVImage.height;
        int paintMapColumns = UVImage.width;

        int alphaMapRows = alphaBaseImage.height;
        int alphaMapColumns = alphaBaseImage.width;
        Color[] alphaPixels = alphaBaseImage.GetPixels();
        //Color[,] alphaPixelsMatrix = new Color[alphaMapRows, alphaMapColumns];
        //for (int n = 0; n < alphaMapColumns; n++)
        //{
        //    for (int m = 0; m < alphaMapRows; m++)
        //    {
        //        alphaPixelsMatrix[n, m] = alphaPixels[n + m + n * (alphaMapColumns - 1)];
        //    }
        //}

        Color32 transparentColor = new Color32(0, 0, 0, 0);
        for (int n = 0; n < alphaMapColumns; n++)
        {
            for (int m = 0; m < alphaMapRows; m++)
            {
                if (alphaPixels[n + m + n * (alphaMapColumns - 1)].r == 0
                    && alphaPixels[n + m + n * (alphaMapColumns - 1)].g == 0
                    && alphaPixels[n + m + n * (alphaMapColumns - 1)].b == 0)
                {
                    int alphaToPaintN = Mathf.FloorToInt(((float)n / alphaMapColumns) * paintMapColumns);
                    int alphaToPaintM = Mathf.FloorToInt(((float)m / alphaMapRows) * paintMapRows);
                    Debug.Log("mapping alpha pixel " + alphaToPaintN + " " + alphaToPaintM);
                    mapPixelsMatrix[alphaToPaintN, alphaToPaintM] = transparentColor;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Vector3 strikePoint = other.gameObject.transform.position;
        float sprayRadius = other.bounds.extents.x;

        StrikeUV(strikePoint, sprayRadius);
    }

    void StrikeUV(Vector3 strikePoint, float sprayRadius)
    {
        float XpointHitInImg = ((this.transform.position.x - strikePoint.x) / (this.transform.position.x - thisBoundX)) * imgSize;
        int colHitInImg = Mathf.FloorToInt(XpointHitInImg);
        float ZpointHitInImg = ((this.transform.position.z - strikePoint.z) / (this.transform.position.z - thisBoundZ)) * imgSize;
        int rowHitInImg = Mathf.FloorToInt(ZpointHitInImg);
        float hitRange = (sprayRadius / (thisBoundX - this.transform.position.x)) * imgSize;
        int HitRangeInImg = Mathf.FloorToInt(Mathf.Abs(hitRange));

        //multiple loops used to make crude circle - faster processing
        Color32 newUVColor = new Color32(0, 0, 0, 0);
        for (int n = rowHitInImg - HitRangeInImg; n <= rowHitInImg + HitRangeInImg; n++)
        {
            for (int m = colHitInImg - HitRangeInImg + 2; m <= colHitInImg + HitRangeInImg - 2; m++)
            {
                mapPixelsMatrix[Mathf.Clamp(n, 0, imgSize - 1), Mathf.Clamp(m, 0, imgSize - 1)] = newUVColor;
            }
        }
        for (int n = rowHitInImg - HitRangeInImg + 2; n <= rowHitInImg + HitRangeInImg - 2; n++)
        {
            for (int m = colHitInImg - HitRangeInImg; m <= colHitInImg + HitRangeInImg; m++)
            {
                mapPixelsMatrix[Mathf.Clamp(n, 0, imgSize - 1), Mathf.Clamp(m, 0, imgSize - 1)] = newUVColor;
            }
        }
        for (int n = rowHitInImg - HitRangeInImg + 1; n <= rowHitInImg + HitRangeInImg - 1; n++)
        {
            for (int m = colHitInImg - HitRangeInImg + 1; m <= colHitInImg + HitRangeInImg - 1; m++)
            {
                mapPixelsMatrix[Mathf.Clamp(n, 0, imgSize - 1), Mathf.Clamp(m, 0, imgSize - 1)] = newUVColor;
            }
        }
        UpdateUV();
    }

    void UpdateUV()
    {
        newImageFormatted = new Texture2D(imgSize, imgSize, TextureFormat.ARGB32, false);

        int paintMapRows = UVImage.height;
        int paintMapColumns = UVImage.width;
        for (int n = 0; n < paintMapColumns; n++)
        {
            for (int m = 0; m < paintMapRows; m++)
            {
                newImageFormatted.SetPixel(n, m, mapPixelsMatrix[n, m]);
            }
        }
        newImageFormatted.Apply();

        if (thisRenderer)
        {
            thisRenderer.material.mainTexture = newImageFormatted;
        }
    }
}
