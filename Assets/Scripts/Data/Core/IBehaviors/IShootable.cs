interface IShootable
{
    void Shoot(UnityEngine.GameObject cannon, UnityEngine.Quaternion pointingAngle, float shootingSpeed, float bulletSize, float bulletSpeed, Constants.BulletType bulletType);
}