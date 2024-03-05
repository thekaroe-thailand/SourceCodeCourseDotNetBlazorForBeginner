using BlazorBootstrap;
using CurrieTechnologies.Razor.SweetAlert2;

namespace BlazorDatabaseProgramming.Pages;

public partial class ProductApi
{
    private List<ProductModel> list = new List<ProductModel>();
    private HttpClient req = new HttpClient();
    private string? Barcode;
    private string? Name;
    private int Price;
    private int Id;
    private Modal? modal;

    protected override async void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            await FetchData();
        }
    }

    async Task OpenModal()
    {
        await modal!.ShowAsync();
    }

    async Task CloseModal()
    {
        await modal!.HideAsync();
    }

    async Task HandleSave()
    {
        try
        {
            object payload = new
            {
                Barcode = Barcode,
                Name = Name,
                Price = Price,
                Id = Id
            };

            bool IsSuccessStatusCode = false;

            if (Id == 0)
            {
                HttpResponseMessage res = await req.PostAsJsonAsync(
                    Config.apiPath + "/api/Product/Create",
                    payload
                );
                IsSuccessStatusCode = res.IsSuccessStatusCode;
            }
            else
            {
                HttpResponseMessage res = await req.PutAsJsonAsync(
                    Config.apiPath + "/api/Product/Edit",
                    payload
                );
                IsSuccessStatusCode = res.IsSuccessStatusCode;
            }

            if (IsSuccessStatusCode)
            {
                await Swal.FireAsync(
                    new SweetAlertOptions
                    {
                        Title = "Save",
                        Text = "Save Success",
                        Icon = SweetAlertIcon.Success,
                        Timer = 1000
                    }
                );

                await FetchData();
                await CloseModal();

                Id = 0;
            }
        }
        catch (Exception ex)
        {
            await Swal.FireAsync(
                new SweetAlertOptions
                {
                    Title = "Error",
                    Text = ex.Message,
                    Icon = SweetAlertIcon.Error
                }
            );
        }
    }

    async Task FetchData()
    {
        try
        {
            list = (
                await req.GetFromJsonAsync<List<ProductModel>>(Config.apiPath + "/api/Product/List")
            )!;
            StateHasChanged();
        }
        catch (Exception ex)
        {
            await Swal.FireAsync(
                new SweetAlertOptions
                {
                    Title = "Error",
                    Text = ex.Message,
                    Icon = SweetAlertIcon.Error
                }
            );
        }
    }

    async Task Edit(ProductModel productModel)
    {
        await OpenModal();

        Barcode = productModel.Barcode;
        Name = productModel.Name;
        Price = productModel.Price;
        Id = productModel.Id;
    }

    async Task Remove(int id)
    {
        SweetAlertResult button = await Swal.FireAsync(
            new SweetAlertOptions
            {
                Title = "Delete",
                Text = "Confirm for Delete",
                Icon = SweetAlertIcon.Question,
                ShowCancelButton = true,
                ShowConfirmButton = true
            }
        );

        if (button.IsConfirmed)
        {
            HttpResponseMessage res = await req.DeleteAsync(
                Config.apiPath + "/api/Product/Remove/" + id
            );

            if (res.IsSuccessStatusCode)
            {
                await Swal.FireAsync(
                    new SweetAlertOptions
                    {
                        Title = "Delete",
                        Text = "Delete Success",
                        Icon = SweetAlertIcon.Success,
                        Timer = 1000
                    }
                );

                await FetchData();
            }
        }
    }
}
