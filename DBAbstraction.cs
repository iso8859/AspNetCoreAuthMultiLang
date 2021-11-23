namespace AspNetCoreAuthMultiLang
{
    public abstract class DBAbstraction
    {
        public abstract bool IsUserPasswordOk(string login, string password);
        public abstract long GetUserId(string login);
    }

    public class DBJson : DBAbstraction
    {
        public override bool IsUserPasswordOk(string login, string password)
        {
            if (login == "admin" && password == "admin")
                return true;
            else
                return false;
        }

        public override long GetUserId(string login)
        {
            return 1;
        }
    }
}
