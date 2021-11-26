namespace cutomer_request_process.Interfaces
{
    public interface INotifyObserver
    {
        void Notify(string user_mail, string account_number, int balance, string _case);
    }
}
