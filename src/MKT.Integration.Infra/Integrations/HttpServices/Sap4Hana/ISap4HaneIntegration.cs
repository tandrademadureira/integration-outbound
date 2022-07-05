using Refit;
using System.Threading.Tasks;

namespace MKT.Integration.Infra.Integrations.HttpServices.Sap4Hana
{
    public interface ISap4HaneIntegration
    {
        [Get("/sap/opu/odata/sap/API_PRODUCT_SRV/A_Product?sap-language=PT")]
        Task GetTokenProduct();
        [Post("sap/opu/odata/sap/API_PRODUCT_SRV/A_Product?sap-language=PT")]
        Task CreateProduct();
        [Get("sap/opu/odata/sap/API_PRODUCT_SRV/A_Product(Product='{productId}')")]
        Task GetProduct(string productId);

        [Get("sap/opu/odata/sap/API_BUSINESS_PARTNER/A_BusinessPartner")]
        Task GetTokenSeller();
        [Post("sap/opu/odata/sap/API_BUSINESS_PARTNER/A_BusinessPartner")]
        Task CreateSeller();
        [Get("sap/opu/odata/sap/API_BUSINESS_PARTNER/A_BusinessPartner(BusinessPartner='sellerId')")]
        Task GetSeller(string sellerId);
        [Get("sap/opu/odata/sap/API_BUSINESS_PARTNER/A_BusinessPartnerTaxNumber/?$filter=BPTaxNumber eq '{taxnumber}'")]
        Task GetSellerByTaxNumber(string taxNumber);

    }
}
