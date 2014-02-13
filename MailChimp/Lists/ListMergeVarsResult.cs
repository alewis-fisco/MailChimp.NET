using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace MailChimp.Lists
{
    [DataContract]
    public class ListMergeVarsResult
    {
        [DataMember(Name="success_count")]
        public int SuccessCount { get; set; }

        [DataMember(Name = "error_count")]
        public int ErrorCount { get; set; }


        [DataMember(Name = "data")]
        public List<ListMergeVarsData> Data { get; set; }

        [DataContract]
        public class ListMergeVarsData
        {
            [DataMember(Name = "id")]
            public string Id { get; set; }

            [DataMember(Name = "name")]
            public string Name { get; set; }

            [DataMember(Name = "merge_vars")]
            public List<ListMergeVarData> MergeVars { get; set; }

            [DataContract]
            public class ListMergeVarData
            {
                [DataMember(Name="name")]
                public string Name { get; set; }

                [DataMember(Name = "req")]
                public bool Required { get; set; }

                [DataMember(Name = "field_type")]
                public string FieldType { get; set; }

                [DataMember(Name = "public")]
                public bool IsPublic { get; set; }

                [DataMember(Name = "show")]
                public bool Show { get; set; }

                [DataMember(Name = "order")]
                public string Order { get; set; }

                [DataMember(Name = "default")]
                public string Default { get; set; }

                [DataMember(Name = "helptext")]
                public string HelpText { get; set; }

                [DataMember(Name = "size")]
                public string Size { get; set; }

                [DataMember(Name = "tag")]
                public string Tag { get; set; }

                [DataMember(Name = "choices")]
                public string[] Choices { get; set; }

                [DataMember(Name = "id")]
                public int Id { get; set; }

            }

        }
    }
}
