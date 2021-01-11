using System;
interface IDamageable
{
    void TakeDamage(object sender, EventArgs e);
    void DestroySelf(object sender, EventArgs e);
}