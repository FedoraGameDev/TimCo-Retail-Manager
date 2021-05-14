CREATE PROCEDURE [dbo].[spProductGetById]
	@Id int
AS
begin
	set nocount on

	SELECT Id, ProductName, [Description], RetailPrice, QuantityInStock, Taxable
	from [dbo].[Product]
	where Id = @Id
end
