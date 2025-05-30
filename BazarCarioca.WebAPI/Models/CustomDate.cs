namespace BazarCarioca.WebAPI.Models
{
    public class CustomDate
    {
        public string WithoutBars(DateTime date)
        {
            var formattedDate = date.ToString("yyyy-MM-dd-HH-mm-ss");

            return formattedDate;
        }
    }
}
