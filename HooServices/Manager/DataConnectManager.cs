using HooServices.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;

namespace HooServices.Manager
{
    public class DataConnectManager
    {
        #region Public property

        public string GetDatabaseConnection 
        {
            get 
            {
                return ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;
            }
        }

        #endregion

        #region Get Request 

        public OfferList GetAllOffers()
        {
            OfferList offersByPreferences = new OfferList();
       
            using (SqlConnection sqlConnection = new SqlConnection(this.GetDatabaseConnection))
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.CommandText = "spGetAllOffers";// Add SP name
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                    sqlCommand.Connection = sqlConnection;
                    sqlConnection.Open();
                    using (System.Data.Common.DbDataReader reader = sqlCommand.ExecuteReader())
                    {
                        if (reader != null && reader.HasRows)
                        {
                            offersByPreferences.Offers = GetOfferDetailObject(reader);
                           
                        }
                    }
                    if (sqlConnection.State == System.Data.ConnectionState.Open)
                        sqlConnection.Close();
                }
            }

            
            return offersByPreferences;
        }

        public OfferList GetOffersByPreferences(string preferences)
        {
            OfferList offersByPreferences = new OfferList();      

            using (SqlConnection sqlConnection = new SqlConnection(this.GetDatabaseConnection))
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.CommandText = "spGetOfferByCategory";// Add SP name
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                    #region Settings StoredProcedure Parameters

                    SqlParameter parameterPreferences = GetSqlParameter(preferences, "@Category", System.Data.DbType.String, System.Data.ParameterDirection.Input, 200);

                    sqlCommand.Parameters.Add(parameterPreferences);

                    #endregion

                    sqlCommand.Connection = sqlConnection;
                    sqlConnection.Open();
                    using (System.Data.Common.DbDataReader reader = sqlCommand.ExecuteReader())
                    {
                        offersByPreferences.Offers = GetOfferDetailObject(reader);
                        
                    }
                    if (sqlConnection.State == System.Data.ConnectionState.Open)
                        sqlConnection.Close();
                }
            }

    
            return offersByPreferences;
        }

        public OfferList GetOffersByLatLong(double latitude, double longitude)
        {
            OfferList offersByPreferences = new OfferList();


            using (SqlConnection sqlConnection = new SqlConnection(this.GetDatabaseConnection))
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.CommandText = "spGetOfferByLocation";// Add SP name
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                    #region Settings StoredProcedure Parameters

                    SqlParameter parameterLat = GetSqlParameter(latitude, "@Latitude", System.Data.DbType.Double, System.Data.ParameterDirection.Input);
                    SqlParameter parameterLong = GetSqlParameter(longitude, "@Longitude", System.Data.DbType.Double, System.Data.ParameterDirection.Input);

                    sqlCommand.Parameters.Add(parameterLat);
                    sqlCommand.Parameters.Add(parameterLong);

                    #endregion

                    sqlCommand.Connection = sqlConnection;
                    sqlConnection.Open();
                    using (System.Data.Common.DbDataReader reader = sqlCommand.ExecuteReader())
                    {
                        if (reader != null && reader.HasRows)
                        {
                            offersByPreferences.Offers = GetOfferDetailObject(reader);
                        }
                    }
                    if (sqlConnection.State == System.Data.ConnectionState.Open)
                        sqlConnection.Close();
                }
            }
            return offersByPreferences;
        }

        public List<Supplier> GetAllSuppliers()
        {

            List<Supplier> allSuppliers = new List<Supplier>();

            using (SqlConnection sqlConnection = new SqlConnection(this.GetDatabaseConnection))
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.CommandText = "spGetAllSuppliers";// Add SP name
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                    sqlCommand.Connection = sqlConnection;
                    sqlConnection.Open();
                    using (System.Data.Common.DbDataReader reader = sqlCommand.ExecuteReader())
                    {
                        if (reader != null && reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Supplier vendor = new Supplier();
                                vendor.SupplierId = reader.IsDBNull(reader.GetOrdinal("SupplierId")) ? 0 : Convert.ToInt32(reader["SupplierId"], CultureInfo.InvariantCulture);
                                vendor.SupplierName = reader.IsDBNull(reader.GetOrdinal("SupplierName")) ? string.Empty : Convert.ToString(reader["SupplierName"], CultureInfo.InvariantCulture);
                                vendor.ContactNumber = reader.IsDBNull(reader.GetOrdinal("ContactNumber")) ? string.Empty : Convert.ToString(reader["ContactNumber"], CultureInfo.InvariantCulture);
                                vendor.Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? string.Empty : Convert.ToString(reader["Email"], CultureInfo.InvariantCulture);
                                vendor.ImageUrl = reader.IsDBNull(reader.GetOrdinal("ImageUrl")) ? string.Empty : Convert.ToString(reader["ImageUrl"], CultureInfo.InvariantCulture);
                                vendor.SupplierAddress = reader.IsDBNull(reader.GetOrdinal("SupplierAddress")) ? string.Empty : Convert.ToString(reader["SupplierAddress"], CultureInfo.InvariantCulture);
                                    allSuppliers.Add(vendor);
                              
                            }
                        }
                    }
                    if (sqlConnection.State == System.Data.ConnectionState.Open)
                        sqlConnection.Close();
                }
            }

            return allSuppliers;
        }

        #endregion

        #region Insert Request

        public bool InsertSupplier(string supplierName, string supplierAddress, string supplierEmail, string supplierContactnumber, string supplierImage)
        {
            bool isSupplierAdded = false;

            using (SqlConnection sqlConnection = new SqlConnection(this.GetDatabaseConnection))
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.CommandText = "spInsertSupplier";// Add SP name
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                    #region Settings StoredProcedure Parameters

                    SqlParameter parametersupplierName = GetSqlParameter(supplierName, "@SupplierName", System.Data.DbType.String, System.Data.ParameterDirection.Input);
                    SqlParameter parametersupplierAddress = GetSqlParameter(supplierAddress, "@SupplierAddress", System.Data.DbType.String, System.Data.ParameterDirection.Input, 500);
                    SqlParameter parametersupplierEmail = GetSqlParameter(supplierEmail, "@Email", System.Data.DbType.String, System.Data.ParameterDirection.Input);
                    SqlParameter parametersupplierContactnumber = GetSqlParameter(supplierContactnumber, "@ContactNumber", System.Data.DbType.String, System.Data.ParameterDirection.Input);
                    SqlParameter parametersupplierImage = GetSqlParameter(supplierImage, "@ImageUrl", System.Data.DbType.String, System.Data.ParameterDirection.Input, 200);

                    sqlCommand.Parameters.Add(parametersupplierName);
                    sqlCommand.Parameters.Add(parametersupplierAddress);
                    sqlCommand.Parameters.Add(parametersupplierEmail);
                    sqlCommand.Parameters.Add(parametersupplierContactnumber);
                    sqlCommand.Parameters.Add(parametersupplierImage);

                    #endregion

                    sqlCommand.Connection = sqlConnection;
                    sqlConnection.Open();
                    int result = sqlCommand.ExecuteNonQuery();
                    if (result > 0)
                        isSupplierAdded = true;
                    if (sqlConnection.State == System.Data.ConnectionState.Open)
                        sqlConnection.Close();
                }
            }
            return isSupplierAdded;
        }

        public bool InsertOffer(string title, string description, string imagePath, DateTime startDate, DateTime endDate, double latitude, double longitude, int supplierId, string city, string offerCategory, string Comments, string promoCode)
        {
            bool isSupplierAdded = false;

            using (SqlConnection sqlConnection = new SqlConnection(this.GetDatabaseConnection))
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.CommandText = "spInsertOffer";// Add SP name
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                    #region Settings StoredProcedure Parameters

                    SqlParameter parameterTitle = GetSqlParameter(title, "@OfferTitle", System.Data.DbType.String, System.Data.ParameterDirection.Input);
                    SqlParameter parameterdescription = GetSqlParameter(description, "@OfferDescription", System.Data.DbType.String, System.Data.ParameterDirection.Input);
                    SqlParameter parameterimagePath = GetSqlParameter(imagePath, "@ImageUrl", System.Data.DbType.String, System.Data.ParameterDirection.Input);
                    SqlParameter parameterstartDate = GetSqlParameter(startDate, "@StartDate", System.Data.DbType.DateTime, System.Data.ParameterDirection.Input);
                    SqlParameter parameterendDate = GetSqlParameter(endDate, "@ExpireDate", System.Data.DbType.DateTime, System.Data.ParameterDirection.Input);
                    SqlParameter parameterComments = GetSqlParameter(description, "@Comments", System.Data.DbType.String, System.Data.ParameterDirection.Input);
                    SqlParameter parameterLat = GetSqlParameter(latitude, "@Latitude", System.Data.DbType.Double, System.Data.ParameterDirection.Input);
                    SqlParameter parameterLong = GetSqlParameter(longitude, "@Longitude", System.Data.DbType.Double, System.Data.ParameterDirection.Input);

                    SqlParameter parametersupplierId = GetSqlParameter(supplierId, "@SupplierId", System.Data.DbType.Int32, System.Data.ParameterDirection.Input);
                    SqlParameter parametercity = GetSqlParameter(city, "@City", System.Data.DbType.String, System.Data.ParameterDirection.Input);
                    SqlParameter parameterofferCategory = GetSqlParameter(offerCategory, "@OfferCategory", System.Data.DbType.String, System.Data.ParameterDirection.Input);
                    SqlParameter parameterPromoCode = GetSqlParameter(promoCode, "@PromoCode", System.Data.DbType.String, System.Data.ParameterDirection.Input);



                    sqlCommand.Parameters.Add(parameterTitle);
                    sqlCommand.Parameters.Add(parameterdescription);
                    sqlCommand.Parameters.Add(parameterimagePath);
                    sqlCommand.Parameters.Add(parameterstartDate);
                    sqlCommand.Parameters.Add(parameterendDate);
                    sqlCommand.Parameters.Add(parameterComments);
                    sqlCommand.Parameters.Add(parameterLat);
                    sqlCommand.Parameters.Add(parameterLong);
                    sqlCommand.Parameters.Add(parametersupplierId);
                    sqlCommand.Parameters.Add(parametercity);
                    sqlCommand.Parameters.Add(parameterofferCategory);
                    sqlCommand.Parameters.Add(parameterPromoCode);

                    #endregion

                    sqlCommand.Connection = sqlConnection;
                    sqlConnection.Open();
                    int result = sqlCommand.ExecuteNonQuery();
                    if (result > 0)
                        isSupplierAdded = true;
                    if (sqlConnection.State == System.Data.ConnectionState.Open)
                        sqlConnection.Close();
                }
            }
            return isSupplierAdded;
        }

        public bool InsertRating(int OfferId, int ratingValue)
        {
            bool isRatingAdded = false;

            using (SqlConnection sqlConnection = new SqlConnection(this.GetDatabaseConnection))
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.CommandText = "spInsertRating";// Add SP name
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                    #region Settings StoredProcedure Parameters

                    SqlParameter parameterOfferId = GetSqlParameter(OfferId, "@OfferId", System.Data.DbType.Int32, System.Data.ParameterDirection.Input);
                    SqlParameter parameterratingValue = GetSqlParameter(ratingValue, "@Rating", System.Data.DbType.Int32, System.Data.ParameterDirection.Input);

                    sqlCommand.Parameters.Add(parameterOfferId);
                    sqlCommand.Parameters.Add(parameterratingValue);

                    #endregion

                    sqlCommand.Connection = sqlConnection;
                    sqlConnection.Open();
                    int result = sqlCommand.ExecuteNonQuery();
                    if (result > 0)
                        isRatingAdded = true;
                    if (sqlConnection.State == System.Data.ConnectionState.Open)
                        sqlConnection.Close();
                }
            }
            return isRatingAdded;
        }

        public static bool AddError(string errorMessageDetails)
        {
            bool isErrorAdded = false;

            using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.CommandText = "spInsertErrorLogs";// Add SP name
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                    #region Settings StoredProcedure Parameters

                    SqlParameter paramerrorMessageDetails = GetSqlParameter(errorMessageDetails, "@Message", System.Data.DbType.String, System.Data.ParameterDirection.Input);

                    sqlCommand.Parameters.Add(paramerrorMessageDetails);

                    #endregion

                    sqlCommand.Connection = sqlConnection;
                    sqlConnection.Open();
                    int result = sqlCommand.ExecuteNonQuery();
                    if (result > 0)
                        isErrorAdded = true;
                    if (sqlConnection.State == System.Data.ConnectionState.Open)
                        sqlConnection.Close();
                }
            }
            return isErrorAdded;
        }

        public bool InsertOfferHitCount(int OfferId, int supplierId)
        {
            bool isRatingAdded = false;

            using (SqlConnection sqlConnection = new SqlConnection(this.GetDatabaseConnection))
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.CommandText = "spInsertOfferHitCount";// Add SP name
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                    #region Settings StoredProcedure Parameters

                    SqlParameter parameterOfferId = GetSqlParameter(OfferId, "@OfferId", System.Data.DbType.Int32, System.Data.ParameterDirection.Input);
                    SqlParameter parametersupplierId = GetSqlParameter(supplierId, "@SupplierId", System.Data.DbType.Int32, System.Data.ParameterDirection.Input);

                    sqlCommand.Parameters.Add(parameterOfferId);
                    sqlCommand.Parameters.Add(parametersupplierId);

                    #endregion

                    sqlCommand.Connection = sqlConnection;
                    sqlConnection.Open();
                    int result = sqlCommand.ExecuteNonQuery();
                    if (result > 0)
                        isRatingAdded = true;
                    if (sqlConnection.State == System.Data.ConnectionState.Open)
                        sqlConnection.Close();
                }
            }
            return isRatingAdded;
        }

        #endregion
        
        #region Private / internal properties 

        
        internal static List<OfferDetails> GetOfferDetailObject(DbDataReader reader)
        {
          
            OfferDetails hooOffer = null;
            List<OfferDetails> allOffers = new List<OfferDetails>();
            while (reader.Read())
            {
                hooOffer = new OfferDetails();
                hooOffer.OfferId = reader.IsDBNull(reader.GetOrdinal("OfferId")) ? 0 : Convert.ToInt32(reader["OfferId"], CultureInfo.InvariantCulture);
                hooOffer.OfferTitle = reader.IsDBNull(reader.GetOrdinal("OfferTitle")) ? string.Empty : Convert.ToString(reader["OfferTitle"], CultureInfo.InvariantCulture);
                hooOffer.OfferDescription = reader.IsDBNull(reader.GetOrdinal("OfferDescription")) ? string.Empty : Convert.ToString(reader["OfferDescription"], CultureInfo.InvariantCulture);
                hooOffer.ImageUrl = reader.IsDBNull(reader.GetOrdinal("OfferImageUrl")) ? string.Empty : Convert.ToString(reader["OfferImageUrl"], CultureInfo.InvariantCulture);
                hooOffer.StartDate = reader.IsDBNull(reader.GetOrdinal("StartDate")) ? DateTime.Now : Convert.ToDateTime(reader["StartDate"], CultureInfo.InvariantCulture);
                hooOffer.ExpireDate = reader.IsDBNull(reader.GetOrdinal("ExpireDate")) ? DateTime.Now : Convert.ToDateTime(reader["ExpireDate"], CultureInfo.InvariantCulture);
                hooOffer.comments = reader.IsDBNull(reader.GetOrdinal("Comments")) ? string.Empty : Convert.ToString(reader["Comments"], CultureInfo.InvariantCulture);
                hooOffer.Latitude = reader.IsDBNull(reader.GetOrdinal("Latitude")) ? 0 : Convert.ToDouble(reader["Latitude"], CultureInfo.InvariantCulture);
                hooOffer.Longitude = reader.IsDBNull(reader.GetOrdinal("Longitude")) ? 0 : Convert.ToDouble(reader["Longitude"], CultureInfo.InvariantCulture);
                hooOffer.City = reader.IsDBNull(reader.GetOrdinal("City")) ? string.Empty : Convert.ToString(reader["City"], CultureInfo.InvariantCulture);
                hooOffer.OfferCategory = reader.IsDBNull(reader.GetOrdinal("OfferCategory")) ? string.Empty : Convert.ToString(reader["OfferCategory"], CultureInfo.InvariantCulture);
                hooOffer.PromoCode = reader.IsDBNull(reader.GetOrdinal("PromoCode")) ? string.Empty : Convert.ToString(reader["PromoCode"], CultureInfo.InvariantCulture);

                hooOffer.Ratings = reader.IsDBNull(reader.GetOrdinal("SumOfRating")) ? 0 : Convert.ToInt32(reader["SumOfRating"], CultureInfo.InvariantCulture);

                hooOffer.Vendor = new Supplier();
                hooOffer.Vendor.SupplierId = reader.IsDBNull(reader.GetOrdinal("SupplierId")) ? 0 : Convert.ToInt32(reader["SupplierId"], CultureInfo.InvariantCulture);
                hooOffer.Vendor.SupplierName = reader.IsDBNull(reader.GetOrdinal("SupName")) ? string.Empty : Convert.ToString(reader["SupName"], CultureInfo.InvariantCulture);
                hooOffer.Vendor.ContactNumber = reader.IsDBNull(reader.GetOrdinal("SupContactNumber")) ? string.Empty : Convert.ToString(reader["SupContactNumber"], CultureInfo.InvariantCulture);
                hooOffer.Vendor.Email = reader.IsDBNull(reader.GetOrdinal("SupEmailId")) ? string.Empty : Convert.ToString(reader["SupEmailId"], CultureInfo.InvariantCulture);
                hooOffer.Vendor.ImageUrl = ConfigurationManager.AppSettings["ImageFolder"].ToString();
                hooOffer.Vendor.ImageUrl +=  reader.IsDBNull(reader.GetOrdinal("SupImageURL")) ? string.Empty : Convert.ToString(reader["SupImageURL"], CultureInfo.InvariantCulture);
                hooOffer.Vendor.SupplierAddress = reader.IsDBNull(reader.GetOrdinal("SupAddress")) ? string.Empty : Convert.ToString(reader["SupAddress"], CultureInfo.InvariantCulture);
                allOffers.Add(hooOffer);

            }
            reader.Close();

            return allOffers;
        }


        /// <summary>
        /// Function will create SQL parameter
        /// </summary>
        /// <param name="value">Value to set</param>
        /// <param name="parameterName">Name of the parameters</param>
        /// <param name="dbType">Db Type</param>
        /// <param name="direction">Direction of Parameter</param>
        /// <param name="size">Size to be set with default as 20</param>
        /// <returns>Returns sqlParameter object</returns>
        internal static SqlParameter GetSqlParameter(object value, string parameterName, System.Data.DbType dbType, System.Data.ParameterDirection direction, int size = 20)
        {
            SqlParameter sqlparameter = new SqlParameter();
            sqlparameter.ParameterName = parameterName;
            sqlparameter.Value = value;
            sqlparameter.Direction = direction;
            sqlparameter.DbType = dbType;
            sqlparameter.Size = size;
            return sqlparameter;
        }

        #endregion

    }
}