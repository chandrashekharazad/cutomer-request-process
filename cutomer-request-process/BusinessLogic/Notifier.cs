using cutomer_request_process.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

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
        public void ExecuteNotifier()
        {
            foreach (INotifyObserver O in ALNotify)
            {
                //Call all notification System  
                O.Notify();
            }
        }
    }
}
