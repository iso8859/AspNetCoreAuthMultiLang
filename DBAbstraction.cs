namespace AspNetCoreAuthMultiLang
{
    public interface IDBAbstraction
    {
        public bool IsUserPasswordOk(string login, string password);
        public long GetUserId(string login);
    }

    public class DBMemory : IDBAbstraction
    {
        public bool IsUserPasswordOk(string login, string password)
        {
            if (login == "admin" && password == "admin")
                return true;
            else
                return false;
        }

        public long GetUserId(string login)
        {
            return 1;
        }
    }
}
