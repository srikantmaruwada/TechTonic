using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HooServices.Models
{
    [Newtonsoft.Json.JsonObject]
    [Serializable]
    public class OfferDetails
    {
        public int OfferId;
        public string OfferTitle;
        public string OfferDescription;
        public double Latitude;
        public double Longitude;
        public string ImageUrl;
        public DateTime StartDate;
        public DateTime ExpireDate;
        public string comments;
        public Supplier Vendor;
        public string City;
        public string OfferCategory;
        public int Ratings;
        public string PromoCode;

    }

    [Newtonsoft.Json.JsonObject]
    [Serializable]
    public class OfferList
    {
        public List<OfferDetails> Offers;
    }

    [Newtonsoft.Json.JsonObject]
    [Serializable]
    public class Supplier
    {
        public int SupplierId;
        public string SupplierName;
        public string SupplierAddress;
        public string Email;
        public string ContactNumber;
        public string ImageUrl;
    }


    [Newtonsoft.Json.JsonObject]
    [Serializable]
    public class OfferRatings
    {
        public int OfferId;
        public int RatingValue;
    }

    [Newtonsoft.Json.JsonObject]
    [Serializable]
    public class VenderCallCount
    {
        public int OfferId;
        public int SupplierId;
    }


}