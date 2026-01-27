public interface IDamageModifier
{
    // 들어오는 데미지를 수정할 수 있음
    void ModifyDamage(ref int damage);
}