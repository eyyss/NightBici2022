using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NpcFootsteps : MonoBehaviour
{
    public Transform groundCheck;
    public LayerMask groundMask;
    public List<FootstepData> footstepDatas;
    [System.Serializable]
    public class FootstepData
    {
        public List<string> materialNames;
        public AudioData audioData;
    }

    public void PlayFootstepSounds()
    {
        if (!Application.isPlaying) return;
        if (Physics.Raycast(groundCheck.position, Vector3.down, out RaycastHit hit, 0.5f, groundMask))
        {
            if (hit.collider != null && hit.collider.TryGetComponent(out Renderer renderer))
            {
                Material mat = renderer.sharedMaterial;
                FootstepData foostepData = null;
                foostepData = footstepDatas.FirstOrDefault(
                    item => item.materialNames.Any(matName => mat.name.Contains(matName))
                );

                if (foostepData != null)
                {
                    foostepData.audioData.Play3D(this, transform.position);
                }
            }
        }
    }
}
