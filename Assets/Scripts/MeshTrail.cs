using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshTrail : MonoBehaviour
{
    public bool isTrailActive = false;
    public float activeTime = 2f;

    [Header("Mesh Related")]
    [SerializeField] SkinnedMeshRenderer[] skinnedMeshRenderers;
    [SerializeField] Material[] mats;
    Material[] playerMats;
    bool getMats = true;
    public string shaderVarRef;
    public float meshRefreshRate = 0.1f;
    public float meshDestroyDelay = 3f;
    public float shaderVarRate = 0.1f;
    public float shaderVarRefreshRate = 0.05f;

    private void Update()
    {
        if (isTrailActive)
        {
            if (getMats)
            {
                playerMats = skinnedMeshRenderers[0].materials;
            }
            getMats = false;

            StartCoroutine(ActiveTrail(activeTime));
        }
    }

    IEnumerator ActiveTrail(float timeActive)
    {
        skinnedMeshRenderers[0].materials = mats;

        while (timeActive > 0)
        {
            timeActive -= meshRefreshRate;

            if (skinnedMeshRenderers == null)
            {
                skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
            }

            foreach (SkinnedMeshRenderer skin in skinnedMeshRenderers)
            {
                GameObject gm = new GameObject();
                gm.transform.SetPositionAndRotation(transform.position, transform.rotation);

                MeshRenderer mr = gm.AddComponent<MeshRenderer>();
                MeshFilter mf = gm.AddComponent<MeshFilter>();

                Mesh mesh = new Mesh();
                skin.BakeMesh(mesh);

                mf.mesh = mesh;
                mr.materials = mats;

                foreach (Material _mat in mr.materials)
                {
                    StartCoroutine(AnimateMaterialFloat(_mat, 0, shaderVarRate, shaderVarRefreshRate));
                }

                Destroy(gm, meshDestroyDelay);
            }

            yield return new WaitForSeconds(meshRefreshRate);
        }

        skinnedMeshRenderers[0].materials = playerMats;
        getMats = true;
        isTrailActive = false;
    }

    IEnumerator AnimateMaterialFloat(Material mat, float goal, float rate, float refreshRate)
    {
        float valueToAnimate = mat.GetFloat(shaderVarRef);

        while (valueToAnimate < goal)
        {
            valueToAnimate -= rate;
            mat.SetFloat(shaderVarRef, valueToAnimate);
            yield return new WaitForSeconds(refreshRate);
        }
    }
}
