using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ImageEffectBase : MonoBehaviour
{
	[SerializeField]
	protected Material material;

	// OnRenderImage() is called when the camera has finished rendering.
	protected virtual void OnRenderImage(RenderTexture src, RenderTexture dst)
	{
		Graphics.Blit(src, dst, material);
	}
}
