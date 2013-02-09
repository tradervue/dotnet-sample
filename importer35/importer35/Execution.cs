namespace importer
{
    class Execution
    {
        public Execution(string datetime, string symbol, string quantity, string price, string option, string commission, string transfee, string ecnfee)
        {
            this.datetime = datetime;
            this.symbol = symbol;
            this.quantity = quantity;
            this.price = price;
            this.option = option;
            this.commission = commission;
            this.transfee = transfee;
            this.ecnfee = ecnfee;
        }

        public string datetime;
        public string symbol;
        public string quantity;
        public string price;
        public string option;
        public string commission;
        public string transfee;
        public string ecnfee;
    }

}
