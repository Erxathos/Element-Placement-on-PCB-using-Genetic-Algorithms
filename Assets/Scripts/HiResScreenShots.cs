using UnityEngine;

//this class is used to make screenshots
public class HiResScreenShots : MonoBehaviour
{
    public int resWidth = 2550;
    public int resHeight = 3300;
    static Vector3 camera_pos;

    private static bool takeHiResShot = false;

    string ScreenShotName(int width, int height)
    {
        return string.Format("{0}/screen_{1}x{2}_{3}.png",
        Application.dataPath,
        width, height,
        System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
    }

    public static void TakeHiResShot(Vector3 pos)
    {
        takeHiResShot = true;
        camera_pos = pos;
    }

    void LateUpdate()
    {
        takeHiResShot |= Input.GetKeyDown("k");
        if (takeHiResShot)
        {
            //remember the data to return the camera to previous position
            Vector3 previous_pos = transform.localPosition;
            Quaternion previous_rotation = transform.localRotation;

            transform.localPosition = camera_pos; //new position
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

            View.DebugText(string.Format("Assembling draft is saved as {0}", filename));

            takeHiResShot = false;

            //return the camera position
            transform.localPosition = previous_pos;
            transform.localRotation = previous_rotation;
        }
    }
}