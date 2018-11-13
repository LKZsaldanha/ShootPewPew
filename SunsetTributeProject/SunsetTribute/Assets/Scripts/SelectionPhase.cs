using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectionPhase : MonoBehaviour {

    public void phase1()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void phase2()
    {
        SceneManager.LoadScene("phase2");
    }

    public void phase3()
    {
        SceneManager.LoadScene("phase3");
    }

    public void phase4()
    {
        SceneManager.LoadScene("phase4");
    }

    public void phase5()
    {
        SceneManager.LoadScene("phase5");
    }
}
