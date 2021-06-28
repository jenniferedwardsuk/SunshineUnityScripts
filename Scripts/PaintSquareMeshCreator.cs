using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PaintSquareMeshCreator : MonoBehaviour {

    public float squarewidth = 1;
    Mesh mesh;
    int RowsToGenerate = 0;
    int ColumnsToGenerate = 0;
    public Texture2D paintMap;

    Vector3[] vertices;
    int[] tri;
    Vector3[] normals;
    Vector2[] uv;

    //List<Vector3> newVertices;
    //List<Vector3> newNormals;
    //List<Vector2> newUv;
    List<int> newTris;
    List<Vector3> splitTris;

    void Start () {
        MeshFilter mf = GetComponent<MeshFilter>();
        mesh = new Mesh();
        mf.mesh = mesh;
        mesh.name = "PaintMesh";

        int paintMapRows = paintMap.height;
        int paintMapColumns = paintMap.width;
        Color[] mapPixels = paintMap.GetPixels();
        Color[,] mapPixelsMatrix = new Color[paintMapRows, paintMapColumns];
        //if (paintMapRows > 100 || paintMapColumns > 100) //not required now that texture modification is used rather than mesh cutting
        //{
        //    Debug.Log("Paint mesh not generated. Paint map needs to be < 100x100px for performance reasons.");
        //    return;
        //}
        for (int n = 0; n < paintMapColumns; n++)
        {
            for (int m = 0; m < paintMapRows; m++)
            {
                mapPixelsMatrix[n, m] = mapPixels[n + m + n * (paintMapColumns - 1)];
            }
        }

        RowsToGenerate = paintMap.height;
        ColumnsToGenerate = paintMap.width;

        vertices = new Vector3[4 * RowsToGenerate * ColumnsToGenerate];
        normals = new Vector3[4 * RowsToGenerate * ColumnsToGenerate];
        uv = new Vector2[4 * RowsToGenerate * ColumnsToGenerate];

        Vector3 unitRowOffset = new Vector3(0, squarewidth, 0);
        Vector3 RowOffset = unitRowOffset;
        Vector3 unitColumnOffset = new Vector3(squarewidth, 0, 0);
        Vector3 ColumnOffset = unitColumnOffset;

        for (int n = 0; n < RowsToGenerate * 4; n += 4)
        {
            for (int m = 0; m < ColumnsToGenerate * 4; m += 4)
            {
                RowOffset = unitRowOffset * ((n / 4) + 1);
                ColumnOffset = unitColumnOffset * ((m / 4) + 1);
                
                vertices[0 + n + m + n * (ColumnsToGenerate - 1)] = new Vector3(0, 0, 0) + RowOffset + ColumnOffset;
                vertices[1 + n + m + n * (ColumnsToGenerate - 1)] = new Vector3(squarewidth, 0, 0) + RowOffset + ColumnOffset;
                vertices[2 + n + m + n * (ColumnsToGenerate - 1)] = new Vector3(0, squarewidth, 0) + RowOffset + ColumnOffset;
                vertices[3 + n + m + n * (ColumnsToGenerate - 1)] = new Vector3(squarewidth, squarewidth, 0) + RowOffset + ColumnOffset;
                
                normals[0 + n + m + n * (ColumnsToGenerate - 1)] = -Vector3.forward;
                normals[1 + n + m + n * (ColumnsToGenerate - 1)] = -Vector3.forward;
                normals[2 + n + m + n * (ColumnsToGenerate - 1)] = -Vector3.forward;
                normals[3 + n + m + n * (ColumnsToGenerate - 1)] = -Vector3.forward;

                float ColsToGen = ColumnsToGenerate;
                float RowsToGen = RowsToGenerate;
                float overallPointX = (n / 4) / (ColsToGen - 1);
                float overallPointY = (m / 4) / (RowsToGen - 1);
                uv[0 + n + m + n * (ColumnsToGenerate - 1)] = new Vector2(overallPointX, overallPointY);
                uv[1 + n + m + n * (ColumnsToGenerate - 1)] = new Vector2(overallPointX, overallPointY + (1 / RowsToGen));
                uv[2 + n + m + n * (ColumnsToGenerate - 1)] = new Vector2(overallPointX + (1 / ColsToGen), overallPointY);
                uv[3 + n + m + n * (ColumnsToGenerate - 1)] = new Vector2(overallPointX + (1 / ColsToGen), overallPointY + (1 / RowsToGen));
            }
        }

        int triPointCount = 6;
        tri = new int[triPointCount * RowsToGenerate * ColumnsToGenerate];
        for (int n = 0; n < Mathf.Min(RowsToGenerate * triPointCount, tri.Length); n += triPointCount)
        {
            for (int m = 0; m < ColumnsToGenerate * triPointCount; m += triPointCount)
            {
                // only generate tris if the alpha map is bright at this pixel
                if (mapPixelsMatrix[n / triPointCount, m / triPointCount].grayscale >= 0.5f)
                {
                    int verticeOffset = (n / triPointCount) * 4 + (m / triPointCount) * 4 + ((n / triPointCount) * 4) * (ColumnsToGenerate - 1);
                    tri[n + 0 + m + n * (ColumnsToGenerate - 1)] = 0 + verticeOffset;
                    tri[n + 1 + m + n * (ColumnsToGenerate - 1)] = 2 + verticeOffset;
                    tri[n + 2 + m + n * (ColumnsToGenerate - 1)] = 3 + verticeOffset;

                    tri[n + 3 + m + n * (ColumnsToGenerate - 1)] = 0 + verticeOffset;
                    tri[n + 4 + m + n * (ColumnsToGenerate - 1)] = 3 + verticeOffset;
                    tri[n + 5 + m + n * (ColumnsToGenerate - 1)] = 1 + verticeOffset;

                    //commented: this slows generation and is unneeded at present, but would allow smoother edges with tri removal if smoothing necessary. update tripointcount to 12 if used.
                    //tri[n + 6 + m + n * (ColumnsToGenerate - 1)] = 0 + verticeOffset;
                    //tri[n + 7 + m + n * (ColumnsToGenerate - 1)] = 2 + verticeOffset;
                    //tri[n + 8 + m + n * (ColumnsToGenerate - 1)] = 1 + verticeOffset;

                    //tri[n + 9 + m + n * (ColumnsToGenerate - 1)] = 2 + verticeOffset;
                    //tri[n + 10 + m + n * (ColumnsToGenerate - 1)] = 3 + verticeOffset;
                    //tri[n + 11 + m + n * (ColumnsToGenerate - 1)] = 1 + verticeOffset;
                }
            }
        }
        splitUpTris();

        //set mesh properties
        mesh.vertices = vertices;
        mesh.triangles = tri;
        mesh.normals = normals;
        mesh.uv = uv;

        this.GetComponent<MeshCollider>().sharedMesh = mesh;
        this.GetComponent<MeshCollider>().enabled = true;
    }

    public void removeTrisForVertexHit(int rowNum, int colNum)
    {
        // full removal is too slow - instead of removing tris, hit tris are hidden by setting them to vertice 0

        // refresh tri lists
        //newTris = tri.ToList();
        //splitUpTris();

        // flag tris for vertex
        flagMeshTrisForIndex(rowNum, colNum);    
        
        // remove flagged tris 
        //removeFlaggedTris();
    }

    void splitUpTris()
    {
        splitTris = new List<Vector3>();
        for (int n = 0; n < tri.Length; n = n + 3)
        {
            splitTris.Add(new Vector3(tri[n], tri[n + 1], tri[n + 2]));
        }
    }
    
    void flagMeshTrisForIndex(int rowNum, int colNum)
    {
        int verticeNum0 = 0 + ((rowNum) * (ColumnsToGenerate) * 4) + ((colNum) * 4);

        for (int p = 0; p < splitTris.Count; p++)
        {
            if (splitTris[p].x == verticeNum0 || splitTris[p].y == verticeNum0 || splitTris[p].z == verticeNum0
            )
            {
                //newTris
                tri[p * 3] = 0;
                tri[p * 3 + 1] = 0;
                tri[p * 3 + 2] = 0;
            }
        }

        //tri = newTris.ToArray();
        mesh.triangles = tri;
        this.GetComponent<MeshFilter>().mesh = mesh;
    }

    void removeFlaggedTris()
    {
        //List<int> newTris2 = newTris.FindAll(x => x != -1);
        //newTris = newTris2;

        for (int n = newTris.Count - 1; n >= 0; n--)
        {
            if (newTris[n] == -1)
            {
                newTris.RemoveAt(n);
            }
        }

        tri = null;
        tri = newTris.ToArray();
        
        mesh.triangles = tri;

        this.GetComponent<MeshFilter>().mesh = mesh;

    }
    
    void Update () {
		
	}
    
}
