namespace OpenImis.ePayment.Models
{
    public class MaritalStatusVal
    {
        public MaritalStatusVal(MaritalStatus v)
        {
            switch (v)
            {
                case MaritalStatus.Single:
                    Value = "S";
                    break;
                case MaritalStatus.Married:
                    Value = "M";
                    break;
                case MaritalStatus.Devorced:
                    Value = "D";
                    break;
                case MaritalStatus.Widowed:
                    Value = "W";
                    break;
                default:
                    Value = "";
                    break;
            }
        }

        public string Value { get; set; }
    }

    public enum MaritalStatus
    {
        Single,Married,Devorced,Widowed
    }
}