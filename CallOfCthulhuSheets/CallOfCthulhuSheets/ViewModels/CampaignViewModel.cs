using CallOfCthulhuSheets.Models;
using CallOfCthulhuSheets.Services;
using CallOfCthulhuSheets.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace CallOfCthulhuSheets.ViewModels
{
    [QueryProperty(nameof(CampaignId), nameof(CampaignId))]
    public class CampaignViewModel : BaseViewModel
    {

        private string campaignId;
        public string CampaignId
        {
            get => campaignId;
            set
            {
                _ = GetCampaign(value);
                campaignId = value;
            }
        }

        private async Task GetCampaign(string value)
        {
            try
            {
                if (string.IsNullOrEmpty(CampaignId))
                {
                    Campaign = new Campaign();
                    return;
                }
                Campaign = await SqliteRepo.GetItemAsync<Campaign>(value);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw e;
            }
        }

        private Campaign campaign;
        public Campaign Campaign
        {
            get => campaign;
            set => SetProperty(ref campaign, value);
        }


        private string name;
        public string Name
        {
            get
            {
                if (name == null)
                {
                    name = Campaign.Name;
                }
                return name;
            }
            set
            {
                if (SetProperty(ref name, value))
                {
                    Campaign.Name = name;
                }
            }
        }


        private string description;
        public string Description
        {
            get
            {
                if (description == null)
                {
                    description = Campaign.Description;
                }
                return description;
            }
            set
            {
                if (SetProperty(ref description, value))
                {
                    Campaign.Description = description;
                }
            }
        }

        private ObservableRangeCollection<Session> sessions;
        public ObservableRangeCollection<Session> Sessions
        {
            get
            {
                if (sessions == null)
                {
                    sessions = new ObservableRangeCollection<Session>();
                }
                if (Campaign?.Sessions != null)
                {
                    sessions.Clear();
                    sessions.AddRange(Campaign.Sessions);
                }
                return sessions;
            }
        }

        private Session selectedSession;
        public Session SelectedSession { get => selectedSession; set => SetProperty(ref selectedSession, value); }

        private AsyncCommand<Session> deleteCommand;
        public AsyncCommand<Session> DeleteCommand
        {
            get
            {
                if (deleteCommand == null)
                {
                    deleteCommand = new AsyncCommand<Session>(DeleteSession);
                }
                return deleteCommand;
            }
        }

        private async Task DeleteSession(Session session)
        {
            try
            {
                await SqliteRepo.DeleteItemAsync(session);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw e;
            }
        }

        private AsyncCommand addNewSession;
        public AsyncCommand AddNewSession
        {
            get
            {
                if (addNewSession == null)
                {
                    addNewSession = new AsyncCommand(PerformAddNewSession);
                }

                return addNewSession;
            }
        }

        private async Task PerformAddNewSession()
        {
            var sess = new Session() { Number = Sessions.Count + 1 };
            var res = await Shell.Current.DisplayPromptAsync("", "Введите название сессии", "ok", "cancel");
            if (res == null)
                return;
            sess.Name = res;
            res = await Shell.Current.DisplayPromptAsync("", "Введите описание", "ok", "cancel");
            if (res == null)
                return;
            sess.Descrption = res;

            if(Campaign.Sessions == null)
            {
                Campaign.Sessions = new List<Session>();
            }
            Campaign.Sessions.Add(sess);
            //Sessions.Add(sess);
            OnPropertyChanged(nameof(Sessions));
        }

        private AsyncCommand<Session> sessionSelectedCommand;

        public AsyncCommand<Session> SessionSelectedCommand
        {
            get
            {
                if (sessionSelectedCommand == null)
                {
                    sessionSelectedCommand = new AsyncCommand<Session>(SessionSelected);
                }

                return sessionSelectedCommand;
            }
        }

        private async Task SessionSelected(Session session)
        {
            SelectedSession = null;
            await Shell.Current.GoToAsync($"{nameof(SessionPage)}");
        }
    }
}
