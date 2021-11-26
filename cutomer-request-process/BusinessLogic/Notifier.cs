using cutomer_request_process.Interfaces;
using System.Collections;

namespace cutomer_request_process.BusinessLogic
{
    class Notifier
    {
        public ArrayList ALNotify = new ArrayList();
        /// <summary>  
        /// Add object of notification System  
        /// </summary>  
        /// <param name="obj">Object is notification class</param>  
        public void AddService(INotifyObserver obj)
        {
            ALNotify.Add(obj);
        }
        /// <summary>  
        /// Remove object of notification System  
        /// </summary>  
        /// <param name="obj">Object of notification Calss</param>  
        public void RemoveService(INotifyObserver obj)
        {
            ALNotify.Remove(obj);
        }
        public void ExecuteNotifier(string user_mail, string account_number, int balance, string _case)
        {
            foreach (INotifyObserver O in ALNotify)
            {
                //Call  notification System  
                O.Notify(user_mail, account_number, balance, _case);
            }
        }
    }
}
