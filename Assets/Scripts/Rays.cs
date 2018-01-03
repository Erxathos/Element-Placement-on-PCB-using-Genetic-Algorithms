using UnityEngine;
using GeneticAlgorithm;
using System.Collections;

public class Rays : MonoBehaviour
{
    GameObject Line_prefab; GameObject Lines;
    Transform Line;
    LineRenderer lineRenderer;
    Ray MyRay;
    RaycastHit hit;
    Transform filter;

    public static Vector3 posB;
    Vector3 x;

    void Start()
    {
        Line_prefab = Resources.Load("Elements/Line") as GameObject;
        posB = Vector3.zero;
    }

    void Update()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, posB, 5 * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            MyRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            x = Input.mousePosition;
            StartCoroutine(MoveCameraMouse(0.1f));
        }
        else if (Input.touchCount == 1)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                MyRay = Camera.main.ScreenPointToRay(Input.touches[0].position);
                StartCoroutine(MoveCameraTouch(0.3f));
            }
        }
    }

    IEnumerator MoveCameraTouch(float t)
    {
        yield return new WaitForSeconds(t);
        if (Input.touches[0].phase != TouchPhase.Moved && Input.touches[0].phase == TouchPhase.Stationary)
            MoveCamera();
    }
    IEnumerator MoveCameraMouse(float t)
    {
        yield return new WaitForSeconds(t);
        if (Mathf.Abs((x - Input.mousePosition).magnitude) < 5)
            MoveCamera();
    }

    void MoveCamera()
    {
        if (Physics.Raycast(MyRay, out hit, 800))
        {
            filter = hit.collider.GetComponent(typeof(Transform)) as Transform;
            if (filter)
            {
                try
                {
                    if (Lines != null)
                        Destroy(Lines.gameObject);
                    Lines = new GameObject { name = "Lines" };
                    int numElem = System.Convert.ToInt32(filter.gameObject.name);
                    for (int j = 0; j < DataStorage.N; j++)
                    {
                        if (DataStorage.C[j, numElem] != 0)
                        {
                            Vector3 a = new Vector3((float)(DataStorage.cm[numElem].x + DataStorage.cm[numElem].Width / 2), 1, (float)(DataStorage.cm[numElem].y + DataStorage.cm[numElem].Height / 2));
                            Vector3 b = new Vector3((float)(DataStorage.cm[j].x + DataStorage.cm[j].Width / 2), 1, (float)(DataStorage.cm[j].y + DataStorage.cm[j].Height / 2));
                            Line = Instantiate(Line_prefab.transform) as Transform;
                            lineRenderer = Line.GetComponent<LineRenderer>();
                            lineRenderer.SetPositions(new Vector3[] { a, b });
                            Line.parent = Lines.transform;
                        }
                    }

                    View.DebugText(DataStorage.cm[numElem].Name + System.Environment.NewLine);
                    View.DebugAppendText("Power of the element is " + DataStorage.cm[numElem].pwDissipation + "W");
                }
                catch (System.Exception) { }
                posB = new Vector3(hit.point.x, hit.point.z);
            }
        }
    }
}