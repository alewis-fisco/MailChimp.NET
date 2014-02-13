using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MailChimp.Lists
{
    [DataContract]
    public class MemberInfoResult
    {
        /// <summary>
        /// the number of subscribers successfully found on the list
        /// </summary>
        [DataMember(Name = "success_count")]
        public int SuccessCount
        {
            get;
            set;
        }

        /// <summary>
        /// the number of subscribers who were not found on the list
        /// </summary>
        [DataMember(Name = "error_count")]
        public int ErrorCount
        {
            get;
            set;
        }

        /// <summary>
        /// List of members information
        /// </summary>
        [DataMember(Name = "data")]
        public List<MemberInfo> Data
        {
            get;
            set;
        }

        public MemberInfo this[string emailAddress]
        {
            get
            {
                if (string.IsNullOrEmpty(emailAddress))
                    throw new ArgumentNullException("emailAddress");

                if (this.SuccessCount == 0 || this.Data == null)
                    return null;

                return this.Data.Where(mi => mi.Email.Equals(emailAddress)).FirstOrDefault();
            }
        }
    }
}
