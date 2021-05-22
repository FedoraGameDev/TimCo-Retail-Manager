using System.Collections.Generic;

namespace FedoraDev.TimCo.DataManager.Library.Internal.DataAccess
{
	public interface ISqlDataAccess
	{
		void CommitTransaction();
		string GetConnectionString(string name);
		List<T> LoadData<T, U>(string connectionStringName, string storedProcedure, U parameters);
		List<T> LoadDataInTransaction<T, U>(string storedProcedure, U parameters);
		void RollbackTransaction();
		void SaveData<T>(string connectionStringName, string storedProcedure, T parameters);
		void SaveDataInTransaction<T>(string storedProcedure, T parameters);
		void StartTransaction(string connectionStringName);
	}
}