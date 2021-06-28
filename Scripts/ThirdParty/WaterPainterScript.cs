using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Generate decals
/// Script posted by Damien Mayance on dmayance.com. Some edits made.
/// </summary>
public class WaterPainterScript : MonoBehaviour
{
    public static WaterPainterScript Instance;
    public FluddController Fludd;

    /// <summary>
    /// A single paint decal to instantiate
    /// </summary>
    public Transform PaintPrefab;
    public Transform spraySource;
    public Transform secondarySource;

    public int maxRange;
    public float fireRate = 0.1f;
    float fireRateTimer;

    private int MinSplashs = 0;
    private int MaxSplashs = 0;
    private float SplashRange = 0.5f;

    private float MinScale = 1f;
    private float MaxScale = 1f;

    // DEBUG
    private bool mDrawDebug;
    private Vector3 mHitPoint;
    private List<Ray> mRaysDebug = new List<Ray>();

    void Awake()
    {
        if (Instance != null) Debug.LogError("More than one Painter has been instantiated in this scene!");
        Instance = this;

        if (PaintPrefab == null) Debug.LogError("Missing Paint decal prefab!");

        fireRateTimer = fireRate;
    }

    void Update()
    {
        if (fireRateTimer > 0)
        {
            fireRateTimer -= Time.deltaTime;
        }
        else
        {
            fireRateTimer = fireRate;
            // Check for a click
            if (Input.GetMouseButton(0))
            {
                // Raycast
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    spraySource.transform.LookAt(hit.point);
                    spraySource.transform.Rotate(new Vector3(0, 180, 0)); // fludd faces backward

                    Vector3 hitFlatPoint = hit.point;
                    hitFlatPoint.y = secondarySource.position.y;
                    secondarySource.transform.LookAt(hitFlatPoint);

                    if (Fludd.WaterLevel > 0)
                    {
                        int floorMask = LayerMask.GetMask("Floor");
                        RaycastHit hit2;
                        if (Physics.Raycast(spraySource.transform.position, spraySource.transform.forward * -1, out hit2, 100f, floorMask))
                        {
                            //todo: make paint 'fall short' if hit is too distant - int maxRange

                            Fludd.WaterLevel -= 1;
                            // Paint!
                            // Step back a little for a better effect (that's what "normal * x" is for)
                            //Paint(hit.point + hit.normal * (SplashRange / 4f));
                            PaintOne(hit.point + hit.normal * (SplashRange / 4f));
                        }
                    }
                    else
                    {
                        Fludd.WaterLevel = 0;
                    }
                }
            }
        }
    }

    public void PaintOne(Vector3 location)
    {  
        int floorMask = LayerMask.GetMask("Floor");
        
        var paintSplatter = GameObject.Instantiate(PaintPrefab, location, Quaternion.FromToRotation(Vector3.back, Vector3.up)) as Transform;
        paintSplatter.transform.position -= paintSplatter.forward.normalized * 0.05f;

        paintSplatter.GetComponent<ShrinkOverTime>().enabled = true;
            
        // kill decal after some time
        Destroy(paintSplatter.gameObject, 1);
    }

    public void Paint(Vector3 location)
    {
        //DEBUG
        mHitPoint = location;
        mRaysDebug.Clear();
        mDrawDebug = true;

        int n = -1;

        int drops = Random.Range(MinSplashs, MaxSplashs);
        RaycastHit hit;

        // Generate multiple decals in once
        while (n <= drops)
        {
            n++;

            // Get a random direction (beween -n and n for each vector component)
            var fwd = transform.TransformDirection(Random.onUnitSphere * SplashRange);

            mRaysDebug.Add(new Ray(location, fwd));

            // Raycast around the position to splash everwhere we can
            int floorMask = LayerMask.GetMask("Floor");
            if (Physics.Raycast(location, fwd, out hit, SplashRange, floorMask))
            {
                // Create a splash if we found a surface
                var paintSplatter = GameObject.Instantiate(PaintPrefab,
                                                           hit.point,
                                                           // Rotation from the original sprite to the normal
                    // Prefab are currently oriented to z+ so we use the opposite
                                                           Quaternion.FromToRotation(Vector3.back, hit.normal)
                                                           ) as Transform;

                paintSplatter.transform.position -= paintSplatter.forward.normalized * 0.05f; // added: offset from surface to avoid partial occlusion

                paintSplatter.GetComponent<ShrinkOverTime>().enabled = true;

                // Random scale
                var scaler = Random.Range(MinScale, MaxScale);

                paintSplatter.localScale = new Vector3(
                    paintSplatter.localScale.x * scaler,
                    paintSplatter.localScale.y * scaler,
                    paintSplatter.localScale.z
                );

                // Random rotation effect
                //var rater = Random.Range(0, 359);
                //paintSplatter.transform.RotateAround(hit.point, hit.normal, rater);


                // kill decal after some time
                Destroy(paintSplatter.gameObject, 1);
            }

        }
    }

    void OnDrawGizmos()
    {
        // DEBUG
        if (mDrawDebug)
        {
            Gizmos.DrawSphere(mHitPoint, 0.2f);
            foreach (var r in mRaysDebug)
            {
                Gizmos.DrawRay(r);
            }
        }
    }
}