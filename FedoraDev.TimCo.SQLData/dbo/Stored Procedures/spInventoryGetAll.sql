CREATE PROCEDURE [dbo].[spInventoryGetAll]
AS
begin
	set nocount on

	SELECT [ProductId], [Quantity], [PurchasePrice], [PurchaseDate]
	from [dbo].[Inventory];
end
