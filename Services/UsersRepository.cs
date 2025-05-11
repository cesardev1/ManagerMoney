namespace ManagerMoney.Services
{
    public class UsersRepository : IUsersRepository
    {
        public int GetUserId()
        {
            return 1; 
        }
    }

    public interface IUsersRepository
    {
        int GetUserId();
    }
}
