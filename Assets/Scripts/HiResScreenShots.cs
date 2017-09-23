using UnityEngine;
using System.Collections;

public class HiResScreenShots : MonoBehaviour
{
    public int resWidth = 2550;
    public int resHeight = 3300;

    private static bool takeHiResShot = false;

    string ScreenShotName(int width, int height)
    {
        return string.Format("{0}/screen_{1}x{2}_{3}.png",
        Application.dataPath,
        width, height,
        System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
    }

    public static void TakeHiResShot()
    {
        takeHiResShot = true;
    }

    void LateUpdate()
    {
        takeHiResShot |= Input.GetKeyDown("k");
        if (takeHiResShot)
        {
            Vector3 t = transform.localPosition;
            Quaternion t1 = transform.localRotation;
            transform.localPosition = new Vector3(diplom.PP.Width / 2, diplom.PP.Height / 2);
            transform.localRotation = new Quaternion(0, 0, 0, 0);

            RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
            Camera.main.targetTexture = rt;
            Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
            Camera.main.Render();
            RenderTexture.active = rt;
            screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
            Camera.main.targetTexture = null;
            RenderTexture.active = null; // JC: added to avoid errors
            Destroy(rt);
            byte[] bytes = screenShot.EncodeToPNG();
            string filename = ScreenShotName(resWidth, resHeight);
            System.IO.File.WriteAllBytes(filename, bytes);
            View.DebugText(string.Format("Сборочный чертеж сохранен: {0}", filename));
            takeHiResShot = false;

            transform.localPosition = t;
            transform.localRotation = t1;
        }
    }
}