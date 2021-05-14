CREATE PROCEDURE [dbo].[spInventoryGetAll]
AS
begin
	set nocount on

	SELECT [Id], [ProductId], [Quantity], [PurchasePrice], [PurchaseDate]
	from [dbo].[Inventory];
end
