namespace FantasyTown.Saving
{
    public interface ISaveable
    {
        void CaptureState();
        void RestoreState();
    }
}

