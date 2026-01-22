namespace Choi
{
    public interface IBuffReceiver
    {
        void ApplyBuff(BuffDataSO data, int stack);
        void RemoveBuff(BuffDataSO data);
    }
}
