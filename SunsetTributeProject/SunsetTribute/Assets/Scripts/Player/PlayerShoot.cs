using UnityEngine;

public class PlayerShoot : MonoBehaviour {

    private PlayerAim playerAim;

    private string fire1InputName = "Fire1P";

    [SerializeField] private GameObject bulletPrefab;

    private GameObject bulletSpawnPoint;

    [SerializeField] private GameObject bulletSpawnPointsUp;
    [SerializeField] private GameObject bulletSpawnPointsDiagUp;
    [SerializeField] private GameObject bulletSpawnPointsFront;
    [SerializeField] private GameObject bulletSpawnPointsDiagDown;
    [SerializeField] private GameObject bulletSpawnPointsDown;


    // Use this for initialization
    void Start () {
        playerAim = GetComponent<PlayerAim>();

        float convertedID = playerAim.playerID + 1;
        fire1InputName = fire1InputName + convertedID.ToString();
    }
	
	// Update is called once per frame
	void Update () {
        if (!GetComponent<CharacterMovement>().dashLock)
        {
            if (Input.GetButtonDown(fire1InputName))
            {
                GetSpawnPoint();
                Instantiate(bulletPrefab, bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.rotation);
            }
        }
	}

    private void GetSpawnPoint()
    {

        switch (playerAim.aimAngle)
        {
            case 0:
                bulletSpawnPoint = bulletSpawnPointsUp;
                break;
            case 45:
                bulletSpawnPoint = bulletSpawnPointsDiagUp;
                break;
            case 90:
                bulletSpawnPoint = bulletSpawnPointsFront;
                break;
            case 135:
                bulletSpawnPoint = bulletSpawnPointsDiagDown;
                break;
            case 180:
                bulletSpawnPoint = bulletSpawnPointsDown;
                break;
            case 225:
                bulletSpawnPoint = bulletSpawnPointsDiagDown;
                break;
            case 270:
                bulletSpawnPoint = bulletSpawnPointsFront;
                break;
            case 315:
                bulletSpawnPoint = bulletSpawnPointsDiagUp;
                break;

        }
    }
}
