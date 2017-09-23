using diplom;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class View : MonoBehaviour
{

    public Transform Plate_prefab, Rotator;

    [SerializeField]
    InputField PCross, PMut, numPerson;

    public Dropdown project_num, plate_num;

    public Slider criterion;

    static Text DebugTxt;

    [SerializeField]
    GameObject SettingsMenu, Footer, ProjectMenu;

    [SerializeField]
    Button SaveButton, TButton;
    bool T = false;

    void Start()
    {
        DataStorage.CurrentProject = 1;
        DataStorage.CurrentPlate = 1;
        SettingsMenu.SetActive(false);
        Footer.SetActive(false);
        SaveButton.interactable = false;
        TButton.interactable = false;
        DebugTxt = GameObject.Find("Log_label").GetComponent("Text") as Text;
        Messenger.AddListener(GameEvent.GUI_UPDATED, GUI_UPDATED);

        project_num.ClearOptions();
        plate_num.ClearOptions();

        SQLite.LoadProjectNames(); SQLite.LoadPlates();

        foreach(string project in DataStorage.ProjectNames)
            project_num.options.Add(new Dropdown.OptionData() { text = (string)project });
        if (project_num.options.Count > 0)
        {
            project_num.value = -1;
            project_num.value = 0;
        }
    }

    void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.GUI_UPDATED, GUI_UPDATED);
    }

    static public void DebugText(string s)
    {
        if (DebugTxt != null)
            DebugTxt.text = s;
    }

    static public void DebugAppendText(string s)
    {
        if (DebugTxt != null)
            DebugTxt.text += s;
    }

    private void GUI_UPDATED()
    {
        try
        {
            Messenger.Broadcast(GameEvent.DESTROY);
        }
        catch
        { }

        Transform Plate = Instantiate(Plate_prefab) as Transform;
        Plate.localScale = new Vector3(PP.Width, PP.Height, 1);
        Plate.parent = Rotator; //перевод платы в 2D - плоскость
        Plate.localEulerAngles = Vector3.zero;

        Transform Elem, pwDissipation;
        List<GameObject> lst = new List<GameObject>();
        foreach (string caseName in DataStorage.caseNames)
            lst.Add(Resources.Load("Elements/" + caseName) as GameObject);

        for (int i = 0; i < DataStorage.N; i++)
        {
            Elem = Instantiate(lst[DataStorage.caseNames.IndexOf(DataStorage.km[i].CaseName)].transform) as Transform;

            Elem.parent = Rotator;
            Elem.localPosition = new Vector3(DataStorage.km[i].x + DataStorage.km[i].Width / 2, DataStorage.km[i].y + DataStorage.km[i].Height / 2, 0); // make it at the exact position of the spawner

            Elem.localEulerAngles = Vector3.zero;
            if (DataStorage.km[i].isVertical == true)
            {
                Elem.localEulerAngles = new Vector3(Elem.rotation.x, Elem.rotation.y, Elem.rotation.z + 90);
            }

            Elem.gameObject.name = i.ToString();
            Elem.Find("Label").GetComponent<TextMesh>().text = DataStorage.km[i].Name;

            pwDissipation = Elem.Find("Sphere");
            if (pwDissipation != null)
            {
                float t = (DataStorage.km[i].pwDissipation * 3);
                pwDissipation.localScale = new Vector3(t, t, t);
            }
            pwDissipation = null;
            T = false;
        }
    }

    public void Assembling_Draft()
    {
        try
        {
            Messenger.Broadcast(GameEvent.DESTROY);
        }
        catch
        { }
        Transform Elem;
        GameObject elem_prefab = Resources.Load("Elements/DraftElement") as GameObject;
        Transform Plate = Instantiate((Resources.Load("Elements/DraftPlate") as GameObject).transform,
            new Vector3(PP.Width / 2, -0.5f, PP.Height / 2), Quaternion.identity) as Transform;
        Plate.localScale = new Vector3(PP.Width, PP.Height, 1);
        Plate.parent = Rotator;
        Plate.localEulerAngles = Vector3.zero;

        for (int i = 0; i < DataStorage.N; i++)
        {
            CircuitElement t = DataStorage.km[i];
            Elem = Instantiate(elem_prefab.transform) as Transform;

            Elem.Find("Scale").transform.localScale = new Vector3(t.Width - 3, t.Height - 3, 1);

            Elem.parent = Rotator;

            Elem.localPosition = new Vector3(t.x + t.Width / 2, t.y + t.Height / 2, 0);

            Elem.localEulerAngles = Vector3.zero;

            Elem.gameObject.name = i.ToString();
            Elem.Find("Label").GetComponent<TextMesh>().text = DataStorage.km[i].Name;
        }
        HiResScreenShots.TakeHiResShot();
        SaveButton.interactable = false;
    }

    public Transform elemT;
    public void viewTM()
    {
        if (!T)
        {
            try
            {
                Messenger.Broadcast(GameEvent.DESTROY);
            }
            catch
            { }
            Transform Plate = Instantiate(Plate_prefab) as Transform;
            Plate.parent = Rotator; //перевод платы в 2D - плоскость
            Plate.localEulerAngles = Vector3.zero;
            Plate.localScale = new Vector3(PP.Width, PP.Height, 1);
            Transform Elem;
            for (int i = 0; i < DataStorage.N; i++)
            {
                if (DataStorage.km[i].pwDissipation > 1)
                {
                    Elem = Instantiate(elemT) as Transform;
                    Elem.parent = Rotator;
                    Elem.localPosition = new Vector3(DataStorage.km[i].x + DataStorage.km[i].Width / 2, DataStorage.km[i].y + DataStorage.km[i].Height / 2, -1); // make it at the exact position of the spawner
                    Elem.localEulerAngles = Vector3.zero;
                    float t = (DataStorage.km[i].pwDissipation * 6);
                    Elem.localScale = new Vector3(t, t, t);
                    Elem.gameObject.name = i.ToString();
                }
            }
            T = true;
        DebugText("Отображение тепловыделения элементов");
        }
        else
        {
            GUI_UPDATED();
        DebugText("Отображение элементов");
        }
    }

    public void projectChanged()
    {
        DataStorage.CurrentProject = project_num.value + 1;
        DataStorage.CurrentPlate = 1;

        plate_num.ClearOptions();

        foreach (int plates in DataStorage.PlateNumbers[DataStorage.CurrentProject - 1])
            plate_num.options.Add(new Dropdown.OptionData() { text = plates.ToString() }); 

        if (plate_num.options.Count > 0)
        {
            plate_num.value = -1;
            plate_num.value = 0;
        }
    }

    public void plateChanged()
    {
        DataStorage.CurrentPlate = plate_num.value + 1;
    }

    public void criterionChanged()
    {
        DataStorage.p2 = DataStorage.p2_start * criterion.value;
        DataStorage.p1 = (1 - criterion.value) * DataStorage.p1_start;
        DebugText("Критерий 1: " + (int)(criterion.value * 100) + "%" + System.Environment.NewLine);
        DebugAppendText("Критерий 2: " + (int)((1 - criterion.value) * 100) + "%" + System.Environment.NewLine);
    }

    public void numPersonChanged()
    {
        if (int.Parse(numPerson.text) < 2)
            numPerson.text = 2 + "";
        DataStorage.NPer = int.Parse(numPerson.text);
    }

    public void PCrossChanged()
    {
        double t = double.Parse(PCross.text);

        if (t < 0)
            PCross.text = 0 + "";
        else if (t > 1)
            PCross.text = 1 + "";

        DataStorage.PCross = double.Parse(PCross.text);
    }

    public void PMutChanged()
    {
        double t = double.Parse(PMut.text);

        if (t < 0)
            PMut.text = 0 + "";
        else if (t > 1)
            PMut.text = 1 + "";

        DataStorage.PMut = int.Parse(PMut.text);
    }

    public void showHideSettingsMenu()
    {
        SettingsMenu.SetActive(!SettingsMenu.activeSelf);
    }

    public void showHideMenu()
    {
        ProjectMenu.SetActive(!ProjectMenu.activeSelf);
        Footer.SetActive(!Footer.activeSelf);
    }

    public void PerformMainCoroutine()
    {
        DebugText("Подождите, выполняется размещение...");
        StartCoroutine(this.gameObject.GetComponent<Main>().main());
        SaveButton.interactable = true;
        TButton.interactable = true;
    }
}