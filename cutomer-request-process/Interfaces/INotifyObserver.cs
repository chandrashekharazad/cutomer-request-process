using System;
using System.Collections.Generic;
using System.Text;

namespace cutomer_request_process.Interfaces
{
    interface INotifyObserver
    {
        void Notify(string user_mail);
    }
}
