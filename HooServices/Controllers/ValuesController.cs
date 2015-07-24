using HooServices.Manager;
using HooServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace HooServices.Controllers
{
    public class ValuesController : ApiController
    {

        #region Default Properties

        //// GET api/values
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/values/5
        //public string Get(int id)
        //{
        //    return "Hi You have send " + id;
        //}

        //// POST api/values
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/values/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/values/5
        //public void Delete(int id)
        //{
        //}
        #endregion

        #region Get Http Requests

        /// <summary>
        ///  Offers by location
        /// </summary>
        /// <param name="latitude">pass latitude </param>
        /// <param name="longitude">pass longitude</param>
        /// <returns></returns>
        [ResponseType(typeof(OfferList))]
        [HttpGet]
        public OfferList GetOffersByLatLong(double latitude, double longitude)
        {
            OfferList offers = new OfferList();

            try
            {
                DataConnectManager dbManager = new Manager.DataConnectManager();
                offers = dbManager.GetOffersByLatLong(latitude, longitude);
            }
            catch (Exception ex)
            {
                DataConnectManager.AddError("Method Name:GetOffersByLatLong  Details:  " + ex.ToString());
            }
            return offers;
        }

        /// <summary>
        /// Get offers by preferences 
        /// </summary>
        /// <param name="preferences">Comma seperated preferences</param>
        /// <returns> Offers list</returns>
        [ResponseType(typeof(OfferList))]
        [HttpGet]
        public OfferList GetOffersByPreferences(string preferences)
        {
            OfferList offers = new OfferList();

            try
            {
                HooServices.Manager.DataConnectManager dbManager = new Manager.DataConnectManager();
                offers = dbManager.GetOffersByPreferences(preferences);
            }
            catch (Exception ex)
            {
                DataConnectManager.AddError("Method Name:GetOffersByPreferences  Details:  " + ex.ToString());
            }
            return offers;
        }

        /// <summary>
        /// Get offers by preferences 
        /// </summary>
        /// <param name="preferences">Comma seperated preferences</param>
        /// <returns> Offers list</returns>
        [ResponseType(typeof(List<Supplier>))]
        [HttpGet]
        public List<Supplier> GetAllSuppliers()
        {
            List<Supplier> suppliers = new List<Supplier>();

            try
            {
                HooServices.Manager.DataConnectManager dbManager = new Manager.DataConnectManager();
                suppliers = dbManager.GetAllSuppliers();
            }
            catch (Exception ex)
            {
                DataConnectManager.AddError("Method Name:GetAllSuppliers  Details:  " + ex.ToString());
            }
            return suppliers;
        }

        /// <summary>
        /// Get offers by preferences 
        /// </summary>
        /// <param name="preferences">Comma seperated preferences</param>
        /// <returns> Offers list</returns>
        [ResponseType(typeof(OfferList))]
        [HttpGet]
        public OfferList GetAllOffers()
        {
            OfferList offers = new OfferList();

            try
            {
                HooServices.Manager.DataConnectManager dbManager = new Manager.DataConnectManager();
                offers = dbManager.GetAllOffers();
            }
            catch (Exception ex)
            {
                DataConnectManager.AddError("Method Name:GetAllOffers  Details:  " + ex.ToString());
            }
            return offers;
        }
 

        #endregion

        #region Post Http Requests

        /// <summary>
        /// Add Offers 
        /// </summary>
        /// <param name="offerValues"></param>
        /// <returns></returns>
        [HttpPost]
        public bool AddOffer([FromBody]OfferDetails offerValues)
        {
            bool isOfferAdded = false;

            try
            {
                HooServices.Manager.DataConnectManager dbManager = new Manager.DataConnectManager();
                isOfferAdded = dbManager.InsertOffer(offerValues.OfferTitle,
                                                                            offerValues.OfferDescription,
                                                                            offerValues.ImageUrl,
                                                                            offerValues.StartDate,
                                                                            offerValues.ExpireDate,
                                                                            offerValues.Latitude,
                                                                            offerValues.Longitude,
                                                                            (offerValues.Vendor != null) ? offerValues.Vendor.SupplierId : 0,
                                                                            offerValues.City,
                                                                            offerValues.OfferCategory,
                                                                            offerValues.comments,
                                                                            offerValues.PromoCode);
            }
            catch (Exception ex)
            {
                DataConnectManager.AddError("Method Name:AddOffer  Details:  " + ex.ToString());
            }
            return isOfferAdded;
        }

        /// <summary>
        /// Add suppliers
        /// </summary>
        /// <param name="supplierDetails"></param>
        /// <returns></returns>
        [HttpPost]
        public bool AddSupplier([FromBody]Supplier supplierDetails)
        {
            bool isOfferAdded = false;

            try
            {
                HooServices.Manager.DataConnectManager dbManager = new Manager.DataConnectManager();
                isOfferAdded = dbManager.InsertSupplier(supplierDetails.SupplierName, supplierDetails.SupplierAddress, supplierDetails.Email, supplierDetails.ContactNumber, supplierDetails.ImageUrl);
            }
            catch (Exception ex)
            {
                DataConnectManager.AddError("Method Name:AddSupplier  Details:  " + ex.ToString());
            }
            return isOfferAdded;
        }


        /// <summary>
        /// Add Rating for Supplier
        /// </summary>
        /// <param name="supplierDetails"></param>
        /// <returns></returns>
        [HttpPost]
        public bool AddRating([FromBody]OfferRatings ratingDetails)
        {
            bool isratingAdded = false;

            try
            {
                HooServices.Manager.DataConnectManager dbManager = new Manager.DataConnectManager();
                isratingAdded = dbManager.InsertRating(ratingDetails.OfferId, ratingDetails.RatingValue);
            }
            catch (Exception ex)
            {
                DataConnectManager.AddError("Method Name:AddRating  Details:  " + ex.ToString());
            }
            return isratingAdded;
        }

        /// <summary>
        /// Add Click count
        /// </summary>
        /// <param name="supplierDetails"></param>
        /// <returns></returns>
        [HttpPost]
        public bool AddOfferHitCount([FromBody]VenderCallCount VendorCallCountDetail)
        {
            bool isHitCountAdded = false;

            try
            {
                HooServices.Manager.DataConnectManager dbManager = new Manager.DataConnectManager();
                isHitCountAdded = dbManager.InsertOfferHitCount(VendorCallCountDetail.OfferId, VendorCallCountDetail.SupplierId);
            }
            catch (Exception ex)
            {
                DataConnectManager.AddError("Method Name:AddOfferHitCount  Details:  " + ex.ToString());
            }
            return isHitCountAdded;
        }

        #endregion

    }

}
