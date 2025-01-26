using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Web.Migrations.InventoryDb
{
    /// <inheritdoc />
    public partial class InsertGetProductSP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = """
                    CREATE OR ALTER PROCEDURE GetProducts
                    @PageIndex INT,
                    @PageSize INT,
                    @OrderBy NVARCHAR(50),
                    @Name NVARCHAR(MAX) = '%',
                    @Barcode NVARCHAR(MAX) = '%',
                    @CategoryId UNIQUEIDENTIFIER = NULL,
                    @Status BIT = NULL,
                    @Tax DECIMAL(18, 2) = NULL,
                    @PriceWithTax DECIMAL(18, 2) = NULL,
                    @StockQuantity INT = NULL,
                    @Total INT OUTPUT,
                    @TotalDisplay INT OUTPUT
                AS
                BEGIN
                    SET NOCOUNT ON;

                    DECLARE @sql NVARCHAR(MAX);
                    DECLARE @countSql NVARCHAR(MAX);
                    DECLARE @paramList NVARCHAR(MAX);
                    DECLARE @countParamList NVARCHAR(MAX);

                    -- Collect Total Record Count
                    SELECT @Total = COUNT(*) FROM Products;

                    -- Collect Total Display Count
                    SET @countSql = '
                        SELECT @TotalDisplay = COUNT(*)
                        FROM Products p
                        INNER JOIN Categories c ON p.CategoryId = c.Id
                        WHERE 1 = 1';

                    SET @countSql = @countSql + ' AND p.Name LIKE ''%'' + @xName + ''%''';
                    SET @countSql = @countSql + ' AND p.Barcode LIKE ''%'' + @xBarcode + ''%''';
                    IF @CategoryId IS NOT NULL
                        SET @countSql = @countSql + ' AND p.CategoryId = @xCategoryId';
                    IF @Status IS NOT NULL
                        SET @countSql = @countSql + ' AND p.Status = @xStatus';
                    IF @Tax IS NOT NULL
                        SET @countSql = @countSql + ' AND p.Tax = @xTax';
                    IF @PriceWithTax IS NOT NULL
                        SET @countSql = @countSql + ' AND p.SellingWithTax = @xPriceWithTax';
                    IF @StockQuantity IS NOT NULL
                        SET @countSql = @countSql + ' AND p.StockQuantity = @xStockQuantity';

                    SELECT @countParamList = '
                        @xName NVARCHAR(MAX),
                        @xBarcode NVARCHAR(MAX),
                        @xCategoryId UNIQUEIDENTIFIER,
                        @xStatus BIT,
                        @xTax DECIMAL(18, 2),
                        @xPriceWithTax DECIMAL(18, 2),
                        @xStockQuantity INT,
                        @TotalDisplay INT OUTPUT';

                    EXEC sp_executesql @countSql, @countParamList,
                        @Name,
                        @Barcode,
                        @CategoryId,
                        @Status,
                        @Tax,
                        @PriceWithTax,
                        @StockQuantity,
                        @TotalDisplay = @TotalDisplay OUTPUT;

                    -- Collect Data
                    SET @sql = '
                        SELECT 
                            p.Id,
                            p.Name,
                            p.Barcode,
                            c.Name AS CategoryName,
                            p.Tax,
                            p.SellingWithTax,
                            p.StockQuantity,
                            p.Status
                        FROM Products p
                        INNER JOIN Categories c ON p.CategoryId = c.Id
                        WHERE 1 = 1';

                    SET @sql = @sql + ' AND p.Name LIKE ''%'' + @xName + ''%''';
                    SET @sql = @sql + ' AND p.Barcode LIKE ''%'' + @xBarcode + ''%''';
                    IF @CategoryId IS NOT NULL
                        SET @sql = @sql + ' AND p.CategoryId = @xCategoryId';
                    IF @Status IS NOT NULL
                        SET @sql = @sql + ' AND p.Status = @xStatus';
                    IF @Tax IS NOT NULL
                        SET @sql = @sql + ' AND p.Tax = @xTax';
                    IF @PriceWithTax IS NOT NULL
                        SET @sql = @sql + ' AND p.SellingWithTax = @xPriceWithTax';
                    IF @StockQuantity IS NOT NULL
                        SET @sql = @sql + ' AND p.StockQuantity = @xStockQuantity';

                    SET @sql = @sql + ' ORDER BY ' + @OrderBy + ' OFFSET @PageSize * (@PageIndex - 1) ROWS FETCH NEXT @PageSize ROWS ONLY';

                    SELECT @paramList = '
                        @xName NVARCHAR(MAX),
                        @xBarcode NVARCHAR(MAX),
                        @xCategoryId UNIQUEIDENTIFIER,
                        @xStatus BIT,
                        @xTax DECIMAL(18, 2),
                        @xPriceWithTax DECIMAL(18, 2),
                        @xStockQuantity INT,
                        @PageIndex INT,
                        @PageSize INT';

                    EXEC sp_executesql @sql, @paramList,
                        @Name,
                        @Barcode,
                        @CategoryId,
                        @Status,
                        @Tax,
                        @PriceWithTax,
                        @StockQuantity,
                        @PageIndex,
                        @PageSize;

                    PRINT @sql;
                    PRINT @countSql;
                END
                GO
                
                """;

            migrationBuilder.Sql(sql);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var sql = "DROP PROCEDURE [dbo].[GetProducts]";
            migrationBuilder.DropTable(sql);
        }
    }
}
