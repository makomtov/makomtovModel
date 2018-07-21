using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiMTModel.Models.Models.View
{
    public class PriceService
    {
        public List<PricesView> GetPrices()

        {
            try
            {
                Entities context = new Entities();

               
                List<PricesView> prices = context.PricesTbl
                .OrderBy(px => px.Days).Select(np => new PricesView
                {
                    Id=np.Id,
                    Days = np.Days,
                    Dogs = np.Dogs,
                    Price = np.Price
                }).ToList();
                var orderPrices= prices.OrderBy(c => c.Dogs).ThenBy(n => n.Days);
                return orderPrices.ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void UpdatePrices(List<PricesView> PricesList)
        {
            int update = 0;
            try
            {
                using (Entities context = new Entities())
                {
                    foreach (PricesView price in PricesList)
                    {
                        var pricet = context.Set<PricesTbl>().Find(price.Id);
                        if (pricet != null)
                        {
                            context.Entry(pricet).CurrentValues.SetValues(price);
                        }
                        else
                        {
                            PricesTbl pricesTbl = new PricesTbl();
                            pricesTbl.Days = price.Days;
                            pricesTbl.Dogs = price.Dogs;
                            pricesTbl.Price = price.Price;
                            context.PricesTbl.Add(pricesTbl);

                        }
                    }
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }
}