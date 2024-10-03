using UnityEngine;

public static class RigibodyExtension
{
    public static void AddForceAtAngle(this Rigidbody2D rigidbody, float force, float angle, ForceMode2D mode)
    {
        Vector2 direction = MathHelper.DegreeToVector2(angle);

        rigidbody.AddForce(force * direction, mode);
    }

    public static void AddForceAtAngle(this Rigidbody2D rigidbody, float force, float angle)
    {
        rigidbody.AddForceAtAngle(force, angle, ForceMode2D.Force);
    }
}
