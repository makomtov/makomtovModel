using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiMTModel.Models.Models.View
{
    public class veterinarService
    {


        //שליפת וטרינר
        public VeterinarDetailsView GetVet(string vetName, string vetPhone)
        {

            try
            {
                Entities context = new Entities();
                VeterinarDetailsView veterinarD = null;

                var veterinar = context.veterinarTbl
              .Where(v => v.VeterinarName==vetName && v.VeterinarPhone1==vetPhone).FirstOrDefault();
                if(veterinar!=null)
                {
                    veterinarD = new VeterinarDetailsView();
                    veterinarD.VeterinarName = veterinar.VeterinarName;
                    veterinarD.VeterinarPhone1 = veterinar.VeterinarPhone1;
                    veterinarD.VeterinarId = veterinar.VeterinarId;
                }
                return veterinarD;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            }
    }
}