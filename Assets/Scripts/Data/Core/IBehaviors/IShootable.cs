interface IShootable
{
    void Shoot(UnityEngine.GameObject cannon, UnityEngine.Quaternion pointingAngle, float shootingSpeed, Constants.BulletType bulletType);
}