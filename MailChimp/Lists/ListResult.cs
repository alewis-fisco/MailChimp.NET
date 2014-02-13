using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MailChimp.Lists
{
    /// <summary>
    /// result of the operation including valid data and any errors
    /// </summary>
    [DataContract]
    public class ListResult
    {
        /// <summary>
        /// the total number of lists which matched the provided filters
        /// </summary>
        [DataMember(Name = "total")]
        public int Total
        {
            get;
            set;
        }

        /// <summary>
        /// lists which matched the provided filters
        /// </summary>
        [DataMember(Name = "data")]
        public List<ListInfo> Data
        {
            get;
            set;
        }

        public ListInfo this[string listName]
        {
            get
            {
                if (string.IsNullOrEmpty(listName))
                    throw new ArgumentNullException("listName");

                if (Total == 0 || Data == null)
                    return null;

                return Data.Where(lg => lg.Name.Equals(listName)).FirstOrDefault();
            }
        }
    }
}
