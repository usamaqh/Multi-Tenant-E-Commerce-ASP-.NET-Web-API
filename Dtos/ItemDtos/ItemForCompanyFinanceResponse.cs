namespace Multi_Tenant_E_Commerce_API.Dtos.ItemDtos
{
    public class ItemForCompanyFinanceResponse
    {
        public string Name { get; set; }
        public int InStock { get; set; }
        public decimal Price { get; set; }
        public int SoldUnits { get; set; }
        public decimal Revenue { get; set; }
    }
}
