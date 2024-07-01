using DataPersistance.Data;

namespace DataPersistance
{
    public interface IDataPersistence
    {
        void LoadData(GameData data);
        void SaveData(GameData data);
    }
}
