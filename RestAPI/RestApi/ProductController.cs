using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace RestApi;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    [HttpGet]
    [Route("[action]")]
    public IActionResult List()
    {
        try
        {
            using NpgsqlConnection conn = new MyConnect().GetConnection();
            using NpgsqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM tb_product ORDER BY id DESC";

            using NpgsqlDataReader reader = cmd.ExecuteReader();
            List<ProductModel> list = new List<ProductModel>();

            while (reader.Read())
            {
                list.Add(
                    new ProductModel
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        Barcode = reader["barcode"].ToString(),
                        Name = reader["name"].ToString(),
                        Price = Convert.ToInt32(reader["price"])
                    }
                );
            }

            return Ok(list);
        }
        catch (Exception ex)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new { message = ex.Message }
            );
        }
    }

    [HttpPost]
    [Route("[action]")]
    public IActionResult Create(ProductModel productModel)
    {
        try
        {
            using NpgsqlConnection conn = new MyConnect().GetConnection();
            using NpgsqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = """
                INSERT INTO tb_product(barcode, name, price) 
                VALUES(@barcode, @name, @price)
            """;
            cmd.Parameters.AddWithValue("barcode", productModel.Barcode!);
            cmd.Parameters.AddWithValue("name", productModel.Name!);
            cmd.Parameters.AddWithValue("price", productModel.Price);

            if (cmd.ExecuteNonQuery() != 0)
            {
                return Ok(new { message = "success" });
            }

            return StatusCode(StatusCodes.Status501NotImplemented);
        }
        catch (Exception ex)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new { message = ex.Message }
            );
        }
    }

    [HttpPut]
    [Route("[action]")]
    public IActionResult Edit(ProductModel productModel)
    {
        try
        {
            using NpgsqlConnection conn = new MyConnect().GetConnection();
            using NpgsqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = """
                UPDATE tb_product SET 
                    barcode = @barcode, 
                    name = @name,
                    price = @price
                WHERE id = @id
            """;
            cmd.Parameters.AddWithValue("barcode", productModel.Barcode!);
            cmd.Parameters.AddWithValue("name", productModel.Name!);
            cmd.Parameters.AddWithValue("price", productModel.Price);
            cmd.Parameters.AddWithValue("id", productModel.Id);

            if (cmd.ExecuteNonQuery() != 0)
            {
                return Ok(new { message = "success" });
            }

            return StatusCode(StatusCodes.Status501NotImplemented);
        }
        catch (Exception ex)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new { message = ex.Message }
            );
        }
    }

    [HttpDelete]
    [Route("[action]/{id}")]
    public IActionResult Remove(int id)
    {
        try
        {
            using NpgsqlConnection conn = new MyConnect().GetConnection();
            using NpgsqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM tb_product WHERE id = @id";
            cmd.Parameters.AddWithValue("id", id);

            if (cmd.ExecuteNonQuery() != 0)
            {
                return Ok(new { message = "success" });
            }

            return StatusCode(StatusCodes.Status501NotImplemented);
        }
        catch (Exception ex)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new { message = ex.Message }
            );
        }
    }
}
