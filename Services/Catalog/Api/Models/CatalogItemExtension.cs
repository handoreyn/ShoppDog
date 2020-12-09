namespace ShoppDog.Services.Catalog.Api.Models
{
    public static class CatalogItemExtension
    {
        public static void FillProductUrl(this CatalogItem item, string picBaseUrl, bool azureStorageEnabled)
        {
            if (item == null) return;
            item.PictureUri = azureStorageEnabled
                ? picBaseUrl + item.PictureFileName
                : picBaseUrl.Replace("[0]", item.Id.ToString());


        }
    }
}