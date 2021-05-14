using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace FedoraDev.TimCo.DataManager.Library.Internal.DataAccess
{
	internal class SqlDataAccess : IDisposable
	{
		private IDbConnection _connection;
		private IDbTransaction _transaction;

		public string GetConnectionString(string name)
		{
			return ConfigurationManager.ConnectionStrings[name].ConnectionString;
		}

		public List<T> LoadData<T, U>(string storedProcedure, U parameters, string connectionStringName)
		{
			string connectionString = GetConnectionString(connectionStringName);

			using (IDbConnection connection = new SqlConnection(connectionString))
				return connection.Query<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure).ToList();
		}

		public void SaveData<T>(string storedProcedure, T parameters, string connectionStringName)
		{
			string connectionString = GetConnectionString(connectionStringName);

			using (IDbConnection connection = new SqlConnection(connectionString))
				_ = connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
		}

		public List<T> LoadDataInTransaction<T, U>(string storedProcedure, U parameters)
		{
			return _connection.Query<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure, transaction: _transaction).ToList();
		}

		public void SaveDataInTransaction<T>(string storedProcedure, T parameters)
		{
			_ = _connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure, transaction: _transaction);
		}

		public void StartTransaction(string connectionStringName)
		{
			string connectionString = GetConnectionString(connectionStringName);

			_connection = new SqlConnection(connectionString);
			_connection.Open();
			_transaction = _connection.BeginTransaction();
		}

		public void CommitTransaction()
		{
			_transaction?.Commit();
			_connection?.Close();
		}

		public void RollbackTransaction()
		{
			_transaction?.Rollback();
			_connection?.Close();
		}

		public void Dispose()
		{
			CommitTransaction();
		}
	}
}
