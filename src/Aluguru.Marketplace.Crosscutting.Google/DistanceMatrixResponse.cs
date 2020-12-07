namespace Aluguru.Marketplace.Crosscutting.Google
{
    public class DistanceMatrixResponse
    {
        public DistanceMatrixResponse(bool success)
        {
            Success = success;
        }

        public DistanceMatrixResponse(bool success, double distance, int duration)
        {
            Success = success;
            Distance = distance;
            Duration = duration;
        }

        public bool Success { get; private set; }
        public double Distance { get; private set; }
        public int Duration { get; private set; }

    }
}
