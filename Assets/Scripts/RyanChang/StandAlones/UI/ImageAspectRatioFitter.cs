using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Aspect ratio fitter for images and raw images.
/// </summary>
/// 
/// <remarks>
/// Authors: Ryan Chang (2024)
/// </remarks>
[ExecuteAlways]
public class ImageAspectRatioFitter : UIBehaviour, ILayoutSelfController,
    ILayoutController
{
    private Image image;

    private RawImage rawImage;

    protected Texture ImageTexture
    {
        get
        {
            if (image)
            {
                return image.mainTexture;
            }
            else if (rawImage)
            {
                return rawImage.texture;
            }
            else
            {
                return null;
            }
        }
    }

    public void SetLayoutHorizontal()
    {
        
    }

    public void SetLayoutVertical()
    {
        UpdateRect();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        InitImages();
    }

    protected override void Start()
    {
        base.Start();
        InitImages();
    }

    private void InitImages()
    {
        image = GetComponent<Image>();
        rawImage = GetComponent<RawImage>();

        UpdateRect();
    }

    private void UpdateRect()
    {
        float w = 0, h = 0;
        RectTransform parent = (RectTransform)transform.parent;
        RectTransform self = (RectTransform)transform;

        if (ImageTexture != null && parent)
        {
            Vector2 textureSize = new(ImageTexture.width, ImageTexture.height);
            float ratio = textureSize.y / textureSize.x;
            Rect bounds = new(0, 0, parent.rect.width, parent.rect.height);

            if (self.eulerAngles.z.RoundToInt() % 180 == 90)
            {
                // Image is rotated.
                bounds.size = new(bounds.height, bounds.width);
            }

            if (ratio < bounds.height / bounds.width)
            {
                // Image height > width.
                w = bounds.width;
                h = w * ratio;
            }
            else
            {
                // Image height <= width.
                h = bounds.height;
                w = h / ratio;
            }
        }
        // self.sizeDelta = new(w, h);
        self.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, w);
        self.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, h);
    }
}