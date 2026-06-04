namespace StreetCleaning.Models
{
    public class Pager
    {
        private int _selectedAmount;
        private int _skipAmount;
        public int SelectedAmount
        {
            get => _selectedAmount;
            set
            {
                if (value >= 0)
                {
                    _selectedAmount = value;
                }
            }
        }
        public int SkipAmount
        {
            get => _skipAmount;
            set
            {
                if (value >= 0)
                {
                    _skipAmount = value;
                }
            }
        }
        public string NoMoreResults { get; set; }

        public Pager()
        {
            SelectedAmount = 25;
            SkipAmount = 0;
            NoMoreResults = "false";
        }
    }
}
