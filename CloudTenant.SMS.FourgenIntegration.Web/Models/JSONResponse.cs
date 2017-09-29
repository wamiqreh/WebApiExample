using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SND.Models
{
    

        public class JsonItemConverter : Newtonsoft.Json.Converters.CustomCreationConverter<BASE>
    {
        public Type typeD { get; set; }
        public override BASE Create(Type objectType)
        {
            throw new NotImplementedException();
        }

        public BASE Create(Type objectType, JObject jObject)
        {

          
            var type = (string)jObject.Property("Type");
            switch (type)
            {
                case "STR_GOODSRECEIVE01MASTER":
                    return new GOODRECIEVE();
                case "STR_GOODSRECEIVE02PRODUCTS":
                    return new GOODRECIEVE2();
                case "STR_STOCKOUT01MASTER":
                    return new TransferOut();
                case "STR_STOCKOUT02PRODUCTS":
                    return new TransferOut2();
                case "STR_STOCKAUDIT01MASTER":
                    return new StockAudit();
                case "STR_STOCKAUDIT02PRODUCTS":
                    return new StockAudit2();
                case "STR_STOCKIN01MASTER":
                    return new TransferIn();
                case "STR_STOCKIN02PRODUCTS":
                    return new TransferIn2();
                case "STR_STOCKIN03PRODUCTSREJECTION":
                    return new TransferIn3();
                case "STR_INDENT01MASTER":
                    return new Indent();
                case "STR_INDENT02PRODUCTS":
                    return new Indent2();

            }

            throw new ApplicationException(String.Format("The given type {0} is not supported!", type));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // Load JObject from stream
            JObject jObject = JObject.Load(reader);


            // Create target object based on JObject
            var target = Create(objectType, jObject);
            // Populate the object properties
            serializer.Populate(jObject.CreateReader(), target);

            return target;
        }
    }


} 