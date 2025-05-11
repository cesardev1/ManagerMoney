namespace ManagerMoney.Services
{
    public class UserServices : IUserServices
    {
        public int GetUserId()
        {
            return 1;
        }
    }

    public interface IUserServices
    {
        int GetUserId();
    }
}
