using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CallOfCthulhuSheets.ViewModels
{
    [QueryProperty(nameof(SessionId), nameof(SessionId))]
    public class SessionViewModel : BaseViewModel
    {
        public string SessionId { get; set; }
    }
}
