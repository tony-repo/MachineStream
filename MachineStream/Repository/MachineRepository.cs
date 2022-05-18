using MachineStream.Model;
using System.Data;

namespace MachineStream.Repository
{
    public interface IMachineRepository
    {
        void SaveMachineInfo(MachineInfo info);
    }

    public class MachineRepository : IMachineRepository
    {
        private DataTable _dt;

        private const string Id = "Id";
        private const string MachineId = "MachineId";
        private const string Timestamp = "Timestamp";
        private const string Status = "Status";

        public MachineRepository()
        {
            _dt = new DataTable("MachineInfo");
            _dt.Columns.Add(Id);
            _dt.Columns.Add(MachineId);
            _dt.Columns.Add(Timestamp);
            _dt.Columns.Add(Status);
            _dt.PrimaryKey = new DataColumn[] { _dt.Columns[0] };
        }

        public void SaveMachineInfo(MachineInfo info)
        {
            var existedData = _dt.Rows.Find(info.Id);
            if (existedData != null)
            {
                existedData[Status] = info.Status;
                return;
            }

            DataRow row = _dt.NewRow();
            row[Id] = info.Id;
            row[MachineId] = info.MachineId;
            row[Timestamp] = info.Timestamp;
            row[Status] = info.Status;

            _dt.Rows.Add(row);
        }
    }
}
