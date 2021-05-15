using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace FedoraDev.TimCo.DataManager.Library.Internal.DataAccess
{
	internal class SqlDataAccess : IDisposable
	{
		private IDbConnection _connection;
		private IDbTransaction _transaction;
		private bool _isClosed = true;
		private readonly IConfiguration _configuration;

		public SqlDataAccess(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public string GetConnectionString(string name)
		{
			return _configuration.GetConnectionString(name);
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
			if (!_isClosed) return;

			string connectionString = GetConnectionString(connectionStringName);

			_connection = new SqlConnection(connectionString);
			_connection.Open();
			_transaction = _connection.BeginTransaction();

			_isClosed = false;
		}

		public void CommitTransaction()
		{
			if (_isClosed) return;
			_isClosed = true;

			_transaction?.Commit();
			_connection.Close();
		}

		public void RollbackTransaction()
		{
			if (_isClosed) return;
			_isClosed = true;

			_transaction?.Rollback();
			_connection.Close();
		}

		public void Dispose()
		{
			try
			{
				CommitTransaction();
			}
			catch (Exception exception)
			{
				Console.WriteLine(exception.StackTrace);
				Console.WriteLine(exception.Message);
				throw;
			}

			_transaction = null;
			_connection = null;
		}
	}
}
