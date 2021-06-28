using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutBySpray : MonoBehaviour {

    GameObject waterPrefab;
    Mesh thisMesh;
    PaintSquareMeshCreator thisMeshCreator;
    int meshSize;
    Vector3[,] verticesMatrix;
    float thisBoundX = 0;
    float thisBoundZ = 0; //limitation: assumes mesh is on ground
    bool populatedVertices = false;

    void Start ()
    {
        thisMeshCreator = this.GetComponent<PaintSquareMeshCreator>();
        if (thisMeshCreator)
        {
            meshSize = thisMeshCreator.paintMap.height;
            verticesMatrix = new Vector3[meshSize, meshSize]; // limitation: square map and one-to-one resolution expected
        }
        else
        {
            Debug.Log("Mesh creator not found for cut script setup");
        }
    }
	
	void Update ()
    {
        if (!thisMesh)
        {
            thisMesh = GetComponent<MeshFilter>().mesh;
        }
        if (!populatedVertices && thisMesh.vertices.Length > 0)
        {
            populateVerticesMatrix();
            //Debug.Log("populating paint vertex matrix");
        }

        if (thisMesh.vertices.Length > 0 && thisBoundX == 0)
        {
            //mesh extends negatively along x and z
            thisBoundX = this.transform.position.x - this.GetComponent<MeshCollider>().bounds.size.x; 
            thisBoundZ = this.transform.position.z - this.GetComponent<MeshCollider>().bounds.size.z;
            //Debug.Log("mesh extent X is " + this.GetComponent<MeshCollider>().bounds.size.x);
            //Debug.Log("paint bound X is " + thisBoundX + ", Z bound is " + thisBoundZ + " and position X and Z are " + this.transform.position.x + " and " + this.transform.position.z);
        }
        else if (thisBoundX == 0)
        {
            Debug.Log("Collider not found for cut script setup");
        }
    }

    void populateVerticesMatrix()
    {
        for (int n = 0; n < meshSize; n++)
        {
            for (int m = 0; m < meshSize; m++)
            {
                verticesMatrix[n, m] = thisMesh.vertices[n + m + n * (meshSize - 1)];
            }
        }
        populatedVertices = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        Vector3 strikePoint = other.gameObject.transform.position;
        float sprayRadius = other.bounds.extents.x;

        CutMesh(strikePoint, sprayRadius);
    }

    void CutMesh(Vector3 strikePoint, float sprayRadius)
    {
        if (thisMesh)
        {
            
            if (!thisMeshCreator)
            {
                Debug.Log("Error: mesh creator not found for mesh cut");
            }
            else
            {
                //transform to mesh space
                float XpointHitInMesh = ((this.transform.position.x - strikePoint.x) / (this.transform.position.x - thisBoundX)) * meshSize;
                int colHitInMesh  = Mathf.FloorToInt(XpointHitInMesh);
                float ZpointHitInMesh = ((this.transform.position.z - strikePoint.z) / (this.transform.position.z - thisBoundZ)) * meshSize;
                int rowHitInMesh = Mathf.FloorToInt(ZpointHitInMesh);
                float hitRange = (sprayRadius / (thisBoundX - this.transform.position.x)) * meshSize;
                int HitRangeInMesh = Mathf.FloorToInt(Mathf.Abs(hitRange));

                //thisMeshCreator.removeTrisForVertexHit(rowHitInMesh, colHitInMesh);

                //multiple loops used to make crude circle - faster processing
                for (int n = rowHitInMesh - HitRangeInMesh; n <= rowHitInMesh + HitRangeInMesh; n++)
                {
                    for (int m = colHitInMesh - HitRangeInMesh + 2; m <= colHitInMesh + HitRangeInMesh - 2; m++)
                    {
                        thisMeshCreator.removeTrisForVertexHit(n, m);
                    }
                }
                for (int n = rowHitInMesh - HitRangeInMesh + 2; n <= rowHitInMesh + HitRangeInMesh - 2; n++)
                {
                    for (int m = colHitInMesh - HitRangeInMesh; m <= colHitInMesh + HitRangeInMesh; m++)
                    {
                        thisMeshCreator.removeTrisForVertexHit(n, m);
                    }
                }
                for (int n = rowHitInMesh - HitRangeInMesh + 1; n <= rowHitInMesh + HitRangeInMesh - 1; n++)
                {
                    for (int m = colHitInMesh - HitRangeInMesh + 1; m <= colHitInMesh + HitRangeInMesh - 1; m++)
                    {
                        thisMeshCreator.removeTrisForVertexHit(n, m);
                    }
                }
            }
        }
        else
        {
            Debug.Log("Error: paint mesh not found for cut");
        }

    }

}
