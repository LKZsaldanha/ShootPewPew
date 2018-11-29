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


	private FocusArea focusArea;


	private void Start()
	{
        gameSystem = GameObject.Find("GameSystem");
		focusArea = new FocusArea(target.GetComponent<Collider2D>().bounds, focusAreaSize);
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
            focusArea.Update(target.GetComponent<Collider2D>().bounds, target2.GetComponent<Collider2D>().bounds);
            Vector2 focusPosition = focusArea.center + offset;

            transform.position = (Vector3)focusPosition + Vector3.forward * zDistance;
        }
        else
        {
          //  SceneManager.LoadScene("SplashScreen");
        }


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
