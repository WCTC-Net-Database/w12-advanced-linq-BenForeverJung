--  Seed an Inventories for the default player Sir Lancelot

SET IDENTITY_INSERT [dbo].[Inventories] ON
INSERT INTO [dbo].[Inventories] ([Id], [PlayerId]) VALUES (1, 1)
SET IDENTITY_INSERT [dbo].[Inventories] OFF

--UPDATE Players SET InventoryId = 1 WHERE Id = 1
