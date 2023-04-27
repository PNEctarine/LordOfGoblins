using UnityEngine;

[ExecuteInEditMode]
public class BlurController : MonoBehaviour
{
    [Header("Blue Settings")]
    [SerializeField] private int _iterations = 3;                   // Blur iterations - larger number means more blur.
    [SerializeField] private float _blurSpread = 0.6f;              // Blur spread for each iteration. Lower values give better looking blur.
    static Material _material = null;
    protected Material Material { get { if (_material == null) { _material = new Material(blurShader) { hideFlags = HideFlags.DontSave }; } return _material; } }

    public Shader blurShader = null;             // The blur iteration shader just takes 4 texture samples and averages them.
                                                 // By applying it repeatedly and spreading out sample locations
                                                 // we get a Gaussian blur approximation.

    // Performs one blur iteration.
    public void FourTapCone(RenderTexture source, RenderTexture dest, int iteration)
    {
        float off = 0.5f + iteration * _blurSpread;
        Graphics.BlitMultiTap(source, dest, Material,
                               new Vector2(-off, -off),
                               new Vector2(-off, off),
                               new Vector2(off, off),
                               new Vector2(off, -off));
    }

    // Downsamples the texture to a quarter resolution.
    private void DownSample4x(RenderTexture source, RenderTexture dest)
    {
        float off = 1.0f;
        Graphics.BlitMultiTap(source, dest, Material,
                               new Vector2(-off, -off),
                               new Vector2(-off, off),
                               new Vector2(off, off),
                               new Vector2(off, -off));
    }

    // Called by the camera to apply the image effect
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        int rtW = source.width / 4;
        int rtH = source.height / 4;
        RenderTexture buffer = RenderTexture.GetTemporary(rtW, rtH, 0);

        // Copy source to the 4x4 smaller texture.
        DownSample4x(source, buffer);

        // Blur the small texture
        for (int i = 0; i < _iterations; i++)
        {
            RenderTexture buffer2 = RenderTexture.GetTemporary(rtW, rtH, 0);
            FourTapCone(buffer, buffer2, i);
            RenderTexture.ReleaseTemporary(buffer);
            buffer = buffer2;
        }
        Graphics.Blit(buffer, destination);
        RenderTexture.ReleaseTemporary(buffer);
    }
}