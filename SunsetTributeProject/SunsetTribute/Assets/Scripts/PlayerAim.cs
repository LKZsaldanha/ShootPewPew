using UnityEngine;

public class PlayerAim : MonoBehaviour {


    //id do player (p1 = 0, p2 = 1, p3 = 2, p4 = 3)
    public int playerID = 0;

    private string horizontalAxisName = "HorizontalP";
    private string verticalAxisName = "VerticalP";
    private string fire1InputName = "Fire1P";

    [SerializeField] private float inputDeadZoneValue;

    private Animator myAnimator;


    //direções da mira
    private bool leftAim, rightAim, upAim, downAim;
    //ultima direção horizontal
    private bool lastSideWasRight = true;

    //angulo para onde está a mira
    public int aimAngle = 0;
    private int lastAimAngle = 0;

    
    private void Start () {
        //reseta todos os inputs
        leftAim = false;
        rightAim = false;
        upAim = false;
        downAim = false;


        myAnimator = GetComponentInChildren<Animator>();
        SetInputAxis();
    }

    private void SetInputAxis()
    {
        float convertedID = playerID + 1;
        horizontalAxisName = horizontalAxisName + convertedID.ToString();
        verticalAxisName = verticalAxisName + convertedID.ToString();
        fire1InputName = fire1InputName + convertedID.ToString();
    }


        private void Update () {

        //Compara inputs para setar booleans
        if (Input.GetAxis(horizontalAxisName) > inputDeadZoneValue)
        {
            leftAim = false;
            rightAim = true;
            lastSideWasRight = true;
        }
        else if (Input.GetAxis(horizontalAxisName) < -inputDeadZoneValue)
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


        if (Input.GetAxis(verticalAxisName) > inputDeadZoneValue)
        {
            upAim = true;
            downAim = false;
        }
        else if (Input.GetAxis(verticalAxisName) < -inputDeadZoneValue)
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

        if (!GetComponent<CharacterMovement>().dashLock)
        {
            SetAimStatus();

            Animate();

            Shoot();
        }

    }

    private void Animate()
    {
        myAnimator.SetInteger("AimAngle", aimAngle);
        if(aimAngle != lastAimAngle)
        {
            myAnimator.SetTrigger("UpdateAim");
            lastAimAngle = aimAngle;
        }

    }

    private void Shoot()
    {
        if (Input.GetButtonDown(fire1InputName))
        {
            myAnimator.SetTrigger("Shoot");

            myAnimator.SetTrigger("UpdateAim");
        }
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
