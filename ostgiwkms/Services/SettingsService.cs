namespace ostgiwkms.Services
{
    public class SettingsService
    {
        public double TemperatureValue { get; set; }
        public int MinLengthValue { get; set; }
        public int MaxLengthValue { get; set;}

        public void SaveSettingsValue(double temp, int minL, int maxL)
        {
            TemperatureValue = temp;
            MinLengthValue = minL;
            MaxLengthValue = maxL;
        }

        public (double, int, int) GetSettingsInputs()
        {
            return (TemperatureValue, MinLengthValue, MaxLengthValue);
        }
    }
}
