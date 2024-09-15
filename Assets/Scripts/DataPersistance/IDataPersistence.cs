using DataPersistance.Data;

namespace DataPersistance
{
    public interface IDataPersistence
    {
        bool IsLoaded { get; set; }
        void LoadData(GameData data);
        void SaveData(GameData data);
    }
}
