using IMS.CoreBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.UseCases.Activities.Interfaces
{
    public interface ISellProductUseCase
    {
        Task ExecuteAsync(string salesOrderNumber, Product product, int quantity,double unitPrice, string doneBY);
    }
}
