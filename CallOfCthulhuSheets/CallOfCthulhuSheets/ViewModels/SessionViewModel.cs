using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;
using CallOfCthulhuSheets.Models;
using CallOfCthulhuSheets.Views;
using System.Threading.Tasks;

namespace CallOfCthulhuSheets.ViewModels
{
    [QueryProperty(nameof(SessionId), nameof(SessionId))]
    public class SessionViewModel : BaseViewModel
    {
        public SessionViewModel()
        {
            Encounters = new ObservableRangeCollection<Encounter>();
            Encounters.AddRange(new List<Encounter>()
            {
                new Encounter(){EncounterType = EncounterTypes.Social, Description = "Знакомство"},
                new Encounter(){EncounterType = EncounterTypes.Figth, Description = "Встреча с культистами"},
                new Encounter(){EncounterType = EncounterTypes.Chase, Description = "Побег"}
            });
        }

        public string SessionId { get; set; }

        public ObservableRangeCollection<Encounter> Encounters { get; }

        private AsyncCommand goToBattlePage;

        public ICommand GoToBattlePage
        {
            get
            {
                if (goToBattlePage == null)
                {
                    goToBattlePage = new AsyncCommand(PerformGoToBattlePage);
                }

                return goToBattlePage;
            }
        }

        private async Task PerformGoToBattlePage()
        {
            await Shell.Current.GoToAsync($"{nameof(BattlePage)}");
        }

    }
}
