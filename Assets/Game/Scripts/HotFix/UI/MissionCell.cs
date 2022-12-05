using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MissionCell : MonoBehaviour 
{
    public Image ImgBackground;
    public Text text;
    // public LayoutElement element;
    // public static float[] randomWidths = new float[3] { 100, 150, 50 };
    void ScrollCellIndex(int idx)
    {
        string name = "Cell " + idx.ToString();
        string imgName = "bgmission" + (idx+1).ToString();
        if (text != null)
        {
            text.text = name;
        }
        // element.preferredWidth = randomWidths[Mathf.Abs(idx) % 3];
        gameObject.name = name;

        //ImgBackground
        UIUtils.SetSprite(ImgBackground, imgName);
    }
}
