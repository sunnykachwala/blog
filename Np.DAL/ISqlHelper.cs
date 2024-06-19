namespace Np.DAL
{
    using Microsoft.Data.SqlClient;
    using System.Data;
    using System.Data.Odbc;
    using System.Xml;

    public interface ISqlHelper
    {
        string GetConnectionString();
        string GetConnectionString(string connectionstring);

        int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText);
        int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters);

        int ExecuteNonQuery(string connectionString, string spName, params object[] parameterValues);
        int ExecuteNonQuery(SqlConnection connection, CommandType commandType, string commandText);

        int ExecuteNonQuery(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters);
        int ExecuteNonQuery(SqlConnection connection, string spName, params object[] parameterValues);
        int ExecuteNonQuery(SqlTransaction transaction, CommandType commandType, string commandText);
        int ExecuteNonQuery(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters);
        int ExecuteNonQuery(SqlTransaction transaction, string spName, params object[] parameterValues);
        int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText, SqlTransaction transaction);
        int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText, SqlTransaction transaction, string[] parameters, object[] values);
        int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText, string[] parameters, object[] values);
        DataTable ExecuteDataTable(string connectionString, CommandType commandType, string commandText);
        DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText);
        DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters);
        DataSet ExecuteDataset(string connectionString, string spName, params object[] parameterValues);
        DataSet ExecuteDataset(SqlConnection connection, CommandType commandType, string commandText);
        DataSet ExecuteDataset(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters);
        DataSet ExecuteDataset(SqlConnection connection, string spName, params object[] parameterValues);
        DataSet ExecuteDataset(SqlTransaction transaction, CommandType commandType, string commandText);
        DataSet ExecuteDataset(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters);
        DataSet ExecuteDataset(SqlTransaction transaction, string spName, params object[] parameterValues);

        SqlDataReader ExecuteReader(string connectionString, CommandType commandType, string commandText);
        SqlDataReader ExecuteReader(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters);
        SqlDataReader ExecuteReader(string connectionString, string spName, params object[] parameterValues);

        SqlDataReader ExecuteReader(SqlConnection connection, CommandType commandType, string commandText);

        SqlDataReader ExecuteReader(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters);
        SqlDataReader ExecuteReader(SqlConnection connection, string spName, params object[] parameterValues);
        SqlDataReader ExecuteReader(SqlTransaction transaction, CommandType commandType, string commandText);

        SqlDataReader ExecuteReader(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters);

        SqlDataReader ExecuteReader(SqlTransaction transaction, string spName, params object[] parameterValues);
        object ExecuteScalar(string connectionString, CommandType commandType, string commandText);
        object ExecuteScalar(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters);
        object ExecuteScalar(string connectionString, string spName, params object[] parameterValues);
        object ExecuteScalar(SqlConnection connection, CommandType commandType, string commandText);
        object ExecuteScalar(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters);
        object ExecuteScalar(SqlConnection connection, string spName, params object[] parameterValues);
        object ExecuteScalar(SqlTransaction transaction, CommandType commandType, string commandText);
        object ExecuteScalar(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters);
        object ExecuteScalar(SqlTransaction transaction, string spName, params object[] parameterValues);
        XmlReader ExecuteXmlReader(SqlConnection connection, CommandType commandType, string commandText);
        XmlReader ExecuteXmlReader(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters);
        XmlReader ExecuteXmlReader(SqlConnection connection, string spName, params object[] parameterValues);
        XmlReader ExecuteXmlReader(SqlTransaction transaction, CommandType commandType, string commandText);
        XmlReader ExecuteXmlReader(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters);
        XmlReader ExecuteXmlReader(SqlTransaction transaction, string spName, params object[] parameterValues);
        void FillDataset(string connectionString, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames);
        void FillDataset(string connectionString, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames, params SqlParameter[] commandParameters);
        void FillDataset(string connectionString, string spName, DataSet dataSet, string[] tableNames, params object[] parameterValues);
        void FillDataset(SqlConnection connection, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames);
        void FillDataset(SqlConnection connection, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames, params SqlParameter[] commandParameters);
        void FillDataset(SqlConnection connection, string spName, DataSet dataSet, string[] tableNames, params object[] parameterValues);
        void FillDataset(SqlTransaction transaction, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames);
        void FillDataset(SqlTransaction transaction, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames, params SqlParameter[] commandParameters);
        void FillDataset(SqlTransaction transaction, string spName, DataSet dataSet, string[] tableNames, params object[] parameterValues);
        void UpdateDataset(SqlCommand insertCommand, SqlCommand deleteCommand, SqlCommand updateCommand, DataSet dataSet, string tableName);
        SqlCommand CreateCommand(SqlConnection connection, string spName, params string[] sourceColumns);
        int ExecuteNonQueryTypedParams(String connectionString, String spName, DataRow dataRow);
        int ExecuteNonQueryTypedParams(SqlConnection connection, String spName, DataRow dataRow);
        int ExecuteNonQueryTypedParams(SqlTransaction transaction, String spName, DataRow dataRow);

        DataSet ExecuteDatasetTypedParams(string connectionString, String spName, DataRow dataRow);

        DataSet ExecuteDatasetTypedParams(SqlConnection connection, String spName, DataRow dataRow);
        DataSet ExecuteDatasetTypedParams(SqlTransaction transaction, String spName, DataRow dataRow);
        SqlDataReader ExecuteReaderTypedParams(String connectionString, String spName, DataRow dataRow);
        SqlDataReader ExecuteReaderTypedParams(SqlConnection connection, String spName, DataRow dataRow);
        SqlDataReader ExecuteReaderTypedParams(SqlTransaction transaction, String spName, DataRow dataRow);
        object ExecuteScalarTypedParams(String connectionString, String spName, DataRow dataRow);
        object ExecuteScalarTypedParams(SqlConnection connection, String spName, DataRow dataRow);
        object ExecuteScalarTypedParams(SqlTransaction transaction, String spName, DataRow dataRow);

        XmlReader ExecuteXmlReaderTypedParams(SqlConnection connection, String spName, DataRow dataRow);
        XmlReader ExecuteXmlReaderTypedParams(SqlTransaction transaction, String spName, DataRow dataRow);
        DataTable ExecuteDataTableOdbc(string connectionString, CommandType commandType, string commandText);

        OdbcDataReader ExecuteDataReaderOdbc(string connectionString, CommandType commandType, string commandText);

        int ExecuteNonQueryOdbc(string connectionString, CommandType commandType, string commandText, OdbcTransaction transaction);
        DataTable CreateDataTable(string primary_Key, string[] parameters, string tblName);
        DataTable CreateDataTable(string primary_Key, string[] parameters);
        DataTable CreateDataTable(DataTable dtt, string primary_Key, string[] parameters, object[] values, string tblName);
        DataTable CreateDataTable(DataTable dtt, string primary_Key, string[] parameters, object[] values);

    }
}
