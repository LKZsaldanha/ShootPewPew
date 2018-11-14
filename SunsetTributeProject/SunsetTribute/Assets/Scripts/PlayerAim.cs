using UnityEngine;

public class PlayerAim : MonoBehaviour {

    [SerializeField] private string horizontalAxisInput = "Horizontal";
    [SerializeField] private string verticalAxisInput = "Vertical";

    [SerializeField] private float inputDeadZoneValue;


    //direções da mira
    private bool leftAim, rightAim, upAim, downAim;
    //ultima direção horizontal
    private bool lastSideWasRight = true;

    //angulo para onde está a mira
    public int aimAngle = 0;

    
    private void Start () {
        //reseta todos os inputs
        leftAim = false;
        rightAim = false;
        upAim = false;
        downAim = false;
    }
	
	
	private void Update () {

        //Compara inputs para setar booleans
        if (Input.GetAxis(horizontalAxisInput) > inputDeadZoneValue)
        {
            leftAim = false;
            rightAim = true;
            lastSideWasRight = true;
        }
        else if (Input.GetAxis(horizontalAxisInput) < -inputDeadZoneValue)
        {
            leftAim = true;
            rightAim = false;
            lastSideWasRight = false;
        }
        else
        {
            leftAim = false;
            rightAim = false;
        }


        if (Input.GetAxis(verticalAxisInput) > inputDeadZoneValue)
        {
            upAim = true;
            downAim = false;
        }
        else if (Input.GetAxis(verticalAxisInput) < -inputDeadZoneValue)
        {
            upAim = false;
            downAim = true;
        }
        else
        {
            upAim = false;
            downAim = false;
        }

        //Reseta de volta para frente se nenhum input está sendo segurado
        if(!leftAim && !rightAim && !upAim && !downAim)
        {
            if (lastSideWasRight)
            {
                rightAim = true;
            }
            else
            {
                leftAim = true;
            }
        }

        SetAimStatus();

    }

    //converte a informação das booleans em um valor de angulo em sentido horario
    private void SetAimStatus()
    {
        if (rightAim)
        {
            //diagonal para frente e cima
            if (upAim)
            {
                aimAngle = 45;
                return;
            }
            //diagonal para frente e baixo
            if (downAim)
            {
                aimAngle = 135;
                return;
            }
            //frente
            aimAngle = 90;
            return;
        }

        if (leftAim)
        {
            if (upAim)
            {
                //diagonal para trás e cima
                aimAngle = 315;
                return;
            }

            if (downAim)
            {
                //diagonal para trás e baixo
                aimAngle = 225;
                return;
            }
            //trás
            aimAngle = 270;
            return;
        }

        if (upAim)
        {
            //cima
            aimAngle = 0;
            return;
        }

        if (downAim)
        {
            //baixo
            aimAngle = 180;
            return;
        }
    }
}
