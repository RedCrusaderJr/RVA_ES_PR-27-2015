using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helpers
{
    public class UpdateHelper
    {
        #region Instance
        private static UpdateHelper _instance;
        private static readonly object syncLock = new object();
        private UpdateHelper()
        {
            PeopleListAdditionHelper = new List<Person>();
            PeopleListRemovalHelper = new List<Person>();
            PeopleListModificationHelper = new List<Person>();

            EventsListAdditionHelper = new List<Event>();
            EventsListRemovalHelper = new List<Event>();
            EventsListModificationHelper = new List<Event>();

            AccountsListAdditionHelper = new List<Account>();
            AccountsListRemovalHelper = new List<Account>();
            AccountsListModificationHelper = new List<Account>();
        }
        public static UpdateHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new UpdateHelper();
                        }
                    }
                }

                return _instance;
            }
        }
        #endregion

        public Int32 PersonCounter { get; set; }
        public Int32 AccountCounter { get; set; }
        public Int32 EventCounter { get; set; }
        public Int32 Limit { get; set; }

        public List<Person> PeopleListAdditionHelper { get; set; }
        public List<Person> PeopleListRemovalHelper { get; set; }
        public List<Person> PeopleListModificationHelper { get; set; }

        public List<Event> EventsListAdditionHelper { get; set; }
        public List<Event> EventsListRemovalHelper { get; set; }
        public List<Event> EventsListModificationHelper { get; set; }

        public List<Account> AccountsListAdditionHelper { get; set; }
        public List<Account> AccountsListRemovalHelper { get; set; }
        public List<Account> AccountsListModificationHelper { get; set; }
    }
}