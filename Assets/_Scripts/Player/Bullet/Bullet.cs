using UnityEngine;

public class Bullet : Unit
{
    private float _speed = 10f;


    #region UNITY FUNCTIONS

    // Update is called once per frame
    private void Update()
    {
        Move();

        // Return to the pool if the bullet leaves the screen
        if (OutsideCamera())
        {
            Disable();
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.TryGetComponent(out CircusBall ball))
        {
            ball.TakeHit();

            // Increase score
            GameManager.Instance.Data.Score(3);

            // Return to pool
            Disable();

            /// Pull of the pool FX bullet impact
            UnitManager.Instance.PullBulletImpact(transform.position, Quaternion.identity);

            // Audio play
            AudioSystem.Instance.Play("Bullet Hit");
            AudioSystem.Instance.Play("Bulet Pop Hit");
        }
    }

    #endregion

    #region MOVE FUNCTIONS

    private void Move()
    {
        transform.position += (Vector3.up * _speed * Time.deltaTime);
    }

    public void SetSpeed(float speed)
    {
        _speed = speed;
    }

    private bool OutsideCamera()
    {
        Camera camera = Camera.main;
        float height = 2f * camera.orthographicSize;
        float width = height * camera.aspect;

        return (transform.position.y > camera.transform.position.y + (height / 2f) + 1f);
    }

    #endregion
}
