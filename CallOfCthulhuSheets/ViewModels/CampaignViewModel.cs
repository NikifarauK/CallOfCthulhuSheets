using System.Diagnostics;
using AsyncAwaitBestPractices.MVVM;
using CallOfCthulhuSheets.Models;
using CallOfCthulhuSheets.Services;
using CallOfCthulhuSheets.Views;
using MvvmHelpers;

namespace CallOfCthulhuSheets.ViewModels
{
    [QueryProperty(nameof(CampaignId), nameof(CampaignId))]
    [QueryProperty(nameof(InvestigId), nameof(InvestigId))]
    public class CampaignViewModel : BaseViewModel
    {

        private string campaignId;
        public string CampaignId
        {
            get => campaignId;
            set
            {
                campaignId = value;
                _ = GetCampaign(campaignId);
            }
        }

        private async Task GetCampaign(string value)
        {
            try
            {
                if (string.IsNullOrEmpty(value))
                {
                    return;
                }
                if (value.Equals("new"))
                {
                    Campaign = new Campaign();
                    return;
                }
                Campaign = SqliteRepo.GetItemAsync<Campaign>(value);
                Name = Campaign.Name;
                Description = Campaign.Description;
                _ = Sessions;
                _ = NPCs;
                //Sessions.Clear();
                //Sessions.AddRange(Campaign.Sessions);
                //NPCs.Clear();
                //NPCs.AddRange(Campaign.NPCs);
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
                    name = Campaign?.Name;
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
                    description = Campaign?.Description;
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

        #region Session
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
                SqliteRepo.DeleteItemAsync(session);
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

            if (Campaign.Sessions == null)
            {
                Campaign.Sessions = new List<Session>();
            }
            Campaign.Sessions.Add(sess);
            OnPropertyChanged(nameof(Sessions));
            _ = Sessions;
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
            if (session == null) return;
            SelectedSession = null;
            await Shell.Current.GoToAsync($"{nameof(SessionPage)}?SessionId={session.Id}");
        }
        #endregion

        #region NPCs

        public string investigId;
        public string InvestigId
        {
            get => investigId;
            set
            {
                if (value != null)
                {
                    _ = AddNpc(value);
                }
                investigId = value;
            }
        }

        private async Task AddNpc(string value)
        {
            var npc = SqliteRepo.GetItemAsync<Investigator>(value);
            if (Campaign.NPCs == null)
            {
                Campaign.NPCs = new List<Investigator>();
            }
            Campaign.NPCs.Add(npc);
            OnPropertyChanged(nameof(NPCs));
            _ = NPCs;
        }

        private ObservableRangeCollection<Investigator> npcs;
        public ObservableRangeCollection<Investigator> NPCs
        {
            get
            {
                if (npcs == null)
                {
                    npcs = new ObservableRangeCollection<Investigator>();
                }
                if (Campaign?.NPCs != null)
                {
                    npcs.Clear();
                    npcs.AddRange(Campaign.NPCs);
                }
                return npcs;
            }
        }


        private Investigator selectedNpc;
        public Investigator SelectedNpc { get => selectedNpc; set => SetProperty(ref selectedNpc, value); }

        private AsyncCommand<Investigator> npcSelectedCommand;
        public AsyncCommand<Investigator> NpcSelectedCommand
        {
            get
            {
                if (npcSelectedCommand == null)
                {
                    npcSelectedCommand = new AsyncCommand<Investigator>(NpcSelected);
                }
                return npcSelectedCommand;
            }
        }

        private async Task NpcSelected(Investigator arg)
        {
            selectedNpc = null;
            await Shell.Current.GoToAsync($"{nameof(InvestigatorDetailsPage)}?InvestigatorId={arg.Id}");
        }

        private AsyncCommand addNewNpcCommand;
        public AsyncCommand AddNewNpcCommand
        {
            get
            {
                if (addNewNpcCommand == null)
                {
                    addNewNpcCommand = new AsyncCommand(AddNewNpc);
                }
                return addNewNpcCommand;
            }
        }

        private async Task AddNewNpc()
        {
            await Shell.Current.GoToAsync($"{nameof(NewInvestigatorPage)}?IsPC={false}");
        }

        private AsyncCommand saveCampaignCommand;
        public AsyncCommand SaveCampaignCommand
        {
            get
            {
                if (saveCampaignCommand == null)
                {
                    saveCampaignCommand = new AsyncCommand(SaveCampaign);
                }
                return saveCampaignCommand;
            }
        }
        #endregion

        private async Task SaveCampaign()
        {
            var playerId = Preferences.Get("CurrentPlayerId", "");
            Player current = SqliteRepo.GetItemAsync<Player>(playerId);
            Campaign.Owner = current;
            Campaign.PlayerId = current.Id;
            SqliteRepo.AddItemAsync(Campaign);
            await Shell.Current.DisplayAlert("", "Сохранено", "ok");
        }

    }
}
