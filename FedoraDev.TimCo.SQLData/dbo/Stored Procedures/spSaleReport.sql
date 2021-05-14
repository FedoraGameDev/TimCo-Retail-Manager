CREATE PROCEDURE [dbo].[spSaleReport]
AS
begin
	set nocount on

	SELECT [s].[CashierId], [s].[SaleDate], [s].[SubTotal], [s].[Tax], [s].[Total], [u].[FirstName], [u].[LastName], [u].[EmailAddress]
	from [dbo].[Sale] s
	inner join [dbo].[User] u on s.CashierId = u.Id
end
