using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace FedoraDev.TimCo.DataManager.Library.Internal.DataAccess
{
	public class SqlDataAccess : IDisposable, ISqlDataAccess
	{
		private IDbConnection _connection;
		private IDbTransaction _transaction;
		private bool _isClosed = true;
		private readonly IConfiguration _configuration;
		private readonly ILogger<SqlDataAccess> _logger;

		public SqlDataAccess(IConfiguration configuration, ILogger<SqlDataAccess> logger)
		{
			_configuration = configuration;
			_logger = logger;
		}

		public string GetConnectionString(string name)
		{
			return _configuration.GetConnectionString(name);
		}

		public List<T> LoadData<T, U>(string connectionStringName, string storedProcedure, U parameters)
		{
			string connectionString = GetConnectionString(connectionStringName);

			using (IDbConnection connection = new SqlConnection(connectionString))
				return connection.Query<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure).ToList();
		}

		public void SaveData<T>(string connectionStringName, string storedProcedure, T parameters)
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
			if (!_isClosed)
				return;

			string connectionString = GetConnectionString(connectionStringName);

			_connection = new SqlConnection(connectionString);
			_connection.Open();
			_transaction = _connection.BeginTransaction();

			_isClosed = false;
		}

		public void CommitTransaction()
		{
			if (_isClosed)
				return;
			_isClosed = true;

			_transaction?.Commit();
			_connection.Close();
		}

		public void RollbackTransaction()
		{
			if (_isClosed)
				return;
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
				_logger.LogError(exception, "Commit transaction failed during dispose.");
			}

			_transaction = null;
			_connection = null;
		}
	}
}
