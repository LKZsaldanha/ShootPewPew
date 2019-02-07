using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraFollow : MonoBehaviour {

	public Transform target;
    public Transform target2;
    public bool showFocusArea = false;
	public Vector2 focusAreaSize;
	public Vector2 offset;
	public float zDistance = -20;
    [SerializeField] GameObject gameSystem;

	float lerpTime = 1f;
    float currentLerpTime;
 
    float moveDistance = 10f;
 
    //private Vector3 startPos;
    public Transform endPosCamera;
	public Transform positionCameraFake; //fiz pra camera pegar a ultima posição original da camera.
	private Vector3 firstPosCamera;
	public bool cameraFocusPlayer;
	public bool reached;
	private float perc = 0f;
	public float slowSpeedTransition = 2f;

	public bool firstTimeFocus;

	public float powerShake = 0.7f;
	//public float duration = 1.0f;
	//public Transform camera;
	//public float slowDownAmount = 1.0f;
	public bool shouldShake;

	//private Vector3 startPosition;
	
 

	private FocusArea focusArea;


	private void Start()
	{
        gameSystem = GameObject.Find("GameSystem");
		focusArea = new FocusArea(target.GetComponent<Collider2D>().bounds, focusAreaSize);

		
        //endPosCamera = transform.position + transform.up * moveDistance;
	}


	void LateUpdate()
	{
        if(gameSystem.GetComponent<GameSystem>().nPlayerVivos.Count>0)
        {
            if (target == null)
            {
                if (target2 != null)
                {
                    target = target2;
                    target2 = null;
                }
            }
            if (target2 == null)
            {
                target2 = target;
            }
			
			if(cameraFocusPlayer){
				focusArea.Update(target.GetComponent<Collider2D>().bounds, target2.GetComponent<Collider2D>().bounds);
				Vector2 focusPosition = focusArea.center + offset;
				transform.position = (Vector3)focusPosition + Vector3.forward * zDistance;
				firstPosCamera = positionCameraFake.position;

			}else{//leva a camera até o estado de ficar parada e tremendo na parte dos bois
				
				if(firstTimeFocus){//vai até a camera parada
					if(perc < 1){
						perc += Time.deltaTime / slowSpeedTransition; //numero de 0 a 1
					}else{
						shouldShake = true;
					}
					//lerp!
					transform.position = Vector3.Lerp(transform.position, endPosCamera.position, perc);
				}else{//volta pra posição original
					if(perc < 1){
						perc += Time.deltaTime / slowSpeedTransition*2; //numero de 0 a 1
					}else{
						cameraFocusPlayer = true; //volta a seguir os players
					}
					
					transform.position = Vector3.Lerp(transform.position, firstPosCamera, perc);
				}
				

				if(shouldShake)
				{
					firstPosCamera = positionCameraFake.position;
					transform.localPosition = transform.position + Random.insideUnitSphere * powerShake;
				}
			}

			
        }
        else
        {
          //  SceneManager.LoadScene("SplashScreen");
        }


	}
	

	public void Perc(){//chamado pelos bois quando chegam no fim
		firstTimeFocus = false;
		shouldShake = false;
		perc = 0;
	}

	void OnDrawGizmos()
	{
		if(showFocusArea){
			Gizmos.color = new Color (0,1,0, 0.5f);
			Gizmos.DrawCube(focusArea.center,focusAreaSize);
		}
	}

	struct FocusArea
	{
		public Vector2 center;
		public Vector2 velocity;

		float left, right;
        float top, bottom;

        public FocusArea (Bounds targetBounds, Vector2 size) {
			left = targetBounds.center.x - size.x/2;
			right = targetBounds.center.x + size.x/2;
			bottom = targetBounds.min.y - size.y/2;
			top = targetBounds.min.y + size.y/2;

            velocity = Vector2.zero;
			center = new Vector2((left + right) / 2,(bottom + top) / 2);
		}

		public void Update (Bounds targetBounds, Bounds targetBounds2)
        {
			float shiftX = 0;
			if(targetBounds.min.x < left)
			{
                //desloca a camera para a esquerda
				//shiftX = targetBounds.min.x - left;
			}
			else if (targetBounds.max.x > right)
			{
                //desloca a camera para a direita
                shiftX = targetBounds.max.x - right;
			}else if(targetBounds2.min.x < left)

            {
                //desloca a camera para a esquerda
                //shiftX = targetBounds.min.x - left;
            }
			else if (targetBounds2.max.x > right)
            {
                //desloca a camera para a direita
                shiftX = targetBounds2.max.x - right;
            }
            left += shiftX;
			right += shiftX;

			float shiftY = 0;

            float averageMinY = (targetBounds.min.y + targetBounds2.min.y) / 2;
            float averageMaxY = (targetBounds.max.y + targetBounds2.max.y) / 2;

            if (averageMinY < bottom)
			{
                //desloca a camera para baixo
                shiftY = averageMinY - bottom;
			}
			else if (averageMaxY > top)
			{
                //desloca a camera para cima
                shiftY = averageMaxY - top;
			}
            top += shiftY;
			bottom += shiftY;

			center = new Vector2((left + right) / 2,(bottom + top) / 2);
			velocity = new Vector2 (shiftX,shiftY);
		}
	}
}
